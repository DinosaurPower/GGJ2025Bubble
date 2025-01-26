using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUpdateHolder : MonoBehaviour
{
    public GameObject[] scenes;
    public int sceneN;
    public void updateScenes (int Order){
        scenes[sceneN].gameObject.SetActive(false); 
        sceneN+= Order;
        if (sceneN >3){sceneN = 0;}
        if (sceneN <0){sceneN = 3;}
        scenes[sceneN].gameObject.SetActive(true);
    }
}
