using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UIElements;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UI : MonoBehaviour
{
    private UIDocument ui;
    private VisualElement root;
    private List<TimeSpan> times;
    private Label time;

    private delegate void UpdateHandler();
    private UpdateHandler update;
    private Color color;
    private MoveDrone moveDrone;
    private Label energy;
    private AudioSource soundCount;
    private AudioSource soundGo;
    private GameObject machine;
    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        TimeAttack.GetInstance().FinishEvent += OnFinish;
        TimeAttack.GetInstance().LapEvent += OnLap;
        TimeAttack.GetInstance().CountDownEvent += OnCountDown;
        time = root.Q<Label>("time");
        energy = root.Q<Label>("energy");
        times = new List<TimeSpan>();
        moveDrone = GameObject.FindWithTag("MainMachine").GetComponent<MoveDrone>();
        machine = GameObject.FindWithTag("MainMachine");
        var audioSources = GetComponents<AudioSource>();
        foreach(var source in audioSources)
        {
            switch (source.clip.name)
            {
                case "CountDown":
                    soundCount = source;
                    break;
                case "Go":
                    soundGo = source;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        time.text = ElapseTime(TimeAttack.GetInstance().race.GetTime());        
        energy.text = moveDrone.energy.ToString();
        if (update != null)
        {
            update();
        }
        if(rigidBody.velocity.magnitude > 120)
        {

        }
    }

    private string ElapseTime(TimeSpan time)
    {
        return String.Format("{0:00}:{1:00}.{2:00}", time.Minutes, time.Seconds, time.Milliseconds / 10);
    }

    private void OnFinish(object sender, FinishEventArgs e)
    {        
        var goal_text = root.Q<Label>("goal");
        var record = root.Q<Label>("record");
        goal_text.visible = true;
        times.Add(TimeAttack.GetInstance().race.GetTime());
        root.Q<Label>("time").style.color = Color.yellow;

        var save = new Saveable();
        SaveDataManager.LoadJsonData(save);
        SynchronizationContext context = SynchronizationContext.Current;
        if (save.times == null || save.times.Count == 0 || save.times[save.times.Count - 1].TotalMilliseconds > times[times.Count - 1].TotalMilliseconds)
        {
            Debug.Log("New Record!!");
            save.times = times;
            SaveDataManager.SaveJsonData(save);
            new Thread(() =>
            {
                Thread.Sleep(3000);
                context.Post(_ =>
                {
                    goal_text.visible = false;
                    var time = times[times.Count - 1];
                    record.text = $"New Record!\n{time.Minutes}:{time.Seconds}:{time.Milliseconds}";
                    record.visible = true;                    
                }, null);
                Thread.Sleep(1500);
                context.Post(_ =>
                {
                    var str = "";
                    for (int i = 0; i < times.Count; i++)
                    {
                        var time = times[i];
                        if (i != 0)
                        {
                            time -= times[i - 1];
                        }
                        str += String.Format("Lap{0}  {1:00}:{2:00}:{3:000}\n", i + 1, time.Minutes, time.Seconds, time.Milliseconds);
                    }
                    record.text = str;
                    record.visible = true;
                }, null);
            }).Start();
        }
        else
        {
            new Thread(() =>
            {
                Thread.Sleep(3000);
                context.Post(_ =>
                {
                    goal_text.visible = false;                    
                }, null);
                Thread.Sleep(1500);
                context.Post(_ =>
                {
                    var str = "";
                    for (int i = 0; i < times.Count; i++)
                    {
                        var time = times[i];
                        if (i != 0)
                        {
                            time -= times[i - 1];
                        }
                        str += String.Format("Lap{0}  {1:00}:{2:00}:{3:000}\n", i + 1, time.Minutes, time.Seconds, time.Milliseconds);
                    }
                    record.text = str;
                    record.visible = true;
                }, null);
            }).Start();
        }        
    }

    private void OnLap(object sender, LapEventArgs e)
    {
        var laps = root.Q<Label>("laps");
        var lap_num = e.laps + 1;
        laps.text = lap_num.ToString();        
        times.Add(TimeAttack.GetInstance().race.GetTime());
        Thread t = new Thread(new ThreadStart(ChangeColor));
        t.Start();                
    }

    private void ChangeColor()
    {        
        color = Color.yellow;
        update += ChangeTextColor;
        Thread.Sleep(3000);
        color = Color.white;
        update += ChangeTextColor;
    }

    private void ChangeTextColor()
    {
        root.Q<Label>("time").style.color = color;
        update -= ChangeTextColor;
    }

    private void OnCountDown(CountDownEventArgs e)
    {
        var count_down = root.Q<Label>("count_down");
        
        if(e.count == 0)
        {
            count_down.text = "Go";
            count_down.visible = true;
            soundGo.Play();
            SynchronizationContext context = SynchronizationContext.Current;
            new Thread(() =>
            {
                Thread.Sleep(1000);
                context.Post(_ =>
                {
                    OnCountDown(new CountDownEventArgs(-1));
                },null);
            }).Start();
        }
        else if(e.count > 0)
        {
            count_down.text = e.count.ToString();
            count_down.visible = true;
            soundCount.Play();
        }
        else
        {
            count_down.visible = false;
        }
    }
}
