using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PingDevelopment.Framework
{
    /// <summary>
    /// Stores values and methods used to manage game play
    /// </summary>
    public static class GameManager
    {
        /// <summary>
        /// The current Game Screen of the game.
        /// </summary>
        public static GameScreens GameScreen;

        /// <summary>
        /// The width of the screen
        /// </summary>
        public const int ScreenWidth = 480;

        /// <summary>
        /// The height of the screen
        /// </summary>
        public const int ScreenHeight = 800;

        /// <summary>
        /// The location of the bottom of the screen.  
        /// Should be 800 initially.  
        /// Need to change to 720 when the ad is being shown
        /// </summary>
        public const int ScreenBottom = 800;

        /// <summary>
        /// The current Score in the game. This will increase as the hero (Robot)
        /// kills each enemy (StickMan). The score will increase by StickMan.PointValue
        /// as each StickMan is killed.
        /// </summary>
        public static int Score = 0;

        /// <summary>
        /// The current Level of the game.
        /// </summary>
        public static int Level;

        /// <summary>
        /// How many StickMan sprites can be on the screen at a time.
        /// This can increase as the game levels advance
        /// </summary>
        public static int MaxConcurrentEnemies;

        /// <summary>
        /// The minimum amount of time (in milliseconds) that must pass
        /// before a new StickMan can be created.
        /// This can get faster (smaller) as the game levels advance
        /// </summary>
        public static double MinEnemyCreateRate;

        /// <summary>
        /// The minimum velocity (x & y) for a StickMan
        /// This can increase as the game levels advance
        /// </summary>
        public static int MinEnemyVelocity;

        /// <summary>
        /// The maximum velocity (x & y) for a StickMan
        /// This can increase as the game levels advance
        /// </summary>
        public static int MaxEnemyVelocity;

        /// <summary>
        /// How many Bullet sprites can be on the screen at once.
        /// We may want to start low, but allow more bullets on the scree
        /// as the game advances (gotta give the Robot and advantage sometimes)
        /// </summary>
        public static int MaxConcurrentBullets;

        /// <summary>
        /// The minimum amount of time (in milliseconds) before another Bullet
        /// sprite can be created. We could start this higher (slower) and
        /// allow it to decrease (faster) as the game advances.
        /// </summary>
        public static double MinBulletCreateRate;

        /// <summary>
        /// The Score value that needs to be reached before the game
        /// advances to the next level.
        /// </summary>
        public static double NextLevelScore;


        public static Vector2 GetFieldSpeed()
        {
            return new Vector2(0,-1);

        }

        /// <summary>
        /// Change the level related fields with new level
        /// </summary>
        public static void AdvanceLevel()
        {
            //If are just starting, or if the score is high enough
            if (Level == 0 || Score >= NextLevelScore)
            {
                //Advance to the next level
                Level++;

                //Set the level related values accordingly
                //Allow more StickMen on the screen with each level
                //Start with 3 and add two with each level
                MaxConcurrentEnemies = 3 + (Level * 2);

                //Manage how fast new stickmen get created.
                //Start at 5 seconds (5000 milliseconds)
                //Decrease 1/2 second (500 milliseconds) with each level
                //Don't get any faster than 500 milliseconds.
                MinEnemyCreateRate = MathHelper.Clamp(5500 - (500 * Level), 500, 5000);

                //Manage how SLOW a StickMan can move.
                //Start with a minimum of 1 pixel at a time
                //Increase to a minimum of 5 pixels at a time
                MinEnemyVelocity = (int)MathHelper.Clamp(++MaxEnemyVelocity, 1, 5);

                //Manage how FAST a StickMan can move
                //Start with a max speed of 5 pixels
                //Increase to a max speed of 25 pixels
                MaxEnemyVelocity = (int)MathHelper.Clamp(++MaxEnemyVelocity, 5, 25);

                //Manage how many bullets can be on the screen at a time
                //Start low, but allow it to increase as the game advances
                //Start at 3 and increase by 2 with each level
                MaxConcurrentBullets = 3 + (Level * 2);

                //Manage how fast new bullets can be added (firing rate)
                //Start slow with only 1 bullet every 1/2 second (500 milliseconds)
                //Decrease the time by 1/10th of a second, 100 milliseconds,
                //(getting faster) by 100 milliseconds with each level
                MinBulletCreateRate = MathHelper.Clamp(550 - (50 * Level), 100, 500);

                //Advance levels every 1000 points.
                NextLevelScore += 1000;
            }
        }
    }
}
