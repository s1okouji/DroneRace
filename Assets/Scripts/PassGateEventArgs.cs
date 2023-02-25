using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class PassGateEventArgs
    {
        public Gate gate { get; }
        public PassGateEventArgs(Gate gate)
        {
            this.gate = gate;
        }
    }
}