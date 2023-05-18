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
using Sprint1.Physics;

namespace Sprint1.Entities
{
    public interface IEntity: ISprite, ICollidable
    {
        Game1 game { get; set; }
        ISprite sprite { get; set; }
        SpriteEnum spriteType {get; set;}
        Rigidbody rigidbody { get; set; }
        delegate Vector2 Transformation(Vector2 value);
        Transformation transformation { get; set; }
        void Set(SpriteEnum spriteType);
    }
}