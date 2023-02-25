using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts;
using System.Threading;

public class TitleUI : MonoBehaviour
{

    private AudioSource soundTap;
    // Start is called before the first frame update
    void Start()
    {
        soundTap = GetComponent<AudioSource>();
        var root = GetComponent<UIDocument>().rootVisualElement;
        var back = root.Q<VisualElement>("bg");
        back.Q<Button>("button_quit").RegisterCallback<MouseUpEvent>((evt) => Quit());
        back.Q<Button>("button_start").RegisterCallback<MouseUpEvent>((evt) => OnButtonTAPressed());
    }

    // Update is called once per frame
    void Update()
    {           
    }        

    public void OnButtonTAPressed()
    {
        Debug.Log("Pressed!");

        // Sound
        SynchronizationContext context = SynchronizationContext.Current;
        soundTap.Play();
        var len = soundTap.clip.length * 1000;
        Debug.Log(len);
        new Thread(() =>
        {
            Thread.Sleep((int) len);
            context.Post(_ =>
            {
                TimeAttack.GetInstance().StartRace("NormalStadium", "");
            }, null);
        }).Start();
        
    }

    public void Quit()
    {
        SynchronizationContext context = SynchronizationContext.Current;
        soundTap.Play();
        var len = soundTap.clip.length * 1000;
        new Thread(() =>
        {
            Thread.Sleep((int)len);
            context.Post(_ =>
            {
                Application.Quit();
            },null);
        }).Start();    
    }
}
