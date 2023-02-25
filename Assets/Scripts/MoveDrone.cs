using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

// アタッチしたGameObjectにRigidbodyがアタッチされていない場合、アタッチする
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class MoveDrone : MonoBehaviour
{

    [SerializeField]
    private float horizontalAccelaration = 8.0f;
    [SerializeField]
    private float verticalAccelaration = 50f;
    [SerializeField]
    private float horizontalSens = 1.0f;
    [SerializeField]
    private float verticalSens = 1.0f;

    private Vector3 _gravity;
    private Vector3 _size;

    public KeyCode turnOver;

    public float gyroPower;
    public bool keyOp;
    public bool isJoyStick;
    public bool manual;
    public float ocAccelaration = 30f;
    private bool freeze = true;
    private AudioSource soundFly;
    private float pitch = 1.75f;
    private float a_pitch = 0.01f;
    public float maxSpeed = 100f;

    public float maxRotate;

    // アタッチしているGameObjectのRigidbodyを格納する変数
    private Rigidbody rigidBody;
    private BoxCollider boxCollider;

    private static Quaternion Zero = Quaternion.Euler(0, 0, 0);
    public float energy { get; set; }
    public float watt { get; set; }
    private float fixedFrameRate;
    private bool boost = false;


    // Start is called before the first frame update
    void Start()
    {
        // アタッチしているGameObjectのRigidbodyを取得
        rigidBody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        boxCollider.center = new Vector3(0, -0.1f, 0);
        boxCollider.size = new Vector3(4, 0.6f, 4);
        rigidBody.drag = 0.45f;
        rigidBody.angularDrag = 0.45f;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        watt = 50f; // per second
        // 重力を取得
        _gravity = Physics.gravity;
        _size = boxCollider.size;
        rigidBody.isKinematic = true;
        fixedFrameRate = 1 / Time.fixedDeltaTime;
        TimeAttack.GetInstance().StartEvent += OnStart;
        Debug.Log(_gravity);
        soundFly = GetComponent<AudioSource>();        
        soundFly.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(turnOver) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            float y = transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, y, 0);
            rigidBody.ResetInertiaTensor();
        }        
    }

    private void FixedUpdate()
    {
        if (freeze) return;
        Auto();
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if(soundFly.pitch < 2.0)
            {
                soundFly.pitch += a_pitch;
            }
        }
        else
        {
            if(soundFly.pitch > 1.75)
            {
                soundFly.pitch -= a_pitch;
            }
        }
    }

    private void Auto()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        float ud = Input.GetAxis("UpDown");
        float roll = Input.GetAxis("Roll");



        float z = transform.eulerAngles.z * Mathf.Deg2Rad;
        float cosZ = Mathf.Cos(Mathf.Abs(z));

        float f = rigidBody.mass * _gravity.y;

        if (Mathf.Abs(z) < 60 * Mathf.Deg2Rad)
        {
            f /= cosZ;
        }

        float x = transform.eulerAngles.x * Mathf.Deg2Rad;
        float cosX = Mathf.Cos(Mathf.Abs(x));

        if (Mathf.Abs(x) < 60 * Mathf.Deg2Rad)
        {
            f /= cosX;
        }

        rigidBody.AddRelativeForce(new Vector3(-1 * rigidBody.mass * verticalAccelaration * h, 0, rigidBody.mass * verticalAccelaration * v));
        rigidBody.AddRelativeForce(new Vector3(0, -1 * f, 0));
        rigidBody.AddRelativeTorque(new Vector3(0, -1 * rigidBody.mass * roll * _size.z / 2 * horizontalAccelaration, 0));

        if (Input.GetKey("joystick button 7") || Input.GetKey(KeyCode.Mouse0))
        {
            var reqWatt = watt / fixedFrameRate;
            if(energy > 0)
            {
                energy = Mathf.Max(energy - reqWatt, 0);
                rigidBody.AddRelativeForce(new Vector3(0, 0, rigidBody.mass * ocAccelaration));                
            }            
        }        

        if (Input.GetKey(KeyCode.E))
        {
            rigidBody.AddRelativeTorque(new Vector3(0, rigidBody.mass * _size.z / 2 * horizontalAccelaration, 0));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rigidBody.AddRelativeTorque(new Vector3(0, -1 * rigidBody.mass * _size.z / 2 * horizontalAccelaration, 0));
        }
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(new Vector3(0, verticalAccelaration, 0));
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            rigidBody.AddRelativeForce(new Vector3(0, -1 * verticalAccelaration, 0));
        }
        AddLift(ud * verticalAccelaration);
    }



    // 揚力を加える
    private void AddLift(Vector3 a)
    {
        rigidBody.AddRelativeForce(rigidBody.mass * a);
    }

    private void AddLift(float a)
    {
        AddLift(new Vector3(0, a, 0));
    }

    private void AddTorque(Vector3 a)
    {
        rigidBody.AddRelativeTorque(rigidBody.mass * a * _size.x / 2);
    }


    private void AddTorque(float a)
    {
        AddTorque(new Vector3(0, a, 0));
    }

    // ホバリングする
    private void Hover()
    {
        AddLift(_gravity * -1);
    }

    private void OnStart()
    {
        freeze = false;
        rigidBody.isKinematic = false;
    }

}
