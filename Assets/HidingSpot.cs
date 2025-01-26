using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public GameObject hidingText;
    
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
            hidingText.SetActive(true);
        }
        }

        void OnTriggerExit2D (Collider2D other){

              if (other.gameObject.CompareTag("Player")){
            hidingText.SetActive(false);
        }


        }
}
