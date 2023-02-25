
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Test : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            TimeAttack.GetInstance().StartRace("NormalStadium","");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}