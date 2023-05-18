using Sprint1.States.ActionStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.Transformations;
using Microsoft.Xna.Framework;
using Sprint1.States.PowerStates;
using Sprint1.Sprites;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Collisions;
using System.Diagnostics;
using Sprint1.Trackers;
using Sprint1.Audio;

namespace Sprint1.States.BlockStates
{    
    public class BrickBlockState : BlockState
    {
        private float anchor;

        public BrickBlockState(BlockEntity entity, IBlockState previousState) : base(entity, previousState)
        {
            this.anchor = entity.Position.Y;
        }

        public override void Collision(ICollidable collidee, int direction)
        {
            if (direction == 2 && collidee is PlayerEntity)
            {
                if ((((PlayerEntity)collidee).spriteType & SpriteEnum.allPowers) == (SpriteEnum.player | SpriteEnum.small))
                {
                    base.args = new SoundEffectEventArgs { effect = "bump" };
                    onSetEffect(base.args);
                    //base.SetEffect -= entity.game.audioManager.PlaySoundEffect;
                    if (entity.Position.Y >= anchor)
                    {
                        entity.transformation = (new Bump(entity, 10, anchor)).applyTransformation;
                        base.args = new SoundEffectEventArgs { effect = "bump" };
                        onSetEffect(base.args);
                        //base.SetEffect -= entity.game.audioManager.PlaySoundEffect;
                    }
                    toBumped();
                }
                else
                {
                    base.args = new SoundEffectEventArgs { effect = "breakblock" };
                    onSetEffect(base.args);
                    base.SetEffect -= entity.game.audioManager.PlaySoundEffect;
                    Shatter();
                    toUsed();
                }
            }
        }

        private void Shatter()
        {
            int numParticles = 4;
            int particleWidth = 24;
            int particleHeight = 27;

            for(int i = 0; i < numParticles; i++)
            {
                bool isTop = i < 2;
                bool isLeft = (i % 2) == 0;
                entity.game.AddSprite(CreateParticle(isLeft, isTop, particleWidth, particleHeight));
            }
            
            entity.game.RemoveSprite(entity);
        }

        private IEntity CreateParticle(bool isLeft, bool isTop, int particleWidth, int particleHeight)
        {
            int offset = 5;
            //Adds particleWidth to the X position of left particles
            int x = (int)entity.Position.X + (isLeft ? 0 : particleWidth);
            //Adds particleHeight to the Y position of bottom particles
            int y = (int)entity.Position.Y + (isTop ? 0 : particleHeight);

            SpriteEnum type = SpriteEnum.particle | SpriteEnum.brick;
            IEntity particle = new Entity(entity.game, type, entity.Position, true, entity.color, entity.layerDepth);
            int xOffset = isLeft ? -offset : offset;
            int yOffset = isTop ? -offset : offset;
            particle.transformation = new ParticleMovement(entity, xOffset, yOffset, particle.Position.X).applyTransformation;
            particle.Set(type);
            particle.Collider = Rectangle.Empty;

            return particle;
        }
    }
}
