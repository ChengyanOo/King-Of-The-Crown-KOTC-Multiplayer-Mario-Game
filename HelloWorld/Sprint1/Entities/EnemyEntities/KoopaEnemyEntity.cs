using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.States.EnemyStates;
using Sprint1.Sprites;
using Sprint1.Transformations;
using Sprint1.Factories.StateFactories;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Collisions;
using Sprint1.States.BlockStates;
using Sprint1.States.ActionStates;
using Sprint1.States.PowerStates;
using Sprint1.Factories;
using Sprint1.Entities;

namespace Sprint1.Entities.EnemyEntities
{
    public class KoopaEnemyEntity : EnemyEntity
    {
        //private EnemyColliderFactory enemyColliderFactory;
        public KoopaEnemyEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {
        }

        public override void Set(SpriteEnum spriteType)
        {
            enemyState = enemyStateFactory.Create(spriteType, enemyState);
            setSprite(spriteType);
            setState(spriteType);
            //enemyColliderFactory.Create(this.spriteType, this.Position);
        }

        public override void Update(GameTime gameTime)
        {
            this.Position = transformation(this.Position);
            //enemyColliderFactory.Create(this.spriteType, this.Position);
            base.Update(gameTime);
        }

        public override void OnCollisionEnter(ICollidable collidee, int direction)
        {
            enemyState.Collision(collidee, direction);
        }


        private void setSprite(SpriteEnum spriteType)
        {
            if ((spriteType & SpriteEnum.enemy) == SpriteEnum.enemy)
            {
                SpriteEnum enemy = SpriteEnum.allEnemies | spriteType;
                if (enemy != SpriteEnum.enemy)
                {
                    base.Set(spriteType);
                }
            }
        }
        private void setState(SpriteEnum spriteType)
        {
            IEnemyState previousEnemyState = enemyState;

            IEnemyState newEnemyState = enemyStateFactory.Create(spriteType, enemyState);
            if (newEnemyState != null)
            {
                enemyState = newEnemyState;
            }

            if ((spriteType & SpriteEnum.enemy) == SpriteEnum.enemy)
            {
                if ((spriteType & SpriteEnum.allEnemies) != SpriteEnum.enemy)
                {
                    enemyState.Exit();
                    enemyState.Enter(previousEnemyState);
                }
            }
        }
    }

}
