using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingDevelopment.Framework
{
    public class SpriteSheet : Sprite
    {
        //Sprite sheet specific fields

        /// <summary>
        /// The number of rows in the sprite sheet.
        /// </summary>
        /// <remarks>
        /// To keep your sprite sheets simple, 
        /// you may want to think about having a single row of frames.
        /// </remarks>
        public int Rows;

        /// <summary>
        /// The number of columns in the sprite sheet.
        /// </summary>
        /// <remarks>
        /// If you only had a single row of frames, 
        /// the Cols value would be the same as the total number of frames.
        /// </remarks>
        public int Cols;

        /// <summary>
        /// The current row (starting at zero) of the sprite sheet.  Defaults to zero.
        /// </summary>
        public int CurrentRow;

        /// <summary>
        /// The current col (starting at zero) of the sprite sheet.  Defaults to zero.
        /// </summary>
        public int CurrentCol;

        /// <summary>
        /// The width of each frame (pixels) in the SpriteSheet texture.  
        /// This SpriteSheet implementation requires that each frame have the same dimensions (WxH).
        /// </summary>
        public int FrameWidth;

        /// <summary>
        /// The height of each frame (pixels) in the SpriteSheet texture.  
        /// This SpriteSheet implementation requires that each frame have the same dimensions (WxH).
        /// </summary>
        public int FrameHeight;

        /// <summary>
        /// The Rectangle for the current frame.    
        /// </summary>
        /// <remarks>
        /// The location and size of the CurrentFrameRectangle is defined by the CurrentCol, CurrentRow, 
        /// FrameWidth FrameHeight properties. This will be used in the call to SpriteBatch.Draw to draw 
        /// only the portion of the texture identified by this rectangle.
        /// </remarks>
        public Rectangle CurrentFrameRectangle
        {
            get
            {
                return new Rectangle(CurrentCol * FrameWidth, CurrentRow * FrameHeight, FrameWidth, FrameHeight);
            }
        }

        /// <summary>
        /// How long (in milliseconds) should each frame be shown.  
        /// </summary>
        public double FrameTime;

        /// <summary>
        /// The last time the frame was advanced.
        /// </summary>
        public double LastFrameTime;

        /// <summary>
        /// How many times should the sprite sheet animation play.  &lt;=0 - Repeats Infinitely.  1 - plays once, 2 - plays twice, etc.
        /// </summary>
        public int PlayCountMaximum;

        /// <summary>
        /// The number of times the sprite sheet has played.  Defaults to zero.
        /// </summary>
        public int PlayCountCurrent;

        /// <summary>
        /// Initializes a new SpriteSheet instance using the values supplied.
        /// </summary>
        /// <param name="SpriteBatch">The SpriteBatch that will be draw to.</param>
        /// <param name="Texture">The Texture2D that contains the entire spritesheet.</param>
        /// <param name="Rows">The number of rows of frames in the sprite sheet.  To keep things simple, let your sprite sheets have a single row.</param>
        /// <param name="Cols">The number of columns of frames in the sprite sheet. If you only have a single row, there may be a large number of columns for sprite sheet with a lot of frames.</param>
        /// <param name="FrameWidth">The width (in pixels) of each individual frame.  This implementation of SpriteSheet requires that all frames have the same dimensions (WxH).</param>
        /// <param name="FrameHeight">The height (in pixels) of each individual frame.  This implementation of SpriteSheet requires that all frames have the same dimensions (WxH).</param>
        /// <param name="FrameTime">How long the class should wait (in milliseconds) before advancing to the next frame. </param>
        public SpriteSheet(SpriteBatch SpriteBatch, Texture2D Texture, int Rows, int Cols, int FrameWidth, int FrameHeight, double FrameTime)
            : base(SpriteBatch, Texture)
        {
            //Initialize the fields with the arguments supplied
            this.Rows = Rows;
            this.Cols = Cols;
            this.FrameWidth = FrameWidth;
            this.FrameHeight = FrameHeight;
            this.FrameTime = FrameTime;
        }


        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //Call the Advance Frame method.  
            AdvanceFrame(gameTime);
            //Still call the base.Update method to do the regular movement handling of the base class
            base.Update(gameTime);
        }

        private void AdvanceFrame(GameTime gameTime)
        {
            //This logic assumes a left right, top bottom read order from the frames in the sprite sheet.
            //In other words, we will read left to right accross the frames in the top row first, then move down to the next row, etc 
            //repeating until all frames in all rows have been read
            //Once all frames have been read, we will test to see if we can loop based on the CurrentPlayCount and MaximumPlayCount values

            //First check to see if this is the first time a frame has been advanced. If so, we need to start with the first frame, 
            //CurrentCol = 0, CurrentRow = 0
            //To check if this is the first time the method has been called, we can investigate the LastFrameTime field.  If it is 0 (zero),
            //then we now the value has not been set by a previous call to the method.  So this must be the first time this method has been
            //called.


            //Test to see if the LastFrameTime is zero.  If so, we know this is the first time this method has been called.  
            if (LastFrameTime == 0)
            {
                //Record the current time (total milliseconds) in the LastFrameTime field
                LastFrameTime = gameTime.TotalGameTime.TotalMilliseconds;
                //Indicate that this is the first (0, zero) time the series of frames has been played
                PlayCountCurrent = 0;
                //Set the current column to the first column (0, zero)
                CurrentCol = 0;
                //Set the current row to the first row (0, zero) 
                CurrentRow = 0;
            }
            //Otherwise, if the LastFrameTime != 0, we need to advance to the next frame (if it is time)
            else
            {
                //Check to see if it is time to advance the frame....
                if (gameTime.TotalGameTime.TotalMilliseconds - LastFrameTime >= FrameTime)
                {

                    //Set the last frame time...
                    LastFrameTime = gameTime.TotalGameTime.TotalMilliseconds;

                    //if it is, first increment the col.
                    CurrentCol++;
                    //Check to see if we have gone past the last column...
                    if (CurrentCol >= Cols)
                    {
                        //If we have, reset the current column back to zero
                        CurrentCol = 0;
                        //Then increment to the next row
                        CurrentRow++;
                        //Check to see if we have gone past the last row...
                        if (CurrentRow >= Rows)
                        {
                            //If we have, then we have played the animation loop.  In crement the play count to indicate that.
                            PlayCountCurrent++;
                            //Test to see if we can loop again, or if we need to mark the animation as inactive....
                            //If we haven't been told we can repeat for ever (MaximumPlayCount==0) and 
                            //the CurrentPlayCount >= MaximumPlayCount, then we need to kill the sprite
                            if (PlayCountMaximum != 0 && PlayCountCurrent >= PlayCountMaximum)
                                //Set its LivesRemaining to zero to kill it....
                                LivesRemaining = 0;
                            //Otherwise, we will be able to loop again!
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Overrides the base Draw method to draw just the portion of the SpriteSheet indicated by the SourceRectangle property
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            //Only draw if we have configured the source rectangle to the proper frame. 
            //We can test this by checking to see if the LastFrameTime is not zero.
            if (LastFrameTime != 0)
                spriteBatch.Draw(Texture, Position, CurrentFrameRectangle, Color.White);
        }

    }
}
