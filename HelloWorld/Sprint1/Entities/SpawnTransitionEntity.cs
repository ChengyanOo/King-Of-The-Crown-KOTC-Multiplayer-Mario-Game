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
    public class SpawnTransitionEntity : Entity
    {
        private IEntity spawnerEntity;
        private float speed = 2;

        private SpawnTransitionEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {
            base.Set(spriteType);
        }

        private SpawnTransitionEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth) : base(game, spriteType, position, isRight, color, layerDepth)
        {
            base.Set(spriteType);
        }

        public SpawnTransitionEntity(Game1 game, SpriteEnum spriteType, IEntity entity, int direction) : this(game, spriteType)
        {
            this.spawnerEntity = entity;
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1);
            Enter(direction);
            //entity.Collider = Rectangle.Empty;
        }

        public SpawnTransitionEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth, IEntity entity, int direction) : this(game, spriteType, position, isRight, color, layerDepth)
        {
            this.spawnerEntity = entity;
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1);
            Enter(direction);
            //entity.Collider = Rectangle.Empty;
        }

        public override void Update(GameTime gameTime)
        {
            this.rigidbody.Update(gameTime);
            this.rigidbody.CheckMoving(this);
            base.Update(gameTime);

            if (checkSpawnEnd())
            {
                Exit();
            }
        }

        public void Enter(int direction)
        {
            switch (direction)
            {
                case 0: //top
                    this.rigidbody.velocity = new Vector2(0, -speed);
                    break;
                case 1: //right
                    this.rigidbody.velocity = new Vector2(speed, 0);
                    break;
                case 2: //bot
                    this.rigidbody.velocity = new Vector2(0, speed);
                    break;
                case 3: //left
                    this.rigidbody.velocity = new Vector2(-speed, 0);
                    break;
            }
        }

        public bool checkSpawnEnd()
        {
            Rectangle spawnerCollider = spawnerEntity.game.GetCollider(spawnerEntity.spriteType, spawnerEntity.Position);
            Rectangle spawneeCollider = this.game.GetCollider(this.spriteType, this.Position);
            return !spawnerCollider.Intersects(spawneeCollider);
        }

        public void Exit()
        {
            this.game.RemoveSprite(this);
            IEntity spawnedEntity = (IEntity)this.game.CreateEntity(this.spriteType, this.Position, this.IsRight, this.color, this.layerDepth);
            this.game.AddSprite(spawnedEntity);
        }
    }
}