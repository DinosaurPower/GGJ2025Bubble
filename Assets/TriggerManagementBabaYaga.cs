using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManagementBabaYaga : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){

        if (other.gameObject.CompareTag("Wall")){


        gameObject.SetActive(false);

    }
        
    }
}
