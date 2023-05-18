using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities.ItemEntities.Fireball;
using Sprint1.Entities.ItemEntities;
using System.Diagnostics;

namespace Sprint1.Factories
{
    public class EntityFactory
    {
        private Game1 game;

        public EntityFactory(Game1 game, Texture2D SpriteSheet)
        { 
            this.game = game;
        }

        public IEntity Create(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth)
        {
            IEntity entity = null;

            if (SpriteTypeHasComponent(spriteType, SpriteEnum.player))
            {
                entity = new PlayerEntity(game, spriteType, position, isRight, color, layerDepth);
            }
            else if (SpriteTypeHasComponent(spriteType, SpriteEnum.crown))
            {
                entity = new Entity(game, spriteType, position, isRight, color, layerDepth);
            }
            else if (SpriteTypeHasComponent(spriteType, SpriteEnum.block))
            {
                entity = new BlockEntity(game, spriteType, position, isRight, color, layerDepth);
            } else if(SpriteTypeHasComponent(spriteType, SpriteEnum.hazard))
            {
                entity = new Entity(game, spriteType, position, isRight, color, layerDepth);
            } else if(SpriteTypeHasComponent(spriteType, SpriteEnum.particle))
            {
                entity = new FireballEntity(game, spriteType);
            }

            //If entity is null, make a basic entity
            entity ??= new Entity(game);

            entity.Collider = GetCollider(spriteType, position);
            return entity;
        }

        public Rectangle GetCollider(SpriteEnum spriteType, Vector2 position)
        {
            Rectangle collider = GetColliderFromDictionary(spriteType);
            collider.Offset(position.X, position.Y);
            return collider;
        }

        public Point GetColliderOffset(SpriteEnum spriteType)
        {
            return GetColliderOffsetFromDictionary(spriteType);
        }

        private static bool SpriteTypeHasComponent(SpriteEnum spriteType, SpriteEnum component)
        {
            return ((spriteType & component) == component);
        }

        private Rectangle GetColliderFromDictionary(SpriteEnum spriteType)
        {
            string s = dictionary1.GetValueOrDefault(spriteType);
            if(SpriteTypeHasComponent(spriteType, SpriteEnum.player | SpriteEnum.dead))
            {
                s = "dead player";
            }
            return colliderDict.GetValueOrDefault(s);
        }

        private Point GetColliderOffsetFromDictionary(SpriteEnum spriteType)
        {
            return GetColliderFromDictionary(spriteType).Location;
        }

        private Dictionary<SpriteEnum, string> dictionary1 = new Dictionary<SpriteEnum, string> 
        {
            {SpriteEnum.dead, "dead player" },
            //small player
            {SpriteEnum.player1, "small idle player" },
            {SpriteEnum.player2, "small idle player" },
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.small | SpriteEnum.idle, 
                "small idle player" },
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.small | SpriteEnum.running,
                "small running player" },
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.small | SpriteEnum.jumping,
                "small jumping player" },
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.small | SpriteEnum.falling,
                "small jumping player" },
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.small | SpriteEnum.dead,
                "dead player"},
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.small | SpriteEnum.idle,
                "small idle player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.small | SpriteEnum.running,
                "small running player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.small | SpriteEnum.jumping,
                "small jumping player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.small | SpriteEnum.falling,
                "small jumping player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.small | SpriteEnum.dead,
                "dead player"},
            //big player
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.super | SpriteEnum.idle,
                "big idle player" },
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.super | SpriteEnum.running,
                "big running player" },
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.super | SpriteEnum.jumping,
                "big jumping player" },
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.super | SpriteEnum.falling,
                "big jumping player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.super | SpriteEnum.idle,
                "big idle player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.super | SpriteEnum.running,
                "big running player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.super | SpriteEnum.jumping,
                "big running player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.super | SpriteEnum.falling,
                "big running player" },
            //fire player
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.fire | SpriteEnum.idle,
                "big idle player" },
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.fire | SpriteEnum.running,
                "big running player" },
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.fire | SpriteEnum.jumping,
                "big jumping player" },
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.fire | SpriteEnum.falling,
                "big jumping player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.fire | SpriteEnum.idle,
                "big idle player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.fire | SpriteEnum.running,
                "big running player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.fire | SpriteEnum.jumping,
                "big running player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.fire | SpriteEnum.falling,
                "big running player" },
            //crouching player
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.super | SpriteEnum.crouching,
                "crouching player" },
            {SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.fire | SpriteEnum.crouching,
                "crouching player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.super | SpriteEnum.crouching,
                "crouching player" },
            {SpriteEnum.player | SpriteEnum.player2 | SpriteEnum.fire | SpriteEnum.crouching,
                "crouching player" },
            //Crown
            {SpriteEnum.crown | SpriteEnum.floating,
                "big crown"},
            {SpriteEnum.crown | SpriteEnum.thrown,
                "big crown"},
            {SpriteEnum.crown | SpriteEnum.attached,
                "attached crown"},
            {SpriteEnum.crown | SpriteEnum.small,
                "small crown"},
            //Walls
            {SpriteEnum.block | SpriteEnum.wall | SpriteEnum.top | SpriteEnum.left,
                "block"},
            {SpriteEnum.block | SpriteEnum.wall | SpriteEnum.top | SpriteEnum.mid,
                "block"},
            {SpriteEnum.block | SpriteEnum.wall | SpriteEnum.top | SpriteEnum.right,
                "block"},
            {SpriteEnum.block | SpriteEnum.wall | SpriteEnum.mid | SpriteEnum.left,
                "block"},
            {SpriteEnum.block | SpriteEnum.wall | SpriteEnum.mid | SpriteEnum.mid,
                "block"},
            {SpriteEnum.block | SpriteEnum.wall | SpriteEnum.mid | SpriteEnum.right,
                "block"},
            {SpriteEnum.block | SpriteEnum.wall | SpriteEnum.bot | SpriteEnum.left,
                "block"},
            {SpriteEnum.block | SpriteEnum.wall | SpriteEnum.bot | SpriteEnum.mid,
                "block"},
            {SpriteEnum.block | SpriteEnum.wall | SpriteEnum.bot | SpriteEnum.right,
                "block"},
            //Horizontal Platforms
            {SpriteEnum.block | SpriteEnum.platform | SpriteEnum.left,
                "block"},
            {SpriteEnum.block | SpriteEnum.platform | SpriteEnum.left | SpriteEnum.tapered,
                "block"},
            {SpriteEnum.block | SpriteEnum.platform | SpriteEnum.mid,
                "block"},
            {SpriteEnum.block | SpriteEnum.platform | SpriteEnum.right,
                "block"},
            {SpriteEnum.block | SpriteEnum.platform | SpriteEnum.right | SpriteEnum.tapered,
                "block"},
            {SpriteEnum.block | SpriteEnum.platform | SpriteEnum.moving,
                "moving platform"},
            //Vertical platforms
            {SpriteEnum.block | SpriteEnum.platform | SpriteEnum.left | SpriteEnum.top,
                "block"},
            {SpriteEnum.block | SpriteEnum.platform | SpriteEnum.left | SpriteEnum.mid,
                "block"},
            {SpriteEnum.block | SpriteEnum.platform | SpriteEnum.left | SpriteEnum.bot,
                "block"},
            {SpriteEnum.block | SpriteEnum.platform | SpriteEnum.right | SpriteEnum.top,
                "block"},
            {SpriteEnum.block | SpriteEnum.platform | SpriteEnum.right | SpriteEnum.mid,
                "block"},
            {SpriteEnum.block | SpriteEnum.platform | SpriteEnum.right | SpriteEnum.bot,
                "block"},
            //Jump pad
            {SpriteEnum.block | SpriteEnum.jumpPad,
                "jump pad"},
            //Hazards
            {SpriteEnum.block | SpriteEnum.hazard | SpriteEnum.spike,
                "spike"},
            {SpriteEnum.block | SpriteEnum.hazard | SpriteEnum.muncher,
                "muncher"},
            {SpriteEnum.block | SpriteEnum.hazard | SpriteEnum.hothead,
                "hothead"},
        };

        // Test
        private Dictionary<string, Rectangle> colliderDict = new Dictionary<string, Rectangle> 
        {
            //Small Player
            { "small idle player", new Rectangle(6, 51, 36, 45) },
            { "small running player", new Rectangle(0, 48, 45, 48) },
            { "small jumping player", new Rectangle(0, 48, 48, 48) },
            { "dead player", new Rectangle(0, 48, 48, 48) },
            //Big player
            { "big idle player", new Rectangle(0, 15, 43, 81) },
            { "big running player", new Rectangle(0, 18, 45, 78) },
            { "big jumping player", new Rectangle(0, 18, 48, 78) },
            { "crouching player", new Rectangle(0, 45, 42, 51) },
            //Crown
            { "big crown", new Rectangle(-6, -6, 60, 60) },
            { "attached crown", new Rectangle(0, 0, 48, 48) },
            { "small crown", new Rectangle(0, 0, 48, 48) },
            //Hazards
            { "spike", new Rectangle(6, 6, 36, 42) },
            { "muncher", new Rectangle(9, 3, 27, 45) },
            { "hothead", new Rectangle(6, 9, 36, 39) },
            //Blocks
            { "block", new Rectangle(0, 0, 48, 48) },
            { "jump pad", new Rectangle(0, 12, 48, 36) },
            { "moving platform", new Rectangle(0, 12, 48, 24) }
        };
    }
    
}
