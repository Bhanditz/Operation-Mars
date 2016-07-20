﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace coolgame
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public const int GAME_WIDTH = 1200;
        public const int GAME_HEIGHT = 600;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bgImage;
        Base baseBuilding;
        Ground ground;

        Button testButton;

        float deltaTime, totalGameTime;

        EnemySpawner enemySpawner1, enemySpawner2;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GAME_WIDTH;
            graphics.PreferredBackBufferHeight = GAME_HEIGHT;

            IsFixedTimeStep = true;
            GameManager.FrameLimiting = true;
            graphics.SynchronizeWithVerticalRetrace = false;

            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            SoundManager.SoundVolume = 50;
            SoundManager.MusicVolume = 50;

            Debug.Log("Game Initialized");
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        
            bgImage = Content.Load<Texture2D>("background");
            ground = new Ground(Content);
            baseBuilding = new Base(Content, GameManager.Ground.Top);

            EnemyFactory.LoadContent(Content);
            Debug.LoadContent(Content);

            testButton = new Button(Content, new Vector2(50, 50), 100, 100);

            SoundManager.AddSong(Content.Load<Song>("music"), "music");
            SoundManager.AddClip(Content.Load<SoundEffect>("towerlaser2"), "laser");
            SoundManager.AddClip(Content.Load<SoundEffect>("crawlerhit"), "crawlerhit");
            SoundManager.AddClip(Content.Load<SoundEffect>("steelroachhit"), "steelroachhit");
            SoundManager.AddClip(Content.Load<SoundEffect>("steelroachattack"), "steelroachattack");

            SoundManager.PlaySong("music");

            int seed = System.DateTime.Now.Year +
                System.DateTime.Now.Month +
                System.DateTime.Now.Day +
                System.DateTime.Now.Hour +
                System.DateTime.Now.Minute +
                System.DateTime.Now.Second +
                System.DateTime.Now.Millisecond;

            enemySpawner1 = new EnemySpawner(seed, new Vector2(Game.GAME_WIDTH + 50, GameManager.Ground.Top), Enemy.EnemyDirection.ToLeft);
            enemySpawner2 = new EnemySpawner(seed + 1337, new Vector2(-50, GameManager.Ground.Top), Enemy.EnemyDirection.ToRight);
            
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            totalGameTime = (float)gameTime.TotalGameTime.TotalMilliseconds;

            Debug.Update(deltaTime);
            InputManager.Update();
            GameManager.UpdateEntities(deltaTime);

            testButton.Update();
            ReadKeyPresses();

            enemySpawner1.Update(totalGameTime, deltaTime);
            enemySpawner2.Update(totalGameTime, deltaTime);
           
            base.Update(gameTime);
        }

        public void ReadKeyPresses()
        {
            if (InputManager.KeyDown(Keys.Escape))
                Exit();

            if (InputManager.KeyPress(Keys.C))
            {
                GameManager.ToggleFrameLimiting(this);
                Debug.Log("Toggled Frame Limiting");
            }

            if (InputManager.KeyPress(Keys.P))
            {
                GameManager.GamePaused = !GameManager.GamePaused;
                Debug.Log("Toggled Game Pause");
            }

            if (InputManager.KeyPress(Keys.F))
            {
                Debug.ToggleFPS();
                Debug.Log("Toggled FPS");
            }
            if (InputManager.KeyPress(Keys.M))
            {
                SoundManager.ToggleMute();
                Debug.Log("Toggled Mute");
            }

            if (InputManager.KeyPress(Keys.R))
            {
                Debug.ToggleRectangles();
                Debug.Log("Toggled Collision Boxes");
            }
            if (InputManager.KeyPress(Keys.L))
            {
                Debug.ToggleDebugLog();
                Debug.Log("Toggled Debug Log");
            }
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, null);

            spriteBatch.Draw(bgImage,
                new Rectangle(0, 0, GAME_WIDTH, GAME_HEIGHT),
                null,
                Color.White,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                LayerManager.GetLayerDepth(Layer.Background));

            GameManager.DrawEntities(spriteBatch);

            testButton.Draw(spriteBatch);

            Debug.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
