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
using Sprint1.Physics;


namespace Sprint1.Entities
{
    public class EnemyEntity : Entity
    {
        protected EnemyStateFactory enemyStateFactory;
        protected IEnemyState enemyState;
        private EnemyState eState;


        public EnemyEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {
            this.enemyStateFactory = new EnemyStateFactory(this);
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1);
            Set(spriteType);
        }

        public EnemyEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {
            this.enemyStateFactory = new EnemyStateFactory(this);
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1);
            Set(spriteType);
        }

        public override void Set(SpriteEnum spriteType)
        {
            //enemyState = enemyStateFactory.Create(spriteType, enemyState);
            setSprite(spriteType);
            setState(spriteType);
        }

        public override void Update(GameTime gameTime)
        {
            enemyState.Update(gameTime);
            this.rigidbody.Update(gameTime);
            this.rigidbody.CheckMoving(this);
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
                eState = (EnemyState)enemyState;
                eState.SetEffect += game.audioManager.PlaySoundEffect;
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

        public void AddPlayer(PlayerEntity playerMario)
        {
            if (enemyState is EnemyPiranhaPlantState)
            {
                ((EnemyPiranhaPlantState)enemyState).AddPlayer(playerMario);
            }
            
        }
    }
}