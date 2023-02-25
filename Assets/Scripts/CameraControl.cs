using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class CameraControl : MonoBehaviour
{

    public Transform drone;
    public Vector3 pos;
    public Transform parent;
    private Rigidbody droneRigid;
    private Vector3 look;
    private bool Lock;

    // Start is called before the first frame update
    void Start()
    {        
        drone = GameObject.FindWithTag("MainMachine").transform;
        Debug.Log(drone.position);
        parent.position = drone.TransformPoint(pos);
        parent.rotation = Quaternion.Euler(0, drone.eulerAngles.y, drone.eulerAngles.z);
        droneRigid = drone.GetComponent<Rigidbody>();
        Lock = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Lock)
        {
            return;
        }
        parent.position = drone.TransformPoint(pos);        
        float y = drone.eulerAngles.y;        
        parent.rotation = Quaternion.Euler(0, y, 0);
    }    
}
