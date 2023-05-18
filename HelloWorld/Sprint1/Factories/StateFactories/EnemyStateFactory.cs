using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.States.EnemyStates;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Sprites;

namespace Sprint1.Factories.StateFactories
{
    public class EnemyStateFactory
    {
        private EnemyEntity entity;
       

        public EnemyStateFactory(EnemyEntity entity)
        {
            this.entity = entity;
          
        }

        public IEnemyState Create(SpriteEnum spriteType, IEnemyState previousEnemyState)
        {
            IEnemyState enemyState = null;
            if ((SpriteEnum.enemy & spriteType) == SpriteEnum.enemy)
            {
                SpriteEnum enemy = (SpriteEnum.allEnemies & spriteType);
                if (enemy != SpriteEnum.enemy)
                {
                    switch (enemy)
                    {
                        case SpriteEnum.enemy | SpriteEnum.koopa:
                            enemyState = new EnemyKoopaState(entity, previousEnemyState);
                            break;
                        case SpriteEnum.enemy | SpriteEnum.goomba:
                            enemyState = new EnemyGoombaState(entity, previousEnemyState);
                            break;
                        case SpriteEnum.enemy | SpriteEnum.shellKoopa:
                            enemyState = new EnemyShellState(entity, previousEnemyState);
                            break;
                        case SpriteEnum.enemy | SpriteEnum.piranhaPlant:
                            enemyState = new EnemyPiranhaPlantState(entity, previousEnemyState);
                            break;
                    }
                }
            }
            return enemyState;
        }
    }
}