using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint1.Entities;

namespace Sprint1.Transformations
{
    public class SpawnMove
    {
        private IEntity entity;
        private int Xoffset;
        private int Yoffset;
        private Vector2 anchor;
        private bool Xmove = true;
        private bool Ymove = true;

        public SpawnMove(IEntity entity, int Xoffset, int Yoffset, Vector2 anchor)
        {
            this.entity = entity;
            this.Xoffset = Xoffset;
            this.Yoffset = Yoffset;
            this.anchor = anchor;
        }

        public Vector2 applyTransformation(Vector2 position)
        {
            Vector2 newPosition = position;
            
            if (Xmove)
            {
                if (Xoffset > 0)
                {
                    newPosition.X++;
                    if (newPosition.X >=  anchor.X + Xoffset)
                    {
                        Xmove = false;
                    }
                }
                else
                {
                    newPosition.X--;
                    if (newPosition.X <= anchor.X + Xoffset)
                    {
                        Xmove = false;
                    }
                }
            }

            if (Ymove)
            {
                if (Yoffset > 0)
                {
                    newPosition.Y++;
                    if (newPosition.Y >=  anchor.Y + Yoffset)
                    {
                        Ymove = false;
                    }
                }
                else
                {
                    newPosition.Y--;
                    if (newPosition.Y <= anchor.Y + Yoffset)
                    {
                        Ymove = false;
                    }
                }
            }

            if (!Xmove && !Ymove)
            {
                entity.transformation = (new NullTransformation()).applyTransformation;
                entity.Collider = entity.game.GetCollider(entity.spriteType, entity.Position);
            }
            
            return newPosition;
        }
    }
}
