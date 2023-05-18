using Microsoft.Xna.Framework;
using Sprint1.Factories;
using Sprint1.Factories.SpriteFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Entities.ItemEntities.Fireball
{
    public class FireballPool
    {
        private Game1 game;
        private List<FireballEntity> fireballs = new List<FireballEntity>();
        public int fireballCount = 0;
        private SpriteEnum spriteType;
        
        public FireballPool(Game1 game, int maxFireballs, SpriteEnum spriteType, float layerDepth)
        {
            for (int i = 0; i < maxFireballs; i++)
            {
                 fireballs.Add((FireballEntity)game.CreateEntity(spriteType, new Vector2(0,0), true, Color.White, layerDepth));
            }
            this.game = game;
            this.spriteType = spriteType;
        }

        public FireballEntity GetFireball()
        {
            if (fireballCount < fireballs.Count)
            {
                fireballCount++;
                return fireballs[fireballCount - 1];
            }
            else
            {
                /**
                FireballEntity fireball = (FireballEntity)game.CreateEntity(spriteType, new Vector2(0, 0), true, Color.White);
                fireballs.Add(fireball);
                fireballCount++;
                return fireball;
                **/
                return null;
            }
        }

        public void ReturnFireball(FireballEntity fireball)
        {
            game.RemoveSprite(fireball);
            fireballCount--;
            if (fireballCount < 0)
            {
                fireballCount = 0;
            }
            fireballs[fireballCount] = fireball;
        }
    }
}
