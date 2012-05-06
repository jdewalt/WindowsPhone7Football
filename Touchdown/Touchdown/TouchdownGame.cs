using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using Touchdown.Collections;
using Touchdown.Sprites;
using Touchdown.Enemies;
using PingDevelopment.Framework;

namespace Touchdown
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TouchdownGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont scoreFont;
        Texture2D welcomeScreen;
        Texture2D pauseScreen;
        Texture2D gameOverScreen;

        Runner runner;
        TacklerCollection tacklers;
        FieldCollection field;
        BlockerCollection blockers;

        

        public TouchdownGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            field = new FieldCollection(spriteBatch, this.Content.Load<Texture2D>("Images/BackgroundLight"), this.Content.Load<Texture2D>("Images/BackgroundDark"));
            tacklers = new TacklerCollection(spriteBatch, this.Content.Load<Texture2D>("Images/Tackler"));

            blockers = new BlockerCollection(spriteBatch, this.Content.Load<Texture2D>("Images/Runner"));

            runner = new Runner(spriteBatch, this.Content.Load<Texture2D>("Images/Runner"));

            // Screen elements
            scoreFont = Content.Load<SpriteFont>("Fonts/ScoreFont");

            //welcomeScreen = Content.Load<Texture2D>("Images/WelcomeScreen");
            //pauseScreen = Content.Load<Texture2D>("Images/PauseScreen");
            //gameOverScreen = Content.Load<Texture2D>("Images/GameOverScreen");
            //gameOverSound = Content.Load<SoundEffect>("Sounds/GameOverSound");

            //GameManager.GameScreen = GameScreens.Welcome;
            StartGame();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            switch (GameManager.GameScreen)
            {
                //case GameScreens.Welcome:
                //    UpdateWelcomeScreen(gameTime);
                //    break;

                //case GameScreens.Pause:
                //    UpdatePauseScreen(gameTime);
                //    break;

                case GameScreens.GameOver:
                    //UpdateGameOverScreen(gameTime);
                    this.Exit();
                    break;

                case GameScreens.GamePlay:
                default:
                    UpdateGamePlayScreen(gameTime);
                    break;

            } 

            base.Update(gameTime);
        }

        /// <summary>
        /// Update the screen during game play
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateGamePlayScreen(GameTime gameTime)
        {
            GameManager.AdvanceLevel();

            field.Update(gameTime);
            runner.Update(gameTime);
            tacklers.Update(gameTime);
            blockers.Update(gameTime);

            CheckForCollisions();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            field.Draw(gameTime);
            runner.Draw(gameTime);
            tacklers.Draw(gameTime);
            blockers.Draw(gameTime);
            DrawScore();
            //switch (GameManager.GameScreen)
            //{
            //    case GameScreens.Welcome:
            //        spriteBatch.Draw(welcomeScreen, Vector2.Zero, Color.White);
            //        break;

            //    case GameScreens.Pause:
            //        spriteBatch.Draw(pauseScreen, Vector2.Zero, Color.White);
            //        break;

            //    case GameScreens.GameOver:
            //        spriteBatch.Draw(gameOverScreen, Vector2.Zero, Color.White);
            //        break;
            //}

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Check to see if any collisions have occurred
        /// </summary>
        private void CheckForCollisions()
        {
            //Next check to see if any of the StickMen collided with any of the Robots...
            for (int s = 0; s < tacklers.Count; s++)
            {
                Tackler tackler = tacklers[s];
                if (!tackler.IsAlive)
                {
                    tacklers.RemoveAt(s);
                }
                else if (tackler.CurrentRectangle.Intersects(runner.CurrentRectangle) && !tackler.Blocked)
                {
                    //CRUD! The StickMan collided with us!!!!
                    runner.RecordHit(tackler);
                    tacklers.RemoveAt(s);

                    //TODO: Play the StickMan explosion
                    //explosions.CreateStickManExplosion(stickMan);
                    //Record the hit on the robot...
                    GameManager.GameScreen = GameScreens.GameOver;

                    //TODO: Play the Game Over sound
                    //explosions.CreateRobotExplosion(robot);
                    //Since we already know we've collided, break out the of the loop
                    break;
                }
                for (int b = 0; b < blockers.Count; b++)
                {
                    if (tackler.CurrentRectangle.Intersects(blockers[b].CurrentRectangle))
                    {
                       // tacklers.RemoveAt(s);
                       // blockers.RemoveAt(b);

                        tacklers[b].Velocity = -GameManager.GetFieldSpeed();
                        tacklers[b].Blocked = true;
                        blockers[b].Velocity = GameManager.GetFieldSpeed();
                        blockers[b].Blocked = true;

                    }
                }
            }

            // Next see if the field has fallen completely off the screen
            for (int f = 0; f < field.Count; f++)
            {
                Field fe = field[f];
                if (!fe.IsAlive)
                {
                    // The field graphic is no longer being used, so recycle it
                    field.RemoveAt(f);
                }
            }
        }

        /// <summary>
        /// Draw the score, and lives remaining on the screen
        /// </summary>
        private void DrawScore()
        {
            //Build the score string
            string scoreString = string.Format("SCORE: {0}", GameManager.Score);

            //Position it 20 pixels from the top, and 20 pixels from the left
            Vector2 scorePosition = new Vector2(20);

            //Draw it on the screen using the scoreFont, at the scorePosition in black
            spriteBatch.DrawString(scoreFont, scoreString, scorePosition, Color.White);

            //Build a string to show the number of lives remaining
            string levelString = string.Format("LEVEL: {0}", GameManager.Level);

            //Determine the size of the rendered string so we can position it.
            float levelStringWidth = scoreFont.MeasureString(levelString).X;

            //Position it 20 pixels from the right edge, and 20 pixels from the top
            Vector2 levelPosition = new Vector2(350, 20);

            //Draw it on the screen at the determined position, in black
            spriteBatch.DrawString(scoreFont, levelString, levelPosition, Color.White);
        }

        /// <summary>
        /// Initialize the game values before gameplay starts.
        /// </summary>
        private void StartGame()
        {
            //Clear the collections:
            field.Clear();
            tacklers.Clear();
            
            //Initialize the Robot. Center it
            runner.CenterRunner();

            //Reset the Score, and the current level
            GameManager.Score = 0;
            GameManager.Level = 0;
        }
    }
}
