using UnityEngine;

namespace Assets
{
    public static class GlobalGameState
    {
        private static float food;

        //Range: 0 (starved to dead) - 1 (full)
        public static float Food
        {
            get { return food; }
            set
            {
                food = Mathf.Clamp(value, 0, 1);
                Debug.Log("New food: " + value);
            }
        }

        //Distance in meters
        public static float Distance { get; set; }
    }
}
