using UnityEngine;

namespace Assets
{
    public static class GlobalGameState
    {
        private static float food;
        private static int distanceMultiplier = 1;

        //Range: 0 (starved to dead) - 1 (full)
        public static float Food
        {
            get { return 1; }
            set { food = Mathf.Clamp(value, 0, 1); }
        }

        //Distance in meters
        public static float Distance
        {
            get { return (EndXPosition - StartXPosition) * distanceMultiplier; }
        }

        public static float StartXPosition { get; set; }
        public static float EndXPosition { get; set; }
        public static string NextKillName
        {
            get
            {
                if(KillIndex < KillOrder.Length)
                {
                    return KillOrder[KillIndex];
                }
                return string.Empty;
            }
        }

        public static int DogsAlive = 4;

        public static string[] DogNames = { "Happy", "Rudolph", "Pavlov", "Hachiko" };
        public static int KillIndex = 0;
        public static string[] KillOrder = { "Hachiko", "Pavlov", "Rudolph", "Happy" };
    }
}