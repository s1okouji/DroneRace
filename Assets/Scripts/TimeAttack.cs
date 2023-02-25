using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{

    // This class is EntryPoint of TimeAttack Game
    public class TimeAttack
    {
        private static TimeAttack instance = null;
        
        public delegate void FinishEventHandler(object sender, FinishEventArgs e);
        public delegate void PassGateEventHandler(object sender, PassGateEventArgs e);
        public delegate void StartEventHandler();
        public delegate void CourseLoadedEventHandler();
        public delegate void LapEventHandler(object sender, LapEventArgs e);
        public delegate void CountDownEventHandler(CountDownEventArgs e);
        public delegate void OverPowerEventHandler(OverPowerEventArgs e);

        public event FinishEventHandler FinishEvent;
        public event PassGateEventHandler PassGateEvent;
        public event StartEventHandler StartEvent;
        public event CourseLoadedEventHandler CourseLoadedEvent;
        public event LapEventHandler LapEvent;
        public event CountDownEventHandler CountDownEvent;
        public event OverPowerEventHandler OverPowerEvent;

        public Race race;

        public TimeAttack()
        {            
        }

        public static TimeAttack GetInstance()
        {
            if(instance == null)
            {
                instance = new TimeAttack();
            }
            return instance;
        }

        public void StartRace(string courseName, string machineName)
        {
            race = new Race(courseName, machineName, GetInstance());
            race.Start();
        }

        public void Quit()
        {
            SceneManager.LoadScene("Title", LoadSceneMode.Single);
            instance = null; // インスタンスの廃棄
        }

        public void Restart()
        {
            instance = null;
            TimeAttack.GetInstance().StartRace("NormalStadium", "");
        }

        public void OnFinish(FinishEventArgs e)
        {
            var handler = FinishEvent;
            if(handler != null)
            {
                handler(this, e);
            }
        }

        public void OnPassGate(PassGateEventArgs e)
        {
            Debug.Log("PassGateEvent");
            var handler = PassGateEvent;
            if(handler != null)
            {
                handler(this, e);
            }
        }

        public void OnStart()
        {
            var handler = StartEvent;
            if(handler != null)
            {
                handler();
            }
        }
        public void OnCourseLoaded()
        {
            var handler = CourseLoadedEvent;
            if (handler != null)
            {
                handler();
            }
        }

        public void OnLap(LapEventArgs e)
        {
            LapEvent.Invoke(this, e);
        }

        public void OnCountDown(CountDownEventArgs e)
        {
            CountDownEvent.Invoke(e);
        }

        public void OnOverPower(OverPowerEventArgs e)
        {
            OverPowerEvent.Invoke(e);
        }
    }
}