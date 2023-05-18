using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.States.ActionStates;
using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace Sprint1.Factories.StateFactories
{
    public class ActionStateFactory
    {
        private Dictionary<SpriteEnum, IActionState> actionStates;
        private PlayerEntity entity;

        public ActionStateFactory (PlayerEntity entity)
        {
            actionStates = new Dictionary<SpriteEnum, IActionState>();
            this.entity = entity;
        }

        public IActionState Create(SpriteEnum spriteType, IActionState previousActionState)
        {
            IActionState actionState = null;
            if ((SpriteEnum.player & spriteType) == SpriteEnum.player)
            {
                if ((spriteType & SpriteEnum.allPowers) == (SpriteEnum.player | SpriteEnum.dead))
                {
                    if (!actionStates.ContainsKey(SpriteEnum.player | SpriteEnum.dead))
                    {
                        actionStates.Add(SpriteEnum.player | SpriteEnum.dead, new DeadActionState(entity, previousActionState));
                    }
                    return actionStates[SpriteEnum.player | SpriteEnum.dead];
                }

                SpriteEnum action = (SpriteEnum.allActions & spriteType);
                if (action != SpriteEnum.player)
                {
                    if (!actionStates.ContainsKey(action))
                    {
                        switch (action)
                        {
                            case SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.crouching:
                            case SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.crouching:
                                actionStates.Add(action, new CrouchingState(entity, previousActionState));
                                break;
                            case SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.falling:
                            case SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.falling:
                                actionStates.Add(action, new FallingState(entity, previousActionState));
                                break;
                            case SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.idle:
                            case SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.idle:
                                actionStates.Add(action, new IdleState(entity, previousActionState));
                                break;
                            case SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.running:
                            case SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.running:
                                actionStates.Add(action, new RunningState(entity, previousActionState));
                                break;
                            case SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.jumping:
                            case SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.jumping:
                                actionStates.Add(action, new JumpingState(entity, previousActionState));
                                break;
                        }
                    }
                    bool b = actionStates.TryGetValue(action, out actionState);
                    Debug.WriteLineIf(!b, "Action state failure: " + ((int)action).ToString("X8"));
                }
            }
            return actionState;
        }
    }
}