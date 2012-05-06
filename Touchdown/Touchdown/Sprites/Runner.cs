using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PingDevelopment.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Touchdown.Sprites
{
    public partial class Runner : Sprite
    {
        public Runner(SpriteBatch SpriteBatch, Texture2D Texture)
            : base(SpriteBatch, Texture)
        {
            CenterRunner();
        }

        public void CenterRunner()
        {
            //Center the Robot on the screen along the bottom
            Position.X = MaxOnScreenX / 2;
            Position.Y = MaxOnScreenY;
        }

        public override void Update(GameTime gameTime)
        {
            //To keep the robot from moving vertically, set it's vertical velocity to zero (0)
            Velocity.Y = 0;

            //Set the robots horizontal velocity using the AcccelerometerReading.X value.
            Velocity.X = InputManager.AccelerometerReading.X * 50;

            //Update the position with the Velocity
            Position += Velocity;

            //Make sure it stays on the screen. We'll use the MathHelper.Clamp method help:
            Position.X = MathHelper.Clamp(Position.X, 0, MaxOnScreenX);
        }
    }
}
