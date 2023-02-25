using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SoundPlayer
    {
        private static SoundPlayer instance = null;
        private GameObject player;
        private SoundController controller;

        public static SoundPlayer GetInstance()
        {
            if(instance == null)
            {
                instance = new SoundPlayer();
            }
            return instance;
        }

        private SoundPlayer()
        {
            player = new GameObject();
            player.name = "SoundPlayer";
            player.AddComponent<AudioSource>();            
            controller = player.AddComponent<SoundController>();
            Object.DontDestroyOnLoad(player);
            SceneManager.sceneLoaded += MoveScene;
        }

        public void Play(AudioSource audioSource)
        {            
            controller.Play(audioSource);            
        }

        public void MoveScene(Scene scene, LoadSceneMode mode)
        {            
            SceneManager.MoveGameObjectToScene(player, scene);
            Object.DontDestroyOnLoad(player);
        }

    }
}