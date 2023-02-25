using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class CountDownEventArgs
    {
        public int count { get; }
        public CountDownEventArgs(int count)
        {
            this.count = count;
        }
    }
}