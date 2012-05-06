using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PingDevelopment.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Touchdown.Sprites;

namespace Touchdown.Collections
{
    public class FieldCollection : List<Field>
    {
        /// <summary>
        /// The Texture we will use for all StickMen.
        /// This will get passed to each StickMan instance as it is created.
        /// </summary>
        Texture2D Texture1;
        Texture2D Texture2;

        /// <summary>
        /// The SpriteBatch to pass to each individual StickMan
        /// </summary>
        SpriteBatch SpriteBatch;

        private int _currentTexture = 1;
        private Texture2D CurrentTexture 
        {
            get 
            {
                if (_currentTexture == 1) return Texture1;
                else return Texture2;
            }
        }
     /// <summary>
        /// Initializes a new FieldCollection using the values provided
        /// </summary>
        /// <param name="SpriteBatch">The SpriteBatch to pass to each StickMan</param>
        /// <param name="Texture">The Texture2D to pass to use for each StickMan</param>
        public FieldCollection(SpriteBatch SpriteBatch, Texture2D texture1, Texture2D texture2)
        {
            this.SpriteBatch = SpriteBatch;
            this.Texture1 = texture1;
            this.Texture2 = texture2;

            this.Add(GetInstance());
            this[0].Position.Y = GameManager.ScreenHeight - this[0].Height;
            //this[0].Position.Y = GameManager.ScreenBottom + ;

            this.Add(GetInstance());
            this[1].Position.Y = this[0].Position.Y - (this[1].Height + 1);
            //this[1].Position.Y = (this[0].Height + 1);
        }

     /// <summary>
        /// Create a new stick man if it is time
        /// </summary>
        /// <param name="gameTime">The current GameTime of the game</param>
        public void CreateField(GameTime gameTime)
        {
            //Determine the current game time (as a total number of milliseconds)
            Double now = gameTime.TotalGameTime.TotalMilliseconds;

            //Test to see if it is "time" to create a new StickMan
            //and there aren't already too many stickmen...
            if ((this.Count < 2) || (this[this.Count - 1].Position.Y == 0))
            {
                //We need a Random Number generator for few things
                Random rnd = new Random();

                //Ok, the StickMan is ready for battle. Add them to the collection
                this.Add(GetInstance());
            }
        }

        public Field GetInstance()
        {
            //Create a new StickMan instance, pass in the sprite batch and texture
            Field field = new Field(this.SpriteBatch, this.CurrentTexture);

            //Let each StickMan be fatal to the runner if they collide
            field.IsFatal = false;
            field.Damage = 0;
            field.LivesRemaining = 1;

            //Allow 100 points to be added to the score each time a StickMan is killed.
            field.PointValue = 10;

            //Position the tackler just off the top of the screen
            field.Position.Y = -field.Height;

            //Randomly decided the horizontal direction. -1 = left, +1 = right
            //int horizontalDirection = rnd.Next(2) == 1 ? -1 : +1;

            if (_currentTexture == 1) _currentTexture = 2;
            else _currentTexture = 1;

            return field;
        }

        /// <summary>
        /// Draw each StickMan in the collection
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        public void Draw(GameTime gameTime)
        {
            //Draw each of the stickmen
            foreach (Field field in this)
                field.Draw(gameTime);
        }

        public int GetCurrentHeight()
        {
            return this.Count * 500;
        }

        /// <summary>
        /// Update each StickMan in the collection
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        public void Update(GameTime gameTime)
        {
            //Update all of the existing stickmen...
            foreach (Field field in this)
            {
                field.Update(gameTime);
            }

            //Create a new StickMan if it is time..
            CreateField(gameTime);
        }
    }
}
