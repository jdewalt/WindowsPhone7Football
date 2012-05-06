using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace PingDevelopment.Framework
{
    public static class InputManager
    {
        public static Accelerometer Accelerometer;
        public static Vector3 AccelerometerReading;
        private static bool isNewTap = false;

        public static bool IsNewTap
        {
            get { return isNewTap; }
        }

        public static bool IsBackButtonPressed
        {
            get
            {
                return (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed);
            }
        }

        static InputManager()
        {
            Accelerometer = new Accelerometer();
            if (Accelerometer.State == SensorState.Ready)
            {
                Accelerometer.CurrentValueChanged +=
                (o, e) => AccelerometerReading = e.SensorReading.Acceleration;
                Accelerometer.Start();
            }

            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        public static void Update()
        {
            isNewTap = false;
            foreach (TouchLocation touch in TouchPanel.GetState())
                if (touch.State == TouchLocationState.Pressed)
                {
                    isNewTap = true;
                    break;
                }
        }
    }
}
