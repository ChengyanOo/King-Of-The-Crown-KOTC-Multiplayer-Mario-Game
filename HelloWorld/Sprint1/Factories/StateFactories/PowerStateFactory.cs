using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.States.ActionStates;
using Sprint1.States.PowerStates;
using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using static System.Collections.Specialized.BitVector32;
using System.Diagnostics;

namespace Sprint1.Factories.StateFactories
{
    public class PowerStateFactory
    {
        private Dictionary<SpriteEnum, IPowerState> powerStates;
        private PlayerEntity entity;

        public PowerStateFactory (PlayerEntity entity)
        {
            powerStates = new Dictionary<SpriteEnum, IPowerState>();
            this.entity = entity;
        }

        public IPowerState Create(SpriteEnum spriteType, IPowerState previousPowerState)
        {
            IPowerState powerState = null;
            if ((SpriteEnum.player & spriteType) == SpriteEnum.player)
            {
                SpriteEnum power = (SpriteEnum.allPowers & spriteType);
                if (power != SpriteEnum.player)
                {
                    if (!powerStates.ContainsKey(power))
                    {
                        switch (power)
                        {
                            case SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.dead:
                            case SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.dead:
                                powerStates.Add(power, new DeadState(entity, previousPowerState));
                                break;
                            case SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.small:
                            case SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.small:
                                powerStates.Add(power, new SmallState(entity, previousPowerState));
                                break;
                            case SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.super:
                            case SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.super:
                                powerStates.Add(power, new SuperState(entity, previousPowerState));
                                break;
                            case SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.fire:
                            case SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.fire:
                                powerStates.Add(power, new FireState(entity, previousPowerState));
                                break;
                        }
                    }
                    bool b = powerStates.TryGetValue(power, out powerState);
                    Debug.WriteLineIf(!b, "Power state failure: " + ((int)power).ToString("X8"));
                }
            }
            return powerState;
        }
    }
}