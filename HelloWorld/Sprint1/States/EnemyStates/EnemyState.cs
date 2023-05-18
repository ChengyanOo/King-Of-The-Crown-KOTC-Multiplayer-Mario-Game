using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Collisions;
using System.Diagnostics;
using Sprint1.Trackers;
using Sprint1.Audio;

namespace Sprint1.States.EnemyStates
{
    public abstract class EnemyState : IEnemyState
    {
        protected EnemyEntity entity;
        protected IEnemyState previousState;
        public event EventHandler<SoundEffectEventArgs> SetEffect;
        protected SoundEffectEventArgs args;

        public EnemyState(EnemyEntity entity, IEnemyState previousState)
        {
            this.entity = entity;
            this.previousState = previousState;
        }

        public void goombaToDead()
        {
            entity.Set(SpriteEnum.enemy | SpriteEnum.deadGoomba);
        }

        public void toShell()
        {
            entity.Set(SpriteEnum.enemy | SpriteEnum.shellKoopa);
        }

        public void toKoopa()
        {
            entity.Set(SpriteEnum.enemy | SpriteEnum.koopa);
        }

        public virtual void Enter(IEnemyState previousState) 
        {
            this.previousState = previousState;
        }
        public virtual void Exit() { }

        public virtual void Collision (ICollidable collidee, int direction) {
        }
        public virtual void Update(GameTime gameTime)
        {
        }
        protected virtual void onSetEffect(SoundEffectEventArgs e)
        {
            EventHandler<SoundEffectEventArgs> handler = SetEffect;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<PointEventArgs> IncScore;
        protected virtual void onIncScore(PointEventArgs e)
        {
            EventHandler<PointEventArgs> handler = IncScore;
            if (handler != null)
                handler(this, e);
        }
    }
}