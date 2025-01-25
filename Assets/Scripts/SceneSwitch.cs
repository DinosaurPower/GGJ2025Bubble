using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitch : MonoBehaviour
{
    public string DebugMessage;
    public float PlayerPosition;
    public int orderDirection;
    public SceneUpdateHolder sceneUpdateHolder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D (Collider2D other){
        if (other.gameObject.CompareTag("Player")){
        sceneUpdateHolder.updateScenes(orderDirection);
        other.gameObject.transform.position = new Vector3(PlayerPosition, other.gameObject.transform.position.y, other.gameObject.transform.position.z);


    }
    }
}
