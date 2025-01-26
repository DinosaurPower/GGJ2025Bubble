using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallAwakeningLogic : MonoBehaviour
{
    public GameObject babaYaga;
    public int randomNumber;

    void onAwake(){
        randomNumber = Random.Range(0, 1);
        if (randomNumber == 0){babaYaga.SetActive(true);}
    }
}
