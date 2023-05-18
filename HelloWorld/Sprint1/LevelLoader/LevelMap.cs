using Microsoft.Xna.Framework;
using Sprint1.Entities;
using Sprint1.Factories;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Scrolling;
using Sprint1.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Sprint1.LevelLoader
{
    internal class LevelMap
    {
        private Game1 game;
        private Camera camera;
        private int tileWidth;
        private int tileHeight;

        private readonly float[] parallaxValues;
        private readonly int[,,] tileInts;
        private const int offset = 0;
        private readonly int startColumn, endColumn, startRow, endRow;
        private readonly int numLayers;
        private readonly int levelColumns, levelRows;
        private Color maskColor;
        private readonly Color white = Color.White;

        private Dictionary<Tuple<int, int, int>, List<SpriteEnum>> spawningBlockItems;
        internal Layer EntityLayer { get; private set; }
        internal List<Layer> BackgroundLayers { get; private set; }
        internal List<Layer> allLayers;


        public LevelMap(int[,,] tileInts, int startColumn, int endColumn, int startRow, int endRow,
            float[] parallaxValues, Dictionary<Tuple<int, int, int>, List<SpriteEnum>> spawningBlocks, 
            int tileWidth, int tileHeight, Color maskColor)
        {
            this.tileInts = tileInts;
            numLayers = tileInts.GetLength(0);
            this.startColumn = startColumn;
            this.startRow = startRow;

            levelColumns = tileInts.GetLength(1);
            levelRows = tileInts.GetLength(2);
            //Either end column/row or last column/row in array, whichever is smaller
            this.endColumn = Math.Min(endColumn, levelColumns - 1);
            this.endRow = Math.Min(endRow, levelRows - 1);

            this.spawningBlockItems = spawningBlocks;
            this.parallaxValues = parallaxValues;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;

            BackgroundLayers = new List<Layer>();
            allLayers = new List<Layer>();

            this.maskColor = maskColor;
        }

        public Rectangle Limits
        {
            get 
            {
                return new Rectangle(startColumn * tileWidth, startRow * tileHeight, (endColumn - startColumn + 1) * tileWidth, (endRow - startRow + 1)* tileHeight);
            }
        }

        public void Initialize(Game1 game, int screenWidth, int screenHeight, EntityFactory entityFactory, SpriteSheetFactory spriteFactory)
        {
            this.game = game;
            camera = new Camera(game.GraphicsDevice.Viewport);
            AddSprites(screenWidth, screenHeight, entityFactory, spriteFactory);
        }

        private void AddSprites(int screenWidth, int screenHeight, EntityFactory entityFactory, SpriteSheetFactory spriteFactory)
        {
            //tileInts is a 3 dimensional array.
            //tileInts[0, x, y] would be LAYER 0 (first/back layer), COLUMN x, ROW y

            for (int layerCount = 0; layerCount < numLayers; layerCount++)
            {
                Layer layer = new Layer(camera, screenWidth, screenHeight, new Vector2(parallaxValues[layerCount]), Limits);
                AddSpritesToLayer(layer, layerCount, entityFactory, spriteFactory);

                if (IsEntityLayer(layerCount))
                {
                    EntityLayer = layer;

                    layer.Parallax = Vector2.One;
                }
                else
                {
                    BackgroundLayers.Add(layer);
                }
                allLayers.Add(layer);
            }
        }

        private void AddSpritesToLayer(Layer layer, int layerNum, EntityFactory entityFactory, SpriteSheetFactory spriteFactory)
        {
            bool isEntityLayer = IsEntityLayer(layerNum);

            for (int columnCount = startColumn; columnCount <= endColumn; columnCount++)
            {
                for (int rowCount = startRow; rowCount <= endRow; rowCount++)
                {
                    if (tileInts[layerNum, columnCount, rowCount] != 0)
                    {
                        SpriteEnum spriteType = (SpriteEnum)tileInts[layerNum, columnCount, rowCount];
                        Vector2 position = new Vector2(columnCount * tileWidth - offset, rowCount * tileHeight - offset);
                        bool isRight = true;
                        float layerDepth = (float)layerNum / 10;
                        ISprite sprite;

                        Color color = IsMasked(spriteType) ? maskColor : white;
                        if (isEntityLayer)
                        {
                            sprite = entityFactory.Create(game, spriteType, position, isRight, color, layerDepth);
                            if (IsSpawning(spriteType))
                            {
                                Tuple<int, int, int> t = new(layerNum, columnCount, rowCount);
                                spawningBlockItems.TryGetValue(t, out List<SpriteEnum> list);

                                ((BlockEntity)sprite).turnSpawning(list);  //Change null to a SpriteEnum list
                            }
                        }
                        else
                        {
                            sprite = spriteFactory.Create(spriteType, position, isRight, color, layerDepth);
                        }

                        layer.SpriteList.Add(sprite);
                    }
                }
            }
        }

        private bool IsSpawning(SpriteEnum spriteType)
        {
            return (spriteType & SpriteEnum.block) == SpriteEnum.block &&
                (spriteType & SpriteEnum.spawning) == SpriteEnum.spawning;
        }

        private bool IsMasked(SpriteEnum spriteType)
        {
            bool isBlock = (spriteType & SpriteEnum.block) == SpriteEnum.block;
            bool isPlatformOrWall = (spriteType & SpriteEnum.platform) == SpriteEnum.platform ||
                (spriteType & SpriteEnum.wall) == SpriteEnum.wall;
            return isBlock && isPlatformOrWall;
        }

        private bool IsEntityLayer(int i)
        {
            Debug.WriteLine(Math.Abs(parallaxValues[i] - 1.0f) < 0.1f);
            return Math.Abs(parallaxValues[i] - 1.0f) < 0.1f;
        }

        internal void ResetCamera()
        {
            foreach(var layer in allLayers)
            {
                layer.ResetCamera(Limits);
            }
        }
    }
}
