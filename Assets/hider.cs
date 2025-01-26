using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hider : MonoBehaviour
{
    public GameObject playerCharacter;
    // Start is called before the first frame update
    void Start()
    {
        if (playerCharacter == null){ 

            playerCharacter = GameObject.FindWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if 'H' key is pressed
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Hide");
            // Hide the sprite
            playerCharacter.GetComponent<SpriteRenderer>().enabled = false;
            playerCharacter.GetComponent<ParameterHolder>().Hidden = true;
        }
        else if (Input.anyKeyDown)
        {
            // If any other key is pressed, show the sprite
            playerCharacter.GetComponent<SpriteRenderer>().enabled = true;
             Debug.Log("show");
            playerCharacter.GetComponent<ParameterHolder>().Hidden = false;
        }
    }
}
