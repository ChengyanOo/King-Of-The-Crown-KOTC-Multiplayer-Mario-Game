
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Sprites;
using Sprint1.States.BlockStates;
using System.Diagnostics;
using System.ComponentModel;

namespace Sprint1.Factories.SpriteFactories
{
    public class SpriteSheetFactory
    {
        private readonly Texture2D spriteSheet;
        private const int FPS = 8;
        private const int msPerFrame = 1000 / FPS;

        private const int tileSize = 48;
        public SpriteSheetFactory(Texture2D spriteSheet)
        {
            this.spriteSheet = spriteSheet;
        }

        public ISprite Create(SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0)
        {
            if ((spriteType & (SpriteEnum.player | SpriteEnum.dead)) == (SpriteEnum.player | SpriteEnum.dead))
            {
                spriteInfoDictionary.TryGetValue(spriteType & SpriteEnum.allPowers, out spriteInfo info);
                return new Sprite(spriteSheet, info.origin, info.size, position, info.visibility, isRight, color, layerDepth);
            }
            else if (spriteInfoDictionary.TryGetValue(spriteType, out spriteInfo info))
            {
                return new Sprite(spriteSheet, info.origin, info.size, position, info.visibility, isRight, color, layerDepth);
            }
            else if (animatedSpriteInfoDictionary.TryGetValue(spriteType, out animatedSpriteInfo animatedInfo))
            {
                return new Sprite(spriteSheet, animatedInfo.spriteInfo.origin, animatedInfo.spriteInfo.size, animatedInfo.rows, animatedInfo.columns, animatedInfo.millisecondsPerFrame, position, animatedInfo.spriteInfo.visibility, isRight, color, layerDepth);
            } 
            else
            {
                return new NullSprite();
            }
        }

        private struct spriteInfo
        {
            public Point origin;
            public Point size;
            public bool visibility;

            public spriteInfo(Point origin, Point size, bool visibility)
            {
                this.origin = origin;
                this.size = size;
                this.visibility = visibility;
            }
        }

        private struct animatedSpriteInfo
        {
            public spriteInfo spriteInfo;
            public int rows;
            public int columns;
            public int millisecondsPerFrame;
            public animatedSpriteInfo(Point origin, Point size, bool visibility, int rows, int columns, int ms)
            {
                spriteInfo = new spriteInfo(origin, size, visibility);
                this.rows = rows;
                this.columns = columns;
                millisecondsPerFrame = ms;
            }
        }

        private Dictionary<SpriteEnum, spriteInfo> spriteInfoDictionary = new Dictionary<SpriteEnum, spriteInfo>()
        {
            //Player 1 small
            {
                SpriteEnum.player | SpriteEnum.small | SpriteEnum.idle | SpriteEnum.player1,
                new spriteInfo(new Point(0, 0), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.small | SpriteEnum.jumping | SpriteEnum.player1,
                new spriteInfo(new Point(tileSize * 4, 0), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.small | SpriteEnum.falling | SpriteEnum.player1,
                new spriteInfo(new Point(tileSize * 4, 0), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.dead | SpriteEnum.player1,
                new spriteInfo(new Point(tileSize * 5, 0), new Point(tileSize, tileSize * 2), true)
            },
            //Player 1 super
            {
                SpriteEnum.player | SpriteEnum.super | SpriteEnum.idle | SpriteEnum.player1,
                new spriteInfo(new Point(0, tileSize * 2), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.super | SpriteEnum.jumping | SpriteEnum.player1,
                new spriteInfo(new Point(tileSize * 4, tileSize * 2), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.super | SpriteEnum.falling | SpriteEnum.player1,
                new spriteInfo(new Point(tileSize * 4, tileSize * 2), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.super | SpriteEnum.crouching | SpriteEnum.player1,
                new spriteInfo(new Point(tileSize * 5, tileSize * 2), new Point(tileSize, tileSize * 2), true)
            },
            //Player 1 fire
            {
                SpriteEnum.player | SpriteEnum.fire | SpriteEnum.idle | SpriteEnum.player1,
                new spriteInfo(new Point(0, tileSize * 4), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.fire | SpriteEnum.jumping | SpriteEnum.player1,
                new spriteInfo(new Point(tileSize * 4, tileSize * 4), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.fire | SpriteEnum.falling | SpriteEnum.player1,
                new spriteInfo(new Point(tileSize * 4, tileSize * 4), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.fire | SpriteEnum.crouching | SpriteEnum.player1,
                new spriteInfo(new Point(tileSize * 5, tileSize * 2), new Point(tileSize, tileSize * 2), true)
            },
            //Player 2 small
            {
                SpriteEnum.player | SpriteEnum.small | SpriteEnum.idle | SpriteEnum.player2,
                new spriteInfo(new Point(tileSize * 7, 0), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.small | SpriteEnum.jumping | SpriteEnum.player2,
                new spriteInfo(new Point(tileSize * 11, 0), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.small | SpriteEnum.falling | SpriteEnum.player2,
                new spriteInfo(new Point(tileSize * 11, 0), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.dead | SpriteEnum.player2,
                new spriteInfo(new Point(tileSize * 12, 0), new Point(tileSize, tileSize * 2), true)
            },
            //Player 2 super
            {
                SpriteEnum.player | SpriteEnum.super | SpriteEnum.idle | SpriteEnum.player2,
                new spriteInfo(new Point(tileSize * 7, tileSize * 2), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.super | SpriteEnum.jumping | SpriteEnum.player2,
                new spriteInfo(new Point(tileSize * 11, tileSize * 2), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.super | SpriteEnum.falling | SpriteEnum.player2,
                new spriteInfo(new Point(tileSize * 11, tileSize * 2), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.super | SpriteEnum.crouching | SpriteEnum.player2,
                new spriteInfo(new Point(tileSize * 12, tileSize * 2), new Point(tileSize, tileSize * 2), true)
            },
            //Player 2 fire
            {
                SpriteEnum.player | SpriteEnum.fire | SpriteEnum.idle | SpriteEnum.player2,
                new spriteInfo(new Point(tileSize * 7, tileSize * 4), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.fire | SpriteEnum.jumping | SpriteEnum.player2,
                new spriteInfo(new Point(tileSize * 11, tileSize * 4), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.fire | SpriteEnum.falling | SpriteEnum.player2,
                new spriteInfo(new Point(tileSize * 11, tileSize * 4), new Point(tileSize, tileSize * 2), true)
            },
            {
                SpriteEnum.player | SpriteEnum.fire | SpriteEnum.crouching | SpriteEnum.player2,
                new spriteInfo(new Point(tileSize * 12, tileSize * 4), new Point(tileSize, tileSize * 2), true)
            },
            //Crown
            {
                SpriteEnum.crown | SpriteEnum.floating,
                new spriteInfo(new Point(0, tileSize * 6), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.crown | SpriteEnum.thrown,
                new spriteInfo(new Point(0, tileSize * 6), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.crown | SpriteEnum.attached,
                new spriteInfo(new Point(tileSize, tileSize * 6), new Point((tileSize * 3) / 4, (tileSize * 11) / 16), true)
            },
            {
                SpriteEnum.crown | SpriteEnum.small,
                new spriteInfo(new Point(tileSize, tileSize * 6), new Point(tileSize / 2, (tileSize * 5) / 16), true)
            },
            //Walls
            {
                SpriteEnum.block | SpriteEnum.wall | SpriteEnum.top | SpriteEnum.left,
                new spriteInfo(new Point(0, tileSize * 7), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.wall | SpriteEnum.top | SpriteEnum.mid,
                new spriteInfo(new Point(tileSize, tileSize * 7), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.wall | SpriteEnum.top | SpriteEnum.right,
                new spriteInfo(new Point(tileSize * 2, tileSize * 7), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.wall | SpriteEnum.mid | SpriteEnum.left,
                new spriteInfo(new Point(0, tileSize * 8), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.wall | SpriteEnum.mid | SpriteEnum.right,
                new spriteInfo(new Point(tileSize * 2, tileSize * 8), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.wall | SpriteEnum.bot | SpriteEnum.left,
                new spriteInfo(new Point(0, tileSize * 9), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.wall | SpriteEnum.bot | SpriteEnum.mid,
                new spriteInfo(new Point(tileSize, tileSize * 9), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.wall | SpriteEnum.bot | SpriteEnum.right,
                new spriteInfo(new Point(tileSize * 2, tileSize * 9), new Point(tileSize, tileSize), true)
            },
            //Horizontal Platforms
            {
                SpriteEnum.block | SpriteEnum.platform | SpriteEnum.left,
                new spriteInfo(new Point(tileSize * 3, tileSize * 8), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.platform| SpriteEnum.left | SpriteEnum.tapered ,
                new spriteInfo(new Point(tileSize * 3, tileSize * 7), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.platform | SpriteEnum.mid,
                new spriteInfo(new Point(tileSize * 4, tileSize * 7), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.platform | SpriteEnum.right,
                new spriteInfo(new Point(tileSize * 5, tileSize * 7), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.platform | SpriteEnum.right | SpriteEnum.tapered,
                new spriteInfo(new Point(tileSize * 4, tileSize * 8), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.platform | SpriteEnum.moving,
                new spriteInfo(new Point(tileSize * 3, tileSize * 9), new Point(tileSize, tileSize), true)
            },
            //Vertical Platforms
            {
                SpriteEnum.block | SpriteEnum.platform | SpriteEnum.left | SpriteEnum.top,
                new spriteInfo(new Point(tileSize * 6, tileSize * 7), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.platform | SpriteEnum.left | SpriteEnum.mid,
                new spriteInfo(new Point(tileSize * 6, tileSize * 8), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.platform | SpriteEnum.left | SpriteEnum.bot,
                new spriteInfo(new Point(tileSize * 6, tileSize * 9), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.platform | SpriteEnum.right | SpriteEnum.top,
                new spriteInfo(new Point(tileSize * 7, tileSize * 7), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.platform | SpriteEnum.right | SpriteEnum.mid,
                new spriteInfo(new Point(tileSize * 7, tileSize * 8), new Point(tileSize, tileSize), true)
            },
            {
                SpriteEnum.block | SpriteEnum.platform | SpriteEnum.right | SpriteEnum.bot,
                new spriteInfo(new Point(tileSize * 7, tileSize * 9), new Point(tileSize, tileSize), true)
            },
            //Jump Pad
            {
                SpriteEnum.block | SpriteEnum.jumpPad,
                new spriteInfo(new Point(tileSize * 4, tileSize * 9), new Point(tileSize, tileSize), true)
            },
            //Hazards
            {
                SpriteEnum.block | SpriteEnum.hazard | SpriteEnum.spike,
                new spriteInfo(new Point(tileSize * 3, tileSize * 6), new Point(tileSize, tileSize), true)
            },
        };

        private Dictionary<SpriteEnum, animatedSpriteInfo> animatedSpriteInfoDictionary = new Dictionary<SpriteEnum, animatedSpriteInfo>()
        {
            //Player
            {
                SpriteEnum.player | SpriteEnum.small | SpriteEnum.running | SpriteEnum.player1,
                new animatedSpriteInfo(new Point(tileSize, 0), new Point(tileSize, tileSize * 2), true, 1, 3, msPerFrame)
            },
            {
                SpriteEnum.player | SpriteEnum.super | SpriteEnum.running | SpriteEnum.player1,
                new animatedSpriteInfo(new Point(0, tileSize * 2), new Point(tileSize, tileSize * 2), true, 1, 4, msPerFrame)
            },
            {
                SpriteEnum.player | SpriteEnum.fire | SpriteEnum.running | SpriteEnum.player1,
                new animatedSpriteInfo(new Point(0, tileSize * 4), new Point(tileSize, tileSize * 2), true, 1, 4, msPerFrame)
            },
            {
                SpriteEnum.player | SpriteEnum.small | SpriteEnum.running | SpriteEnum.player2,
                new animatedSpriteInfo(new Point(tileSize * 8, 0), new Point(tileSize, tileSize * 2), true, 1, 3, msPerFrame)
            },
            {
                SpriteEnum.player | SpriteEnum.super | SpriteEnum.running | SpriteEnum.player2,
                new animatedSpriteInfo(new Point(tileSize * 7, tileSize * 2), new Point(tileSize, tileSize * 2), true, 1, 4, msPerFrame)
            },
            {
                SpriteEnum.player | SpriteEnum.fire | SpriteEnum.running | SpriteEnum.player2,
                new animatedSpriteInfo(new Point(tileSize * 7, tileSize * 4), new Point(tileSize, tileSize * 2), true, 1, 4, msPerFrame)
            },
            //Hazards
            {
                SpriteEnum.block | SpriteEnum.hazard | SpriteEnum.muncher,
                new animatedSpriteInfo(new Point(tileSize * 4, tileSize * 6), new Point(tileSize, tileSize), true, 1, 2, msPerFrame)
            },
            {
                SpriteEnum.block | SpriteEnum.hazard | SpriteEnum.hothead,
                new animatedSpriteInfo(new Point(tileSize * 6, tileSize * 6), new Point(tileSize, tileSize), true, 1, 4, msPerFrame)
            },
        };
    }
}