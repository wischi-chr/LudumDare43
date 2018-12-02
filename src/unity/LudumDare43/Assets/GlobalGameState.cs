using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public static class GlobalGameState
    {
        //Range: 0 (starved to dead) - 1 (full)
        public static float Food { get; set; }

        //Distance in meters
        public static float Distance { get; set; }
    }
}
