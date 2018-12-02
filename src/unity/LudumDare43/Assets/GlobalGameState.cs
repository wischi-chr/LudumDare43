using UnityEngine;

namespace Assets
{
    public static class GlobalGameState
    {
        private static float food;
        private static int distanceMultiplier = 10;

        //Range: 0 (starved to dead) - 1 (full)
        public static float Food
        {
            get { return food; }
            set { food = Mathf.Clamp(value, 0, 1); }
        }

        //Distance in meters
        public static float Distance
        {
            get { return (EndXPosition - StartXPosition) * distanceMultiplier; }
        }

        public static float StartXPosition { get; set; }
        public static float EndXPosition { get; set; }
    }
}