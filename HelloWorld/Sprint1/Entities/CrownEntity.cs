using Sprint1.Sprites;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Factories.StateFactories;
using Microsoft.Xna.Framework;
using Sprint1.Collisions;
using Sprint1.States.EnemyStates;
using Sprint1.Factories;
using Sprint1.Transformations;
using Sprint1.Physics;
using Sprint1.States.CrownStates;
using Sprint1.Trackers;
using Sprint1.Audio;

namespace Sprint1.Entities
{
    public class CrownEntity : Entity
    {
        private CrownStateFactory crownStateFactory;
        private ICrownState crownState;
        public PlayerEntity playerEntity { get; set; }
        
        public CrownEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        { 
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1, 0);
            this.crownStateFactory = new CrownStateFactory(this);
            Set(spriteType);
        }

        public CrownEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1, 0);
            this.crownStateFactory = new CrownStateFactory(this);
            Set(spriteType);
        }

        public CrownEntity(Game1 game, PlayerEntity playerEntity, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1, 0);
            this.crownStateFactory = new CrownStateFactory(this);
            this.playerEntity = playerEntity;
            Set(spriteType);
        }

        public override Vector2 Position
        {
            get { return sprite.Position; }
            set
            { 
                sprite.Position = value;
                sprite.Position = Vector2.Clamp(sprite.Position, new Vector2(48, 38), new Vector2(1148, 581));
                if ((sprite.Position.X <= 48 || sprite.Position.X >= 1148) && this.rigidbody != null)
                {
                    this.rigidbody.velocity = new Vector2(-this.rigidbody.velocity.X, this.rigidbody.velocity.Y);
                }
                collider.Location = colliderOffset + new Point((int)sprite.Position.X, (int)sprite.Position.Y);
            }
        }

        public void turnFloating()
        {
            crownState.toFloating();
        }

        public void turnThrown()
        {
            crownState.toThrown();
        }

        public void turnAttached(PlayerEntity player)
        {
            if (crownState is AttachedCrownState)
            {
                crownState.Exit();
            }

            crownState.toAttached(player);
        }

        public override void OnCollisionEnter(ICollidable collidee, int direction)
        {
            crownState.collision(collidee, direction);
        }

        public override void Set(SpriteEnum spriteType)
        {
            setSprite(spriteType);
            setState(spriteType);
        }

        public override void Update(GameTime gameTime)
        {
            if (rigidbody != null)
            {
                this.rigidbody.Update(gameTime);
                this.rigidbody.CheckMoving(this);
            }
            
            base.Update(gameTime);
        }

        private void setSprite(SpriteEnum spriteType)
        {
            if ((spriteType & SpriteEnum.crown) == SpriteEnum.crown)
            {
                SpriteEnum state = SpriteEnum.allCrowns | spriteType;
                if (state != SpriteEnum.crown)
                {
                    base.Set(spriteType);
                }
            }
        }

        public void Exit()
        {
            crownState.Exit();
        }

        private void setState(SpriteEnum spriteType)
        {
            ICrownState previousState = crownState;

            ICrownState newState = crownStateFactory.Create(spriteType, crownState);
            if (newState != null)
            {
                crownState = newState;

                if (previousState != null)
                {
                    previousState.Exit();
                }
                crownState.Enter(previousState);
            }
        }       
    }
}

