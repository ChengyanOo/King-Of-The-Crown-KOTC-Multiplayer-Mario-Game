using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.States.ActionStates; 
using Sprint1.Collisions;
using Sprint1.Factories.SpriteFactories;
using Sprint1.States.EnemyStates;
using Sprint1.Trackers;
using Sprint1.Audio;
using System.Net.NetworkInformation;

namespace Sprint1.States.CrownStates
{
    public abstract class CrownState : ICrownState
    {
        protected CrownEntity entity;
        protected ICrownState previousState;
        public event EventHandler<PositionEventArgs> SetPosition;
        PositionEventArgs PositionEventArgs;

        public CrownState(CrownEntity entity, ICrownState previousState)
        {
            this.entity = entity;
            this.previousState = previousState;
            
        }

        public void toFloating()
        {
            entity.playerEntity = null;
            entity.Set(SpriteEnum.crown | SpriteEnum.floating);
        }

        public void toThrown()
        {
            entity.Set(SpriteEnum.crown | SpriteEnum.thrown);
        }

        public void toAttached(PlayerEntity playerEntity)
        {
            entity.playerEntity = playerEntity;
            entity.Set(SpriteEnum.crown | SpriteEnum.attached);
        }

        public virtual void Enter(ICrownState previousState)
        {
            this.previousState = previousState;
        }
        public virtual void Exit() { }

        public virtual void collision (ICollidable collidee, int direction) { }
    }
}