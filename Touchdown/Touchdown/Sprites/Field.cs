using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using PingDevelopment.Framework;

namespace Touchdown.Sprites
{
    public partial class Field : Sprite
    {
        public Field(SpriteBatch SpriteBatch, Texture2D Texture)
            : base(SpriteBatch, Texture)
        {
            // Always move the field downward by 1
            this.Velocity = new Vector2(0f, 1f);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //This methods manages the StickMan's position on Screen.  Stick men
            //bounce off the left and right edges of the screen, but wrap 
            //back to the top when they go off the bottom

            //Update their position based on their velocity
            Position += Velocity;

            //Wrap from the bottom to the top
            if (this.IsAlive && (Position.Y == (GameManager.ScreenBottom + Height)))
            {
                GameManager.Score += this.PointValue;
                this.LivesRemaining--;
                //this.IsAlive = false;
            }
        }
    }
}
