using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public Dialogue dialogue;   // Assign lines in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Start the dialogue sequence
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}
