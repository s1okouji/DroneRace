using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts;

public class PauseUI : MonoBehaviour
{

    private VisualElement root;
    private bool isDisplay = false;
    private VisualElement container;
    private VisualElement button_container;
    private Button button_quit;
    private Button button_continue;
    private Button button_restart;

    private AudioSource tapSound;

    // Start is called before the first frame update
    void Start()
    {
        var ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        container = root.Q<VisualElement>("container");
        button_container = container.Q<VisualElement>("button_container");
        button_quit = button_container.Q <Button> ("quit");
        button_continue = button_container.Q<Button>("continue");
        button_restart = button_container.Q<Button>("restart");
        tapSound = GetComponent<AudioSource>();


        button_quit.RegisterCallback<MouseUpEvent>((evt) =>
        {
            Quit();
        });
        button_continue.RegisterCallback<MouseUpEvent>((evt) => Continue());
        button_restart.RegisterCallback<MouseUpEvent>((evt) => Restart());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Display(!isDisplay);
            isDisplay = !isDisplay;
        }
    }
    
    private void Display(bool display)
    {        
        container.visible = display;        
        button_container.visible = display;
    }

    private void Quit()
    {
        tapSound.Play();
        var len = tapSound.clip.length * 1000;
        SynchronizationContext context = SynchronizationContext.Current;
        new Thread(() =>
        {
            Thread.Sleep((int)len);
            context.Post(_ => TimeAttack.GetInstance().Quit(), null);
        }).Start();        
    }

    private void Continue()
    {
        tapSound.Play();
        // TODO バックグラウンドでの処理を停止する。
        Display(false);
        isDisplay = false;
    }

    private void Restart()
    {
        tapSound.Play();
        var len = tapSound.clip.length * 1000;
        SynchronizationContext context = SynchronizationContext.Current;
        new Thread(() =>
        {
            Thread.Sleep((int)len);
            context.Post(_ => TimeAttack.GetInstance().Restart(), null);
        }).Start();        
    }    
}