using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introSequence : MonoBehaviour
{
    public Dialogue introDialogue;
    public DialogueManager dialogueManager;
    // Start is called before the first frame update
    void Start()
    {
       dialogueManager.StartDialogue(introDialogue); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
