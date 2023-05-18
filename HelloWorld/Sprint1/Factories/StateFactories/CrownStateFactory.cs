using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.States.CrownStates;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Sprites;

namespace Sprint1.Factories.StateFactories
{
    public class CrownStateFactory
    {
        private CrownEntity entity;

        public CrownStateFactory(CrownEntity entity)
        {
            this.entity = entity;
        }

        public ICrownState Create(SpriteEnum spriteType, ICrownState previousCrownState)
        {
            ICrownState crownState = null;
            if ((SpriteEnum.crown & spriteType) == SpriteEnum.crown)
            {
                SpriteEnum crown = (SpriteEnum.allCrowns & spriteType);
                if (crown != SpriteEnum.crown)
                {
                    switch (crown)
                    {
                        case SpriteEnum.crown | SpriteEnum.floating:
                            crownState = new FloatingCrownState(entity, previousCrownState);
                            break;
                        case SpriteEnum.crown | SpriteEnum.thrown:
                            crownState = new ThrownCrownState(entity, previousCrownState);
                            break;
                        case SpriteEnum.crown | SpriteEnum.attached:
                            crownState = new AttachedCrownState(entity, previousCrownState);
                            break;
                    }
                }
            }
            return crownState;
        }
    }
}