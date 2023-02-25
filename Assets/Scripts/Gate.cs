using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

[RequireComponent(typeof(MeshCollider))]
public class Gate : MonoBehaviour
{

    private MeshCollider collider;
    [SerializeField]
    public int gateNum;
    private static Material normalGate;
    private static Material nextGate;
    private MeshRenderer renderer;
    private bool finished;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<MeshCollider>();
        collider.isTrigger = true;
        renderer = GetComponent<MeshRenderer>();
        if(normalGate == null)
        {
            normalGate = Resources.Load<Material>("Material/NormalGate");
        }

        if (nextGate == null)
        {
            nextGate = Resources.Load<Material>("Material/NextGate");
        }
        TimeAttack.GetInstance().PassGateEvent += PassGate;
        TimeAttack.GetInstance().FinishEvent += OnFinish;
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFinish(object sender, FinishEventArgs e)
    {
        finished = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        TimeAttack.GetInstance().OnPassGate(new PassGateEventArgs(this));
    }  

    private void PassGate(object sender, PassGateEventArgs e)
    {
        if (finished) return;
        if(e.gate.gateNum + 1 == gateNum)
        {            
            if(renderer.material.name.StartsWith("NormalGate"))
            {
                renderer.material = nextGate;
            }
        }
        else
        {   
            if(e.gate.renderer.material.name.StartsWith("GoalGate") && gateNum == 0)
            {
                renderer.material = nextGate;
            }
            else if(renderer.material.name.StartsWith("NextGate"))
            {
                renderer.material = normalGate;
            }
        }
    }
}
