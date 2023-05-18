using Microsoft.Xna.Framework;
using Sprint1.Collisions;
using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Physics
{
    public class Rigidbody
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;
        public float mass;
        public float drag;
        public float gravity;
        public bool isGrounded;
        private Game1 game;
        public Vector2 nextPosition;
        private bool falling = true;
        public event EventHandler<EventArgs> Moving;

        public Rigidbody(Game1 game, Vector2 position, Vector2 velocity, Vector2 acceleration, float mass, float drag, float gravity, bool isGrounded)
        {
            this.game = game;
            this.position = position;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.mass = mass;
            this.drag = drag;
            this.gravity = gravity;
            this.isGrounded = isGrounded;
        }

        public Rigidbody(Game1 game, Vector2 position, Vector2 velocity, float mass)
        {
            this.game = game;
            this.position = position;
            this.velocity = velocity;
            this.mass = mass;
            this.drag = 0.1f;
            this.gravity = 1;
            this.isGrounded = false;
            this.nextPosition = position;
        }

        public Rigidbody(Game1 game, Vector2 position, Vector2 velocity, float mass, float gravity)
        {
            this.game = game;
            this.position = position;
            this.velocity = velocity;
            this.mass = mass;
            this.drag = 0.1f;
            this.gravity = gravity;
            this.isGrounded = false;
            this.nextPosition = position;
            falling = (gravity != 0);
        }


        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
           
            if (falling && !isGrounded)
            {
                if (velocity.Y <= 10)
                {
                    velocity.Y += gravity;
                }
                
                /*
                if (velocity.Y <= 0)
                {
                    velocity.Y += 1;
                }
                */
            }    
            velocity.Y += acceleration.Y * deltaTime;            
            //velocity.Y = 1;
            //velocity.Y += acceleration.Y * deltaTime;
            velocity.X += acceleration.X * deltaTime;
            //position += velocity * deltaTime;
            //nextPosition = position + velocity * deltaTime;
            //isGrounded = false;
        }
        
        public void CheckMoving(Entity entity)
        {
            if(entity.rigidbody != null)
            {
                if (entity.rigidbody.velocity == Vector2.Zero)
                {
                    game.RemoveMoving(entity);
                }
                else
                {
                    game.AddMoving(entity);
                    onMoving();
                }
            }
        }
        
        //I know this is awful but its a temporary solution until we can properly check the collsion below mario.
        public void CheckGround(IEntity entity, List<ICollidable> possibleCollisions)
        {
            if (isGrounded)
            {
                int collisionCount = 0;
                foreach (ICollidable collision in possibleCollisions)
                {
                    if (collision is IEntity && !((((IEntity)collision).spriteType & SpriteEnum.allBlocks) == (SpriteEnum.block | SpriteEnum.hidden)))
                    {
                        if (entity.rigidbody != null && entity != collision)
                        {
                            Rectangle offset = entity.game.GetCollider(entity.spriteType, entity.Position);
                            Rectangle feetCollider = new Rectangle(offset.X+5, offset.Bottom, offset.Width-10, 1);
                            
                            if (feetCollider.Intersects(collision.Collider))
                            {
                                collisionCount++;
                            }
                        }
                    }
                }
                if (collisionCount == 0)
                {
                    //Debug.WriteLine("Not Grounded");
                    entity.rigidbody.isGrounded = false;
                }
            }
        }

        public void AddForce(float force, float angle)
        {
            Vector2 forceVector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            forceVector *= force;
            acceleration += forceVector / mass;          
        }

        protected virtual void onMoving()
        {
            EventHandler<EventArgs> handler = Moving;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /**
        public void AddForce(float force, float angle)
        {
            x = position.X + ((float)MathF.Cos((float)((Math.PI / 180) * angle)) * force) * timeSincePeak;
            newPosition.X = x * deltaTime;

            y = position.Y + ((float)MathF.Sin((float)((Math.PI / 180) * angle)) * force) * timeSincePeak - (gravity * (timeSincePeak * timeSincePeak / 2));
            newPosition.Y = y * deltaTime;
        }**/


    }


}
