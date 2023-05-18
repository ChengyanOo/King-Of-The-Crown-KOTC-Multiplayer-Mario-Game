using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint1.Controllers;
using Sprint1.Sprites;
using Sprint1.Entities;
using System.Collections.Generic;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Factories;
using Sprint1.Collisions;
using Sprint1.LevelLoader;
using System.Diagnostics;
using System;
using Sprint1.Scrolling;
using Sprint1.Trackers;
using Sprint1.Audio;
using Sprint1.States.GameState;
using Sprint1.TimerSlider;
using Sprint1.Yannstempclasses;
using Microsoft.Xna.Framework.Media;

//using System.Diagnostics;
//using System.Security.Cryptography.X509Certificates;

namespace Sprint1
{

    public class Game1 : Game
    {
        //Graphics
        public SpriteBatch spriteBatch;
        public SpriteFont font;
        public Vector2 screenCenter;
        protected GraphicsDeviceManager graphics;
        protected PlayerFeedback feedback;
        protected const int screenOffset = 0;
        //Factories
        public SpriteSheetFactory spriteSheetFactory;
        protected Texture2D spriteSheet;
        protected EntityFactory entityFactory;
        //Controllers
        public List<IController> controllers;
        //Entity stuff
        public CollisionDetector2 collisionDetector;
        public CrownEntity crown;
        public List<PlayerEntity> playerList;
        //Level
        public List<ISprite> spriteList;
        protected XmlLevelReader xmlReader;
        protected string levelPath;
        protected Level level;
        //States
        public bool isGameOver;
        public bool isWon;
        public bool isMarioWon;
        public bool isLuigiWon;
        public WinningState winner;
        public GameOverState gameOver;
        public StartState startState;
        protected PlayState playState;
        protected PauseState pauseState;
        protected bool isPaused;
        protected bool isGameStarted;
        //Trackers
        public TimeTracker timeTracker;
        public LifeTracker lifeTracker;
        public CoinTracker coinTracker;
        public PointTracker pointTracker;
        public RoundTracker roundTracker;
        public WaypointTracker waypointTracker;
        public HoldTimeTracker holdTimeTracker { get; set; }
        public List<Vector2> waypointList = new List<Vector2>();
        public event EventHandler<PointEventArgs> SetScore;
        //Pickers
        public SpawnPicker spawnPicker;
        public MapPicker mapPicker = new MapPicker(new List<String>(){ "IceAge.xml", "DeepSpace.xml", "RedGiant.xml"});

        //Audio
        public AudioManager audioManager;
        public bool isGameOverReload;

        //public System.Timers.Timer aTimer;
        //public int time;
        private Texture2D back;
        private Texture2D front;
        public Texture2D marioWins;
        public Texture2D luigiWins;
        public Texture2D startScreen;
        public RoundTransition roundTransition;


        public  ProgressionBar progressionBar;
        public ProgressBarAnimated progressBarAnimated;

        public ProgressionBar progressionBarLuigi;
        public ProgressBarAnimated progressBarAnimatedLuigi;

        public AudioManager Sounds
        {
            get { return audioManager; }
        }
        public Game1(string levelPath = "Level1-1.xml")
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.levelPath = levelPath;
            isPaused = false;
            isGameStarted = false;
            isWon = false;

            isMarioWon = false;
            isLuigiWon = false;

            isGameOver = false;
            isGameOverReload = false;
        }

        protected override void Initialize()
        {
            audioManager = new AudioManager();
            roundTransition = new RoundTransition(this);

            //This loads content
            base.Initialize();
            //Make list of sprites
            spriteList = new List<ISprite>();
            controllers = new List<IController>();
            playerList = new List<PlayerEntity>();
            spriteSheetFactory = new SpriteSheetFactory(spriteSheet);
            entityFactory = new EntityFactory(this, spriteSheet);
            collisionDetector = new CollisionDetector2(0, new Rectangle(0, 0, 800, 480));
            //Progress Bar
            feedback = new PlayerFeedback(spriteBatch, font, this);
            //feedback = new PlayerFeedback(spriteBatch, font, this, front, back);
            progressionBar = new(back, front, 0, new(100, 50 + font.LineSpacing), this);
            progressBarAnimated= new(back, front, 30, new(100, 50 + font.LineSpacing), this);
            progressionBarLuigi = new(back, front, 0, new(1000, 50 + font.LineSpacing), this);
            progressBarAnimatedLuigi = new(back, front, 30, new(1000, 50 + font.LineSpacing), this);
            //Trackers
            timeTracker = new TimeTracker(this);
            lifeTracker = new LifeTracker(this);
            coinTracker = new CoinTracker();
            pointTracker = new PointTracker();
            roundTracker = new RoundTracker(this);
            holdTimeTracker = new HoldTimeTracker(this);
            //holdTimeTracker = new HoldTimeTracker();
            //aTimer = new System.Timers.Timer(2000);
            //aTimer.Elapsed += onTimeEvent;
            //aTimer.Start();
            //holdTimeTracker.PlayerOneTimerStart();

            //add event
            timeTracker.TimeRanOut += lifeTracker.DecreaseLife;
            coinTracker.IncLife += lifeTracker.IncreaseLife;

            timeTracker.SetEffect += audioManager.PlaySoundEffect;

            xmlReader = new();

            //List<String> pathNames = new List<String>(){ "IceAge.xml", "DeepSpace.xml", "RedGiant.xml"};
            //mapPicker = new MapPicker(pathNames);

            NewRound();
            isPaused = true;
            pauseState = new PauseState(this);
            playState = new PlayState(this);
            winner = new WinningState(graphics.GraphicsDevice, spriteBatch, font, this);
            gameOver = new GameOverState(graphics.GraphicsDevice, spriteBatch, font, this);
            startState = new StartState(graphics.GraphicsDevice, spriteBatch, font, this);
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteSheet = Content.Load<Texture2D>("KOTC");
            audioManager.LoadAudioFiles(Content);
            roundTransition.LoadRoundTransitionPics(Content);
            font = Content.Load<SpriteFont>("PlayerFeedback");
            marioWins = Content.Load<Texture2D>("MarioWins");
            luigiWins = Content.Load<Texture2D>("LuigiWins");
            startScreen = Content.Load<Texture2D>("mariovlugio");
            back = Content.Load<Texture2D>("backProgress");
            front = Content.Load<Texture2D>("frontProgress");
        }
        protected override void Update(GameTime gameTime)
        {
            //Debug.WriteLine(spawnPicker.Next());
            if (isPaused)
            {
                //PauseUpdate(gameTime);
                pauseState.PauseUpdate(gameTime);
            }
            else
            {
                //playState.PlayUpdate(gameTime);
                PlayUpdate(gameTime);
            }
        }

        //public void onTimeEvent(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    time++;
        //    Console.WriteLine("Time is: " + time);
        //}

        public void PlayUpdate(GameTime gameTime)
        {
            timeTracker.Update(gameTime);
            coinTracker.Update();
            //waypointTracker.Update();
            int controllerCount = controllers.Count;
            for (int i = 0; i < controllerCount; i++)
            {
                controllers[i].ProcessInputs();
            }

            level.Update(gameTime, screenCenter);

            collisionDetector.Update();
            base.Update(gameTime);

            //feedback.Update(timeTracker.time);
            progressionBar.Update(timeTracker.time);
            progressBarAnimated.Update(progressionBar.marioHoldTime);
            progressBarAnimatedLuigi.Update(progressionBarLuigi.luigiHoldTime);
        }
        
        public void changeIsPaused()
        {
            isPaused = !isPaused;
        }
        
        public void DrawBase(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!isPaused && !isMarioWon && !isLuigiWon)
            {
                level.Draw(this, spriteBatch, gameTime, collisionDetector.isColliderVisible);
                feedback.Draw(lifeTracker.life, (int)(pointTracker.points), 400 - ((int)(timeTracker.time)));
                progressionBar.Draw();
                progressionBarLuigi.Draw();
                progressBarAnimated.Draw();
                
                progressBarAnimatedLuigi.Draw();
                roundTransition.Draw();
             
            }
            else if (isMarioWon || isLuigiWon)
            {
                ClearAllSprites();
                controllers.Clear();
                controllers.Add(new KeyboardController());
                foreach (IController controller in controllers)
                {
                    controller.WinAndLoseAddControls(this);
                    controller.ProcessInputs();
                }
                audioManager.Mute();
                winner.WinScreen(gameTime);
               // feedback.Draw(lifeTracker.life, (int)(pointTracker.points), 400-((int)(timeTracker.time)));
            }
            else if (!isGameStarted)
            {
                startState.StartScreen(gameTime);
            }
           

        }

        private void ResetGameInfo()
        {
            if (isGameOverReload || isWon)
            {
                lifeTracker.Reset();
                coinTracker.Reset();
                pointTracker.Reset();


                timeTracker.Reset();
            }
            isGameOverReload = false;
            isWon = false;

            ClearAllSprites();
            playerList.Clear();
            controllers.Clear();
        }

        public void LoadLevel(string path)
        {
            ResetGameInfo();

            level = xmlReader.LoadNewMap(this, path, graphics, entityFactory, spriteSheetFactory);
            MediaPlayer.IsMuted = false;
            spriteList = level.AllEntities;
            this.Window.Title = level.LevelName;

            GetPlayerEntities();
            AddControllers(controllers);

            spawnPicker = new SpawnPicker(level.CrownSpawnPoints);
            Vector2 position = spawnPicker.Next();
            Debug.WriteLine(position);

            crown = new CrownEntity(this, playerList[0], SpriteEnum.crown | SpriteEnum.floating, position, true, Color.White);
            crown.Collider = new Rectangle((int)position.X - 6, (int)position.Y - 6, 60, 60);
            AddSprite((ISprite)crown);

            screenCenter = Vector2.Divide(getScreenDimensions(), 2.0f) + new Vector2(0, screenOffset);
            MakeCollidable();
            audioManager.PlaySoundtrack();
        }

        public void WinOrGameOverControls()
        {
            controllers.Clear();
            AddControllers(controllers);
            foreach (IController controller in controllers)
            {
                controller.WinAndLoseAddControls(this);
            }
        }

        private void GetPlayerEntities()
        {
            foreach (ISprite sprite in spriteList)
            {
                if (sprite is PlayerEntity)
                {
                    if ((((PlayerEntity)sprite).spriteType & SpriteEnum.player1) == SpriteEnum.player1)
                    {
                        playerList.Add((PlayerEntity)sprite);
                    }
                }
            }

            foreach (ISprite sprite in spriteList)
            {
                if (sprite is PlayerEntity)
                {
                    if ((((PlayerEntity)sprite).spriteType & SpriteEnum.player1) != SpriteEnum.player1)
                    {
                        playerList.Add((PlayerEntity)sprite);
                    }
                }
            }
        }

        private void AddControllers(List<IController> controllerList)
        {
            controllerList.Add(new KeyboardController());
            controllerList.Add(new GamepadController(PlayerIndex.One));
            controllerList.Add(new GamepadController(PlayerIndex.Two));

            controllerList[0].AddControlsGeneral(this, collisionDetector);
            controllerList[0].AddControlsMario(playerList[0]);
            controllerList[0].AddControlsLuigi(playerList[1]);

            controllerList[1].AddControlsGeneral(this, collisionDetector);
            controllerList[1].AddControlsMario(playerList[0]);

            controllerList[2].AddControlsGeneral(this, collisionDetector);
            controllerList[2].AddControlsMario(playerList[1]);
        }

        public Vector2 getScreenDimensions()
        {
            int screenWidth = graphics.PreferredBackBufferWidth;
            int screenHeight = graphics.PreferredBackBufferHeight;
            return new Vector2(screenWidth, screenHeight);
        }

        public void NewRound()
        {
            String map = mapPicker.Next();
            if (playerList.Count > 0)
            {
                playerList[0].crownEntity = null;
                playerList[1].crownEntity = null;
                crown.Exit();

            }
            LoadLevel(map);
        }

        public void ResetGame()
        {
            NewRound();
            if (isMarioWon || isLuigiWon)
            {
                this.SetScore += pointTracker.SetScore;
                onSetScore(new PointEventArgs { PointValue = 0 });
                this.SetScore -= pointTracker.SetScore;
            }
            isLuigiWon = false;
            isMarioWon = false;

            //isWon = false;
            //lifeTracker.Reset();
        }

        public void AddSprite(ISprite sprite)
        {
            level.AddSpriteToEntityLayer(sprite);
            spriteList.Add(sprite);
            if (sprite is IEntity)
            {
                IEntity entity = (IEntity)sprite;
                collisionDetector.insertCollidable(entity);
                if (entity.Position != entity.transformation(entity.Position))
                {
                    collisionDetector.insertMoving(entity);
                }
                if (entity is Entity)
                {
                    if (((Entity)entity).rigidbody != null && ((Entity)entity).rigidbody.velocity != Vector2.Zero)
                    {
                        collisionDetector.insertMoving(entity);
                    }
                }
            }
        }

        private void MakeCollidable()
        {
            for (int i = 0; i < spriteList.Count; i++)
            {
                ISprite sprite = spriteList[i];
                if (sprite is IEntity)
                {
                    IEntity entity = (IEntity)sprite;
                    collisionDetector.insertCollidable(entity);
                    if (entity.Position != entity.transformation(entity.Position))
                    {
                        collisionDetector.insertMoving(entity);
                    }
                    if (entity is Entity)
                    {
                        if (((Entity)entity).rigidbody != null && ((Entity)entity).rigidbody.velocity != Vector2.Zero)
                        {
                            collisionDetector.insertMoving(entity);
                        }
                    }
                }
            }
        }

        /*Yann: This method still doesn't seems to be able to remove entity from the collidable list for some reason, I added line 297 - "collisionDetector.removeCollidable(entity);"
         * which tempereroly solved this.
         * */
        public void RemoveSprite(ISprite sprite)
        {
            spriteList.Remove(sprite);
            level.RemoveSpriteFromEntityLayer(sprite);
            if (sprite is IEntity)
            {
                IEntity entity = (IEntity)sprite;
                collisionDetector.removeCollidable(entity);
                
                if (collisionDetector.containsMoving(entity))
                {
                    collisionDetector.removeMoving(entity);
                }
            }
        }

        public void ClearAllSprites()
        {
            if (level != null)
            {
                level.ClearSprites();
            }
            spriteList.Clear();
            collisionDetector.clearCollidable();
            collisionDetector.clearMoving();
        }

        public void AddMoving(IEntity entity)
        {
            collisionDetector.insertMoving(entity);
        }

        public void RemoveMoving(IEntity entity)
        {
            collisionDetector.removeMoving(entity);
        }

        public ISprite CreateSprite(SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0)
        {
            return spriteSheetFactory.Create(spriteType, position, isRight, color, layerDepth);
        }

        public ISprite CreateEntity(SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0)
        {
            return entityFactory.Create(this, spriteType, position, isRight, color, layerDepth);
        }

        public Rectangle GetCollider(SpriteEnum spriteType, Vector2 position)
        {
            return entityFactory.GetCollider(spriteType, position);
        }

        public Point GetColliderOffset(SpriteEnum spriteType)
        {
            return entityFactory.GetColliderOffset(spriteType);
        }

        public float GetTimeRemaining()
        {
            return timeTracker.time;
        }

        protected virtual void onSetScore(PointEventArgs e)
        {
            EventHandler<PointEventArgs> handler = SetScore;
            if (handler != null)
                handler(this, e);
        }


        public void RemoveCollidable(IEntity entity)
        {
            collisionDetector.removeCollidable(entity);
        }

        public void ClearAllMoving()
        {
            collisionDetector.clearMoving();
        }

        public void StartGame()
        {
            if (isGameStarted == false)
            {
                isGameStarted = true;
                isPaused = false;
            }          
        }

    }
}