using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public static class GlobalGameState
    {

        private static int distanceMultiplier = 10;

        //Range: 0 (starved to dead) - 1 (full)
        public static float Food { get; set; }

        //Distance in meters
        public static float Distance
        {
            get
            {
                return (EndXPosition - StartXPosition) * distanceMultiplier;
            }
        }

        public static float StartXPosition { get; set; }

        public static float EndXPosition { get; set; }
    }
}
