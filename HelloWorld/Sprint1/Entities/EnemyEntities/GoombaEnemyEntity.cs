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


namespace Sprint1.Entities
{
    public class GoombaEnemyEntity : EnemyEntity
    {
        public GoombaEnemyEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {}
        public GoombaEnemyEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {}

        public override void Set(SpriteEnum spriteType)
        {
            enemyState = enemyStateFactory.Create(spriteType, enemyState);
            setSprite(spriteType);
            setState(spriteType);
        }

        public override void Update(GameTime gameTime)
        {
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

        public void addPlayer(PlayerEntity playerEntity)
        {
            if (enemyState is EnemyGoombaState)
            {
                ((EnemyGoombaState)enemyState).AddPlayer(playerEntity);
                ((EnemyGoombaState)enemyState).moveGoomba();
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