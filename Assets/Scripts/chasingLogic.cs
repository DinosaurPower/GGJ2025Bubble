using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chasingLogic : MonoBehaviour
{
   
        public GameObject player; // Reference to the player GameObject
        public float distance;
    public float speed = 2.0f; // Speed at which the enemy moves

    private bool movingToPositiveX = true; // Track direction of movement

void Start (){

    if (player == null){ 

            player = GameObject.FindWithTag("Player");
        }

}

    void Update()
    {
        // Check if the player is hidden
        if (!player.GetComponent<ParameterHolder>().Hidden)
        {
            // Player is not hidden, chase the player
            Vector3 position = transform.position;
            position.x = Mathf.MoveTowards(position.x, player.transform.position.x, speed * Time.deltaTime);
            transform.position = position;
        }
        else
        {
            // Player is hidden, move to X = 7 or X = -7
            float targetX = movingToPositiveX ? distance : -distance;
            Vector3 position = transform.position;
            position.x = Mathf.MoveTowards(position.x, targetX, speed * Time.deltaTime);
            transform.position = position;

            // Switch direction if target is reached
            if (Mathf.Abs(transform.position.x - targetX) < 0.01f)
            {
                movingToPositiveX = !movingToPositiveX;
            }
        }
    } 

}
