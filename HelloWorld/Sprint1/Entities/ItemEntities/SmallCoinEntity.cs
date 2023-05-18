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
using Sprint1.Factories;
using Sprint1.Transformations;
using System.Diagnostics;
using static Sprint1.States.PowerStates.PowerState;
using Sprint1.Trackers;
using Sprint1.Audio;

namespace Sprint1.Entities.ItemEntities
{
    public class SmallCoinEntity : ItemEntity
    {
        float hideTime = 1f; //every  5s.
        float currentTime = 0f;

        public event EventHandler<PointEventArgs> IncScore;
        public event EventHandler<CoinEventArgs> SetCoin;
        CoinEventArgs CoinArgs;
        public event EventHandler<SoundEffectEventArgs> SetEffect;
        SoundEffectEventArgs SoundEffectArgs;


        public SmallCoinEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {
            Set(spriteType);

            this.IncScore += game.pointTracker.IncScore;
            this.SetCoin += game.coinTracker.AteCoin;
            this.SetEffect += game.audioManager.PlaySoundEffect;
        }

        public SmallCoinEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {
          
            Set(spriteType);

            this.IncScore += game.pointTracker.IncScore;
            this.SetCoin += game.coinTracker.AteCoin;

            PointEventArgs args = new PointEventArgs { PointValue = 100 };
            onIncScore(args);

            CoinArgs = new CoinEventArgs { CoinValue = 1 };
            onSetCoin(CoinArgs);
            this.SetCoin -= game.coinTracker.AteCoin;

            this.SetEffect += game.audioManager.PlaySoundEffect;

            SoundEffectArgs = new SoundEffectEventArgs { effect = "coin" };
            onSetEffect(SoundEffectArgs);
        }

        public override void Set(SpriteEnum spriteType)
        {

            setSprite(spriteType);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
          
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentTime >= hideTime)
            {
                currentTime = 0;
                game.RemoveSprite(this);

            }
            

        }

        public override void OnCollisionEnter(ICollidable collidee, int direction)
        {
        }

        private void setSprite(SpriteEnum spriteType)
        {
            if ((spriteType & SpriteEnum.item) == SpriteEnum.item)
            {
                SpriteEnum itemMushroom = SpriteEnum.allItems | spriteType;
                if (itemMushroom != SpriteEnum.item)
                {
                    base.Set(spriteType);
                }
            }
        }


        protected virtual void onIncScore(PointEventArgs e)
        {
            EventHandler<PointEventArgs> handler = IncScore;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void onSetCoin(CoinEventArgs e)
        {
            EventHandler<CoinEventArgs> handler = SetCoin;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void onSetEffect(SoundEffectEventArgs e)
        {
            EventHandler<SoundEffectEventArgs> handler = SetEffect;
            if (handler != null)
                handler(this, e);
        }
    }
}

