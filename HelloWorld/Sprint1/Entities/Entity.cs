using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.States.ActionStates;
using Sprint1.States.PowerStates;
using Sprint1.Sprites;
using Sprint1.Factories;
using Sprint1.Transformations;
using Sprint1.Collisions;
using Sprint1.Factories.SpriteFactories;
using System.Diagnostics;
using Sprint1.Physics;

namespace Sprint1.Entities
{
    public class Entity : IEntity
    {
        public Game1 game { get; set; }
        public ISprite sprite { get; set; }
        public SpriteEnum spriteType {get; set;}
        private IEntity.Transformation trans;
        private Rigidbody rb;
        protected Rectangle collider;
        protected Point colliderOffset;
        private readonly IEntity.Transformation nullTransformation = new NullTransformation().applyTransformation;
       
        public int RightEdge { get => sprite.RightEdge; }
        public Entity(Game1 game, SpriteEnum spriteType)
        { 
            this.game = game;
            this.colliderOffset = game.GetColliderOffset(spriteType);
            this.spriteType = spriteType;
            this.sprite = new NullSprite();
            this.transformation = (new NullTransformation()).applyTransformation;
        }
        public Entity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : this (game, spriteType)
        {
            this.Position = position;
            this.IsRight = isRight;
            this.color = color;
            this.layerDepth = layerDepth;
        }

        public Entity(Game1 game, ISprite sprite)
        { 
            this.game = game;
            //this.colliderOffset = game.GetColliderOffset(spriteType);
            this.sprite = sprite;
            this.transformation = (new NullTransformation()).applyTransformation;
        }
        public Entity(Game1 game)
        {
            this.game = game;
            this.sprite = new NullSprite();
            this.transformation = (new NullTransformation()).applyTransformation;
        }

        public virtual Vector2 Position
        {
            get { return sprite.Position; }
            set
            { 
                sprite.Position = value;
                collider.Location = colliderOffset + new Point((int)value.X, (int)value.Y);
            }
        }

        public bool IsVisible
        {
            get { return sprite.IsVisible; }
            set { sprite.IsVisible = value; }
        }

        public bool IsRight
        {
            get { return sprite.IsRight; }
            set { sprite.IsRight = value; }
        }

        public Color color
        {
            get { return sprite.color; }
            set { sprite.color = value; }
        }

        public float layerDepth
        {
            get { return sprite.layerDepth; }
            set { sprite.layerDepth = value; }
        }
        
        public IEntity.Transformation transformation 
        { 
            get 
            { 
                return trans;
            }
            set 
            {
                trans = value;
                /*
                if (value != null)
                {
                    if (Position == transformation(Position))
                    {
                        game.RemoveMoving(this);
                    }
                    else
                    {
                        game.AddMoving(this);
                    }
                }
                */
            }
        }
        
        public Rigidbody rigidbody
        {
            get { return rb; }
            set 
            {
                rb = value;
                if (value != null)
                {                    
                    if (rb.velocity == Vector2.Zero)
                    {
                        game.RemoveMoving(this);
                    }
                    else
                    {            
                        game.AddMoving(this);
                    }
                }

            }
        }

        public Rectangle Collider 
        {
            get
            {
                Rectangle sweepingBox = collider;
                Vector2 positionDelta;
                if (rigidbody != null)
                {
                    positionDelta = rigidbody.velocity;
                }
                else 
                { 
                    positionDelta = transformation(Position) - Position;
                }
                
                sweepingBox.Width += (int)Math.Abs(positionDelta.X);
                sweepingBox.Height += (int)Math.Abs(positionDelta.Y);
                
                if (positionDelta.X < 0)
                {
                    sweepingBox.X += (int)positionDelta.X;
                }
                
                if (positionDelta.Y < 0)
                {
                    sweepingBox.Y += (int)positionDelta.Y;
                }
                
                return sweepingBox;
                
                //return collider;
            }
            set
            {
                collider = value;
            }
        }

        public virtual void Set(SpriteEnum spriteType) 
        {
            this.spriteType = spriteType;
            this.sprite = game.CreateSprite(spriteType, Position, IsRight, color, layerDepth);
            this.colliderOffset = game.GetColliderOffset(spriteType);
            this.Collider = game.GetCollider(spriteType, Position);
        }

        public virtual void Update(GameTime gameTime)
        {
            //sprite.Position = transformation(Position);
            sprite.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);  
        }

        public virtual void DrawCollider(SpriteBatch spriteBatch)
        {
            Texture2D colliderRect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            colliderRect.SetData(new Color[] { Color.White });
            spriteBatch.Draw(colliderRect, Collider, Collider, Color.GreenYellow, 0, new Vector2(0,0), 0, layerDepth);
        }

        public virtual void OnCollisionEnter(ICollidable collision, int direction) { }

        /*
        public void FlipColliderHorizontally()
        {
            int originalOffsetX = colliderOffset.X;
            Rectangle originalCollider = collider;

            colliderOffset.X = this.sprite.RightEdge - (originalOffsetX + collider.Width);
            originalCollider.X = (int)Position.X + colliderOffset.X;
        }

        */
    }
}