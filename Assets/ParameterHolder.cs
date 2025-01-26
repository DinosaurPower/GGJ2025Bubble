using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterHolder : MonoBehaviour
{

    public bool Hidden = false;
        public bool Key1 = false;
            public bool Key2 = false;
                public bool Key3 = false;

    void Update(){
      
    }

    public void checkKey1(){
        Key1 = true;

    }
     public void checkKey2(){
        Key2 = true;

    }

     public void checkKey3(){
        Key3 = true;

    }
    
    
    
}
