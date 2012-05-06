using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using PingDevelopment.Framework;

namespace Touchdown.Collections
{
    class Blocker : Sprite
    {
         public bool Blocked { get; set; }

        public Blocker(SpriteBatch SpriteBatch, Texture2D Texture)
            : base(SpriteBatch, Texture)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //This methods manages the StickMan's position on Screen.  Stick men
            //bounce off the left and right edges of the screen, but wrap 
            //back to the top when they go off the bottom

            //Update their position based on their velocity
            Position -= Velocity;

            //Bounce off the left and right sides
            if (Position.X <= 0)
            {
                Position.X = 0;
                Velocity.X = Math.Abs(Velocity.X);
            }
            else if (Position.X >= MaxOnScreenX)
            {
                Position.X = MaxOnScreenX;
                Velocity.X = -Math.Abs(Velocity.X);
            }

            //Wrap from the bottom to the top
            if (this.IsAlive && (Position.Y >= (0 + Height)))
            {
                
                // Update the score
               // GameManager.Score += this.PointValue;
                //this.IsAlive = false;
            }
        }
    }
}
