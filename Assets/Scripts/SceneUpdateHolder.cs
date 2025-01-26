using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUpdateHolder : MonoBehaviour
{
    public GameObject[] scenes;
    public GameObject[] canvasProps;
    public int sceneN;
    public void updateScenes (int Order){
        scenes[sceneN].gameObject.SetActive(false); 
        canvasProps[sceneN].gameObject.SetActive(false);
        sceneN+= Order;
        if (sceneN >2){sceneN = 0;}
        if (sceneN <0){sceneN = 2;}
        scenes[sceneN].gameObject.SetActive(true);
        canvasProps[sceneN].gameObject.SetActive(true);
        
    }
}
