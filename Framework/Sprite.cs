using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingDevelopment.Framework
{
    public class Sprite
    {
        protected SpriteBatch spriteBatch;
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Velocity;

        public Sprite(SpriteBatch SpriteBatch, Texture2D Texture)
        {
            this.spriteBatch = SpriteBatch;
            this.Texture = Texture;
            this.Position = Vector2.Zero;
            this.Velocity = Vector2.Zero;
        }

        public int Width
        {
            get { return Texture.Width; }
        }

        public int Height
        {
            get { return Texture.Height; }
        }

        public int MaxOnScreenX
        {
            get
            {
                //Notice the dependency on the GameManager.ScreenWidth
                return GameManager.ScreenWidth - Width;
            }
        }

        /// <summary>
        /// The number of lives this sprite has remaining.
        /// Set this to the total possible number of lives
        /// when you create the sprite instance.
        /// </summary>
        public int LivesRemaining;

        /// <summary>
        /// The highest possible health value for the sprite.
        /// This will be used when the sprite is first created,
        /// as well as to reset the health value if a new life is used.
        /// </summary>
        public int HealthMax;

        /// <summary>
        /// The current health of the sprite. This will be decremented
        /// by the "damage" value of another sprite when they collide
        /// </summary>
        public int HealthCurrent;

        /// <summary>
        /// Whether or not this sprite is fatal to other sprites.
        /// If this sprite is fatal (true) it automatically takes
        /// a life from the sprite it collides with
        /// </summary>
        public bool IsFatal;

        /// <summary>
        /// If this sprite is NOT fatal, it will cause the this much
        /// damage to the sprite it collides with
        /// </summary>
        public int Damage;

        /// <summary>
        /// If this sprite is "killed" by another sprite, the other
        /// sprite should be awarded this many points
        /// </summary>
        public int PointValue;

        /// <summary>
        /// A string value that you can use for your own purposes to "tag"
        /// this sprite.
        /// </summary>
        public string Tag;

        /// <summary>
        /// A simple property to test if this sprite is alive (LivesRemaining > 0)
        /// </summary>
        public bool IsAlive
        {
            get { return (LivesRemaining > 0); }
        }

        public int MaxOnScreenY
        {
            get
            {
                //Notice the dependency on the GameManager.ScreenBottom
                return GameManager.ScreenBottom - Height;
            }
        }

        /// <summary>
        /// The current rectangle of the sprite based on its position, width and height
        /// </summary>
        public Rectangle CurrentRectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Width, Height); }
        }

        public virtual void Draw(GameTime gameTime)
        {
            //Draw the sprite texture, and it's current position with no color tinting.
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        /// <summary>
        /// Call this method to indicate that this sprite has collided with (hit) another sprite.
        /// Updates the health and lives remaining of this sprite based on the
        /// IsFatal and Damage values of the other sprite
        /// </summary>
        /// <param name="OtherSprite">The other Sprite class (or derivative) that this
        /// sprite collided with</param>
        public void RecordHit(Sprite OtherSprite)
        {
            //Use the following field during this method to see if the sprite "died" as a
            //result of this hit.
            //We'll use this at the end of the method to decrement the LivesRemaining of
            //the sprite if it is true.
            bool died = false;
            //If the other sprite is fatal, it will kill this sprite on a hit.
            if (OtherSprite.IsFatal)
            {
                //Set this sprites lives remaining to -1, indicating that it is no longer active.
                died = true;
            }
            else
            {
                //Subtract the other sprite's damage value from this sprite's health.
                HealthCurrent -= OtherSprite.Damage;
                //If the new health of this sprite is less than zero, kill it
                died = (HealthCurrent <= 0);
            }
            //If the sprite died because of this hit
            if (died)
            {
                //Decrement the LivesRemaining count
                LivesRemaining -= 1;
                //Reset the health value to the max if there are still lives remaining,
                //or to zero if no more lives.
                HealthCurrent = (LivesRemaining > 0) ? HealthMax : 0;
            }
        }
        
        public virtual void Update(GameTime gameTime)
        {
            //Simply update the sprite's position by adding it's velocity
            this.Position += this.Velocity;
        }
    }
}
