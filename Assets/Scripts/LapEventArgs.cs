using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class LapEventArgs
    {
        public int laps;
        public LapEventArgs(int laps)
        {
            this.laps = laps;
        }
    }
}