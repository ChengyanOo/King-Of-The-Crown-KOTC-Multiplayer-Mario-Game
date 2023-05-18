using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Sprint1.Factories;
using Sprint1.Factories.SpriteFactories;

namespace Sprint1.LevelLoader
{
    public class XmlLevelReader
    {
        private readonly XmlDocument doc;
        private Dictionary<string, int> spriteCodes;
        private Dictionary<string, Color> colorCodes;

        public XmlLevelReader()
        {
            doc = new XmlDocument();
            CreateDictionaries();
        }

        public Level LoadNewMap(Game1 game, string fileName, GraphicsDeviceManager graphicsManager, EntityFactory entityFactory, SpriteSheetFactory spriteFactory)
        {
            if (!File.Exists(fileName))
            {
                Environment.Exit((int)ExitCodes.InvalidFilename);
            }
            doc.Load(fileName);
            Level level = GenerateLevel(game);
            level.Initialize(game, graphicsManager, entityFactory, spriteFactory);
            return level;
        }

        public Level GenerateLevel(Game1 game)
        {
            string levelName;
            int tileWidth, tileHeight, levelColumns, levelRows, screenWidth, screenHeight;
            tileWidth = tileHeight = -1;
            levelColumns = levelRows = screenWidth = screenHeight = (tileWidth - 1);

            bool formattedCorrectly = this.NecessaryElementsPresent() &&
                System.Int32.TryParse(doc.GetElementsByTagName("TileWidth").Item(0).InnerText, out tileWidth) &&
                System.Int32.TryParse(doc.GetElementsByTagName("TileHeight").Item(0).InnerText, out tileHeight) &&
                System.Int32.TryParse(doc.GetElementsByTagName("LevelColumns").Item(0).InnerText, out levelColumns) &&
                System.Int32.TryParse(doc.GetElementsByTagName("LevelRows").Item(0).InnerText, out levelRows) &&
                System.Int32.TryParse(doc.GetElementsByTagName("ScreenWidth").Item(0).InnerText, out screenWidth) &&
                System.Int32.TryParse(doc.GetElementsByTagName("ScreenHeight").Item(0).InnerText, out screenHeight) &&
                (tileWidth > 0 && tileHeight > 0 && levelColumns > 0 && screenWidth > 0 && screenHeight > 0)
            ;

            //initialize title
            if(doc.GetElementsByTagName("Title").Item(0) == null)
            {
                levelName = "Monogame";
            } else
            {
                levelName = doc.GetElementsByTagName("Title").Item(0).InnerText;
            }

            List<Point> points = GetSpawnPoints(out bool pointsFormattedCorrectly);
            formattedCorrectly = formattedCorrectly && pointsFormattedCorrectly;


            //initialize tileInts
            XmlNodeList layerList = doc.GetElementsByTagName("Layer");
            int layerCount = layerList.Count;
            float[] parallax = new float[layerCount];

            for(int i = 0; i < layerCount; i++) { 
                XmlNode p = layerList.Item(i).Attributes.GetNamedItem("parallax");
                formattedCorrectly = formattedCorrectly && 
                    p != null && p.InnerText != null && 
                    float.TryParse(p.InnerText.ToString(), out parallax[i]);
            }


            Color backgroundColor = GetBackgroundColor();
            Color maskColor = GetMaskColor();

            if (!formattedCorrectly)
            {
                Environment.Exit((int)ExitCodes.XmlFormatError);
            }

            int[,,] tileInts = new int[layerCount, levelColumns, levelRows];
            Dictionary<Tuple<int, int, int>, List<SpriteEnum>> spawningBlockItems = new Dictionary<Tuple<int, int, int>, List<SpriteEnum>>();
            GenerateTileInts(layerList, tileInts, spawningBlockItems);

            Level level = new Level(levelName, tileWidth, tileHeight, parallax, points, backgroundColor, 
                maskColor, tileInts, spawningBlockItems, screenWidth, screenHeight);

            return level;
        }

        private bool NecessaryElementsPresent()
        {
            //Bare minimum, there is no null necessary elements,
            //also get num columns for when we check each layer later
            return NoNulls() && LayersFormattedCorrectly();
        }

        private bool NoNulls()
        {
            return (doc.GetElementsByTagName("TileWidth").Item(0) != null) &&
                (doc.GetElementsByTagName("TileHeight").Item(0) != null) &&
                (doc.GetElementsByTagName("LevelRows").Item(0) != null) &&
                (doc.GetElementsByTagName("LevelColumns").Item(0) != null) &&
                (doc.GetElementsByTagName("ScreenWidth").Item(0) != null) &&
                (doc.GetElementsByTagName("ScreenHeight").Item(0) != null) &&
                (doc.GetElementsByTagName("Layer").Item(0) != null);
        }

        private List<Point> GetSpawnPoints(out bool correctFormat)
        {
            correctFormat = true;
            XmlNodeList spawnPointNodes = doc.GetElementsByTagName("SpawnPoints").Item(0).ChildNodes;

            List<Point> points = new List<Point>();

            for (int i = 0; i < spawnPointNodes.Count; i++)
            {
                int x, y;
                x = y = 0;
                correctFormat = correctFormat &&
                    System.Int32.TryParse(spawnPointNodes.Item(i).Attributes.GetNamedItem("x").InnerText, out x);
                correctFormat = correctFormat &&
                    System.Int32.TryParse(spawnPointNodes.Item(i).Attributes.GetNamedItem("y").InnerText, out y);
                points.Add(new Point(x, y));
            }
            correctFormat = correctFormat && points.Count > 0;

            return points;
        }

        private bool LayersFormattedCorrectly()
        {
            bool correct = true;
            //Get how many columns specified by LevelColumns element
            int numColumns = -1;
            correct = correct && Int32.TryParse(doc.GetElementsByTagName("LevelColumns").Item(0).InnerText, out numColumns);

            XmlNodeList layerList = doc.GetElementsByTagName("Layer");
            for (int i = 0; i < layerList.Count; i++)
            {
                XmlNode layer = layerList[i];
                correct = correct && layer.ChildNodes.Count == numColumns;
            }

            return correct;
        }

        private void GenerateTileInts(XmlNodeList layerList, int[,,] tileInts, Dictionary<Tuple<int, int, int>, List<SpriteEnum>> spawningBlockItems)
        {
            int totalLayers = tileInts.GetLength(0);

            for(int i = 0; i < totalLayers; i++)
            {
                ParseLayer(i, layerList.Item(i), tileInts, spawningBlockItems);
            }
        }

        private void ParseLayer(int layerNum, XmlNode layer, int[,,] tileInts, Dictionary<Tuple<int, int, int>, List<SpriteEnum>> spawningBlockItems)
        {
            XmlNodeList columnList = layer.ChildNodes;
            int totalColumns = tileInts.GetLength(1);

            for(int j = 0; j < totalColumns; j++)
            {
                ParseColumn(layerNum, j, columnList.Item(j), tileInts, spawningBlockItems);
            }
        }

        private void ParseColumn(int layerNum, int columnNum, XmlNode column, int[,,] tileInts, Dictionary<Tuple<int, int, int>, List<SpriteEnum>> spawnableBlockList)
        {
            XmlNodeList sprites = column.ChildNodes;
            int totalRows = tileInts.GetLength(2);

            for(int k = 0; k < totalRows; k++)
            {
                    tileInts[layerNum, columnNum, k] = 0;
            }

            for(int m = 0; m < sprites.Count; m++)
            {
                if (sprites.Item(m).Attributes.GetNamedItem("y") != null &&
                   Int32.TryParse(sprites.Item(m).Attributes.GetNamedItem("y").InnerText, out int row) && 
                   (row < totalRows))
                {
                    tileInts[layerNum, columnNum, row] = ParseSprite(sprites.Item(m));
                    
                    if(sprites.Item(m).ChildNodes.Count > 0)
                    {
                        List<SpriteEnum> blockItems = new List<SpriteEnum>();
                        foreach (XmlNode child in sprites.Item(m).ChildNodes)
                        {
                            blockItems.Add((SpriteEnum)ParseSprite(child));
                        }
                        spawnableBlockList.Add(new Tuple<int, int, int>(layerNum, columnNum, row), blockItems);
                    }
                }
            }
        }

        private int ParseSprite(XmlNode sprite)
        {
            string name = sprite.Name;
            bool success;

            int spriteName, spriteType, spritePowerState, spriteActionState, spriteVariant;
            spriteType = spritePowerState = spriteActionState = spriteVariant = 0;

            success = spriteCodes.TryGetValue(name, out spriteName);
            if (name is "player")
            {
                string power = sprite.Attributes.GetNamedItem("power").InnerText;
                string action = sprite.Attributes.GetNamedItem("action").InnerText;
                string variant = sprite.Attributes.GetNamedItem("variant").InnerText;
                success = success && spriteCodes.TryGetValue(power, out spritePowerState) && 
                    spriteCodes.TryGetValue(action, out spriteActionState) &&
                    spriteCodes.TryGetValue(variant, out spriteVariant);
                spriteType = spritePowerState | spriteActionState;
            }
            else
            {
                string type = sprite.Attributes.GetNamedItem("type").InnerText;
                int verticalCode = 0, horizontalCode = 0, spawningCode = 0;
                XmlNode vertical = sprite.Attributes.GetNamedItem("vertical");
                XmlNode horizontal = sprite.Attributes.GetNamedItem("horizontal");
                XmlNode variant = sprite.Attributes.GetNamedItem("variant");
                if(vertical != null)
                {
                    success = success && spriteCodes.TryGetValue(vertical.InnerText, out verticalCode);
                }
                if(horizontal != null)
                {
                    success = success && spriteCodes.TryGetValue(horizontal.InnerText, out horizontalCode);
                }
                if(variant != null)
                {
                    success = success && spriteCodes.TryGetValue(variant.InnerText, out spriteVariant);
                }
                if((type == "brick" || type == "question" || type == "pipe") && (sprite.ChildNodes.Count > 0))
                {
                    success = success && spriteCodes.TryGetValue("spawning", out spawningCode);
                }

                success = success && spriteCodes.TryGetValue(type, out spriteType);
                Debug.WriteLineIf(!success, type);
                spriteType |= verticalCode | horizontalCode | spawningCode;
            }

            if(!success)
            {
                Environment.Exit((int)ExitCodes.XmlSpriteFormatError);
            }
            return spriteName | spriteType | spriteVariant;
        }

        private Color GetBackgroundColor()
        {
            string colorName = "Blue";
            XmlNodeList list = doc.GetElementsByTagName("BackgroundColor");
            if (list.Count > 0)
            {
                colorName = list[0].InnerText;
            }

            bool inDictionary = colorCodes.TryGetValue(colorName, out Color bgColor);

            if (!inDictionary)
            {
                bgColor = Color.CornflowerBlue;
            }

            return bgColor;
        }

        private Color GetMaskColor()
        {
            string colorName = "White";
            XmlNodeList list = doc.GetElementsByTagName("MaskColor");
            if (list.Count > 0)
            {
                colorName = list[0].InnerText;
            }

            bool inDictionary = colorCodes.TryGetValue(colorName, out Color maskColor);

            if (!inDictionary)
            {
                maskColor = Color.White;
            }

            return maskColor;
        }

        private void CreateDictionaries()
        {
            colorCodes = new Dictionary<string, Color>
            {
                { "White", Color.White },
                { "Black", Color.Black },
                { "Blue", Color.CornflowerBlue },
                { "Red", Color.OrangeRed },
                { "Purple", Color.MediumVioletRed },
            };

            spriteCodes = new Dictionary<string, int>
            {
                //orient codes
                { "top", (int)SpriteEnum.top },
                { "bottom", (int)SpriteEnum.bot },
                { "left", (int)SpriteEnum.left },
                { "right", (int)SpriteEnum.right},
                { "mid", (int)SpriteEnum.mid },
                //player codes
                { "player", (int)SpriteEnum.player },
                { "player1", (int)SpriteEnum.player1 },
                { "player2", (int)SpriteEnum.player2 },

                { "dead", (int)SpriteEnum.dead },
                { "small", (int)SpriteEnum.small },
                { "super", (int)SpriteEnum.super },
                { "fire", (int)SpriteEnum.fire },

                { "idle", (int)SpriteEnum.idle },
                { "running", (int)SpriteEnum.running },
                { "jumping", (int)SpriteEnum.jumping },
                { "falling", (int)SpriteEnum.falling },
                { "crouching", (int)SpriteEnum.crouching },
                //crown
                { "crown", (int)SpriteEnum.crown },
                { "floating", (int)SpriteEnum.floating },
                { "thrown", (int)SpriteEnum.thrown },
                { "attached", (int)SpriteEnum.attached },
                //enemy codes
                { "enemy", (int)SpriteEnum.enemy },
                { "koopa", (int)SpriteEnum.koopa },
                { "shellKoopa", (int)SpriteEnum.shellKoopa },
                { "goomba", (int)SpriteEnum.goomba },
                { "plant", (int)SpriteEnum.piranhaPlant },
                //item codes
                { "item", (int)SpriteEnum.item },
                { "small coin", (int)SpriteEnum.smallCoin },
                { "big coin", (int)SpriteEnum.bigCoin },
                { "green mushroom", (int)SpriteEnum.greenMushroom },
                { "red mushroom", (int)SpriteEnum.redMushroom },
                { "fire flower", (int)SpriteEnum.fireFlower },
                { "star", (int)SpriteEnum.star },
                //block codes
                { "block", (int)SpriteEnum.block },
                { "spawning", (int)SpriteEnum.spawning },
                { "brick", (int)SpriteEnum.brick },
                { "hidden", (int)SpriteEnum.hidden },
                { "floor", (int)SpriteEnum.floor },
                { "platform", (int)SpriteEnum.platform },
                { "wall", (int)SpriteEnum.wall },
                { "tapered", (int)SpriteEnum.tapered },
                { "jumpPad", (int)SpriteEnum.jumpPad },
                //Hazard codes
                { "hazard", (int)SpriteEnum.hazard },
                { "spike", (int)SpriteEnum.spike },
                { "muncher", (int)SpriteEnum.muncher },
                { "hothead", (int)SpriteEnum.hothead },
            };
        }
    }
}
