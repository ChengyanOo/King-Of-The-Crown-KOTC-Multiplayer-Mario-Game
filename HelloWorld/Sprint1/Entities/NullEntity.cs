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


//using System.Numerics;

namespace Sprint1.Entities
{
    public class NullEntity : IEntity
    {

        // Game1 game { get; set; }
        // ISprite sprite { get; set; }
        // SpriteEnum spriteType { get; set; }
        public delegate Vector2 Transformation(Vector2 value);
        public IEntity.Transformation transformation { get; set; }
        //lol


        //bool IsMoving();
        public Game1 game { get; set; }
        public ISprite sprite { get; set; }
        public SpriteEnum spriteType { get; set; }
        private IEntity.Transformation trans;
        private Rectangle collider;
        private readonly IEntity.Transformation nullTransformation = new NullTransformation().applyTransformation;
        public Vector2 Position { get; set; }
        public bool IsVisible { get; set; }
        public bool IsRight { get; set; }
        public Color color { get; set; }
        public float layerDepth { get; set; }
        public Rigidbody rigidbody { get; set; }

        public Rectangle Collider { get; set; }

        public int RightEdge { get => 0; }
        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch) { }

        public NullEntity()
        {
            //Content.RootDirectory = "Content";
            game = new Game1();
            sprite = new NullSprite();
            spriteType = new SpriteEnum();

            /*
            Position = new Vector2(0, 0);
            IsVisible = false;
            IsRight = true;
            backgroundColor = Color.White;
            layerDepth = 0;
            */
        }
        public void Set(SpriteEnum spriteType)
        {

        }

        public void OnCollisionEnter(ICollidable collision, int direction) { }
        public void DrawCollider(SpriteBatch spriteBatch) { }




    }
}
