using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(AudioSource audioSource)
    {
        Debug.Log("Start Play. Clip name: " + audioSource.clip.name);        
        audioSource.Play();               
    }
}
