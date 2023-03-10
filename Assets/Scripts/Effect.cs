using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts;
using DG.Tweening;

public class Effect : MonoBehaviour
{

    private GameObject machine;
    private Rigidbody rigidbody;
    private UIDocument ui;
    private VisualElement root;
    private MoveDrone moveDrone;
    private VisualElement effectSyutyu;
    private float currentOpacity;

    // Start is called before the first frame update
    void Start()
    {
        machine = GameObject.FindWithTag("MainMachine");
        moveDrone = machine.GetComponent<MoveDrone>();
        rigidbody = machine.GetComponent<Rigidbody>();
        ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        effectSyutyu = root.Q<VisualElement>("effectSyutyu");
        TimeAttack.GetInstance().BoostStartEvent += OnBoostStart;
        TimeAttack.GetInstance().BoostEndEvent += OnBoostEnd;
    }

    // Update is called once per frame
    void Update()
    {        
    }

    private void OnBoostStart(BoostStartEventArgs e)
    {
        DOTween.To(() => effectSyutyu.resolvedStyle.opacity, x => effectSyutyu.style.opacity = new StyleFloat(x), 1, 2);
    }

    private void OnBoostEnd(BoostEndEventArgs e)
    {
        DOTween.To(() => effectSyutyu.resolvedStyle.opacity, x => effectSyutyu.style.opacity = new StyleFloat(x), 0, 2);
    }
}
