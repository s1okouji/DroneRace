using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class CourseScene
    {
        private string courseName;
        private GameObject courseObject;
        private GameObject platformObject;


        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.name.Equals(courseName))
            {
                courseObject = GameObject.FindWithTag("Course");
                platformObject = GameObject.FindWithTag("Platform");
                Debug.Log(courseObject.name);
                Debug.Log(platformObject.name);
                TimeAttack.GetInstance().OnCourseLoaded();
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        public CourseScene(string courseName)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            this.courseName = courseName;
            LoadScene();
            /*UnLoadScene();*/
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(courseName, LoadSceneMode.Single);
        }

        private void UnLoadScene()
        {
            SceneManager.UnloadScene(0);
        }

        public Transform GetStartTransform()
        {
            return platformObject.transform;
        }
    }
}