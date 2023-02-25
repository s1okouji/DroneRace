using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;


namespace Assets.Scripts
{
    public class Race
    {
        private string courseName;
        private string machineName;
        private TimeAttack context;

        private int passedGate = -1;
        private int laps = 0;

        public List<int> lapGateNums = new List<int>(){14,14,14};
        private CourseScene courseScene;
        private UIDocument ui;
        private GameObject machineObject;        


        public Stopwatch stopWatch { get; }

        public Race(string courseName, string machineName, TimeAttack context)
        {
            this.courseName = courseName;
            this.machineName = machineName;
            this.context = context;            
            context.PassGateEvent += PassGate;
            context.CourseLoadedEvent += Setup;
            context.FinishEvent += OnFinish;
            stopWatch = new Stopwatch();
        }

        public void Start()
        {
            courseScene = new CourseScene("NormalStadium");
            // TODO 操作を不可能にして、カウントダウンを行う

            SynchronizationContext context = SynchronizationContext.Current;
            new Thread(() =>
            {
                int i = 3;
                while(i >= 0)
                {
                    Thread.Sleep(1000);
                    context.Post(i =>
                    {                        
                        TimeAttack.GetInstance().OnCountDown(new CountDownEventArgs((int) i));
                        UnityEngine.Debug.Log(i);
                        if((int)i == 0)
                        {
                            StartRace();
                        }
                    }, i);
                    i--;
                }
            }).Start();
        }

        private void StartRace()
        {
            // ストップウォッチを作動
            stopWatch.Start();
            // ignite event
            context.OnStart();
        }



        public TimeSpan GetTime()
        {
            return stopWatch.Elapsed;
        }

        private void OnFinish(object sender, FinishEventArgs e)
        {
            stopWatch.Stop();
        }

        // Call after course loaded
        private void Setup()
        {            
            // Set Machine to start position
            var platform = courseScene.GetStartTransform();            
            // Fix to use dynamic machine
            var machine = LoadMachine();
            machineObject = GameObject.Instantiate(machine, platform.position + new Vector3(0, 1, 0), platform.rotation);
            machineObject.AddComponent<MoveDrone>();
            machineObject.tag = "MainMachine";
            machineObject.GetComponent<MoveDrone>().energy = 500;
            ui = GameObject.FindObjectOfType<UIDocument>(); // Assume that UI Document is unique.
        }

        private GameObject LoadMachine()
        {
            var machine = Resources.Load<GameObject>("MyDrone/prefab/drone Black");
            return machine;
        }

        private void PassGate(object sender, PassGateEventArgs e)
        {
            if (laps >= lapGateNums.Count) return;
            // passed right gate
            if (passedGate + 1 == e.gate.gateNum)
            {
                passedGate = e.gate.gateNum;
            }

            if (passedGate == lapGateNums[laps])
            {
                passedGate = -1;
                laps++;
                if (laps == lapGateNums.Count)
                {
                    context.OnFinish(new FinishEventArgs());
                }
                else
                {
                    context.OnLap(new LapEventArgs(laps));
                }
                UnityEngine.Debug.Log("laps: " + laps);
            }                       
        }

        
    }
}