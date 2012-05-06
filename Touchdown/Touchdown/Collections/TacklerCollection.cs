using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PingDevelopment.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Touchdown.Enemies;

namespace Touchdown.Collections
{
    public class TacklerCollection : List<Tackler>
    {
        /// <summary>
        /// The Texture we will use for all StickMen.
        /// This will get passed to each StickMan instance as it is created.
        /// </summary>
        Texture2D Texture;

        /// <summary>
        /// The SpriteBatch to pass to each individual StickMan
        /// </summary>
        SpriteBatch SpriteBatch;

        /// <summary>
        /// The time (total milliseconds) when the last StickMan was created
        /// </summary>
        private double LastTacklerCreateTime;

        /// <summary>
        /// Initializes a new StickManCollection using the values provided
        /// </summary>
        /// <param name="SpriteBatch">The SpriteBatch to pass to each StickMan</param>
        /// <param name="Texture">The Texture2D to pass to use for each StickMan</param>
        public TacklerCollection(SpriteBatch SpriteBatch, Texture2D Texture)
        {
            this.SpriteBatch = SpriteBatch;
            this.Texture = Texture;
        }

        /// <summary>
        /// Create a new stick man if it is time
        /// </summary>
        /// <param name="gameTime">The current GameTime of the game</param>
        public void CreateTackler(GameTime gameTime)
        {
            //Determine the current game time (as a total number of milliseconds)
            Double now = gameTime.TotalGameTime.TotalMilliseconds;

            //Test to see if it is "time" to create a new StickMan
            //and there aren't already too many stickmen...
            if (now - LastTacklerCreateTime > GameManager.MinEnemyCreateRate
            && this.Count < GameManager.MaxConcurrentEnemies)
            {
                //Record that the last stick man was created now
                LastTacklerCreateTime = now;

                //We need a Random Number generator for few things
                Random rnd = new Random();

                //Create a new StickMan instance, pass in the sprite batch and texture
                Tackler tackler = new Tackler(this.SpriteBatch, this.Texture);

                //Let each tackler be fatal to the runner if they collide
                tackler.IsFatal = true;

                //Give each tackler a single life. If they lose it, they don't come back to life.
                tackler.LivesRemaining = 1;

                //Give each tackler a health of 1 point. Make their current health the same...
                tackler.HealthMax = 1;
                tackler.HealthCurrent = tackler.HealthMax;

                //Allow 10 points to be added to the score each time a tackler is dodged.
                tackler.PointValue = 10;

                //Position the tackler at a random horizontal position.
                tackler.Position.X = rnd.Next(0, tackler.MaxOnScreenX);

                //Position the tackler just off the top of the screen
                tackler.Position.Y = -tackler.Height;

                //Randomly decided the horizontal direction. -1 = left, +1 = right
                int horizontalDirection = rnd.Next(2) == 1 ? -1 : +1;

                //Give the stick man a random velocity, in the chosen horizontalDirection
                //We'll keep it between MinStickManVelocity and MaxStickManVelocity
                tackler.Velocity.X = 
                    horizontalDirection
                    * rnd.Next(GameManager.MinEnemyVelocity, GameManager.MaxEnemyVelocity + 1);

                //the vertical velocity needs to be positive (moves from top to bottom)
                //We'll keep it between MinStickManVelocity and MaxStickManVelocity
                tackler.Velocity.Y =
                    rnd.Next(GameManager.MinEnemyVelocity, GameManager.MaxEnemyVelocity + 1);

                //Ok, the StickMan is ready for battle. Add them to the collection
                this.Add(tackler);
            }
        }

        /// <summary>
        /// Draw each StickMan in the collection
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        public void Draw(GameTime gameTime)
        {
            //Draw each of the stickmen
            foreach (Tackler tackler in this)
                tackler.Draw(gameTime);
        }

        /// <summary>
        /// Update each StickMan in the collection
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        public void Update(GameTime gameTime)
        {
            //Update all of the existing stickmen...
            for (int i=0; i < this.Count; i++)
            {
                this[i].Update(gameTime);
                if (!this[i].IsAlive) this.RemoveAt(i);
            }

            //Create a new StickMan if it is time..
            CreateTackler(gameTime);
        }
    }
}
