using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{   private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
          // Check if any key is currently being pressed
        if (Input.anyKey)
        {
            if (animator != null){
            if (animator.speed != 1){
            animator.speed = 1;
            }
        }
        }
        else
        {   
            if (animator.speed != 0){
            // Pause the animation by setting speed to 0
            animator.speed = 0; }
        
        }


        
    }
}
