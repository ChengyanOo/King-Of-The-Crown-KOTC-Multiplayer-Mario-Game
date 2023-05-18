using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint1.Sprites;
using Sprint1.Factories;
using Sprint1.Factories.SpriteFactories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sprint1.Scrolling;

namespace Sprint1.LevelLoader
{
    public class Level
    {
        public string LevelName {get; set;}
        private int[,,] tileInts;
        private Dictionary<Tuple<int, int, int>, List<SpriteEnum>> questionBlockItems;
        private float[] parallaxValues;
        private int tileWidth;
        private int tileHeight;
        private int screenWidth;
        private int screenHeight;
        private LevelMap map;
        private Color backgroundColor;
        private Color maskColor;


        public List<Point> CrownSpawnPoints;
        public Layer EntityLayer { get => map.EntityLayer; }
        public List<Layer> BackgroundLayers { get => map.BackgroundLayers; }

        public Level(string levelName, int tileWidth, int tileHeight, 
            float[] parallaxValues, List<Point> crownSpawnPoints, Color backgroundColor, Color maskColor,
            int[,,] tileInts, Dictionary<Tuple<int, int, int>, List<SpriteEnum>> questionBlockItems, 
            int screenWidth, int screenHeight)
        {
            LevelName = levelName;

            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;

            this.tileInts = tileInts;
            this.parallaxValues = parallaxValues;
            CrownSpawnPoints = crownSpawnPoints;

            this.backgroundColor = backgroundColor;
            this.maskColor = maskColor;

            this.questionBlockItems = questionBlockItems;

            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        public void Initialize(Game1 game, GraphicsDeviceManager graphicsManager, EntityFactory entityFactory, 
            SpriteSheetFactory spriteFactory)
        {
            FormatScreen(graphicsManager);

            Point tileSize = new Point(tileWidth, tileHeight);

            for(int i = 0; i < CrownSpawnPoints.Count; i++)
            {
                CrownSpawnPoints[i] = CrownSpawnPoints[i] * tileSize;
            }

            LevelMap area = new(tileInts, 0, tileInts.GetLength(1) - 1, 0, tileInts.GetLength(2) - 1, parallaxValues, questionBlockItems, 
                tileWidth, tileHeight, maskColor);
            area.Initialize(game, screenWidth, screenHeight, entityFactory, spriteFactory);
            map = area;
        }

        public void FormatScreen(GraphicsDeviceManager graphicsManager)
        {
            if(graphicsManager.PreferredBackBufferWidth != screenWidth ||
                graphicsManager.PreferredBackBufferHeight != screenHeight)
            {
                graphicsManager.PreferredBackBufferWidth = screenWidth;
                graphicsManager.PreferredBackBufferHeight = screenHeight;
                graphicsManager.ApplyChanges();
            }
        }

        public void Draw(Game1 game, SpriteBatch spriteBatch, GameTime gameTime, bool isColliderVisible)
        {
            game.GraphicsDevice.Clear(backgroundColor);

            game.DrawBase(gameTime);

            foreach (var layer in map.allLayers)
            {
                layer.Draw(spriteBatch, isColliderVisible);
            }
        }

        public void Update(GameTime gameTime, Vector2 scrollingPosition)
        {
            EntityLayer.Update(gameTime, scrollingPosition);
        }

        public void AddSpriteToEntityLayer(ISprite sprite)
        {
            EntityLayer.SpriteList.Add(sprite);
        }

        public void RemoveSpriteFromEntityLayer(ISprite sprite)
        {
            EntityLayer.SpriteList.Remove(sprite);
        }

        public void ClearSprites()
        {
            foreach(var layer in map.allLayers)
            {
                layer.SpriteList.Clear();
            }
        }

        public List<ISprite> AllEntities
        {
            get {
                List<ISprite> list = new List<ISprite>();
                list.AddRange(EntityLayer.SpriteList);
                return list;
            }
        }
    }
}
