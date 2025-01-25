using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Only if using TextMeshPro

[System.Serializable]
public class Dialogue
{
    [TextArea(3, 5)]
    public string[] lines; // Each element is one line of dialogue
}

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        // If there?s already an instance and it?s not this, destroy this GameObject.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, make this the single instance and optionally persist across scenes.
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [Header("UI References")]
    // Assign your Panel (the background for the dialogue) in the Inspector
    public GameObject dialoguePanel;

    // If using TextMeshPro:
    public TextMeshProUGUI dialogueText;

    // Or, if you're using the old Text system, comment out the above and uncomment this:
    // public Text dialogueText;

    [Header("Typing Settings")]
    [Tooltip("Time in seconds between displaying each character.")]
    public float typingSpeed = 0.05f;

    [Tooltip("Key used to go to the next line or skip typing.")]
    public KeyCode proceedKey = KeyCode.Space;

    // Internal state
    private string[] currentDialogueLines;
    private int currentLineIndex;

    private Coroutine typingCoroutine;
    private bool isTyping;  // True if we're in the middle of typing a line

    /// <summary>
    /// Call this method to begin showing multiple lines from a Dialogue object.
    /// </summary>
    public void StartDialogue(Dialogue dialogue)
    {
        // Show the dialogue panel (make sure it's active)
        dialoguePanel.SetActive(true);

        // Reset line index and store lines
        currentLineIndex = 0;
        currentDialogueLines = dialogue.lines;

        // Start with the first line
        DisplayNextLine();
    }

    private void Update()
    {
        // If the dialogue panel is active, and the user presses the proceed key...
        if (dialoguePanel.activeSelf && Input.GetKeyDown(proceedKey))
        {
            // If we are still typing out the current line, skip to the end
            if (isTyping)
            {
                SkipTyping();
            }
            else
            {
                // Proceed to the next line if typing is complete
                DisplayNextLine();
            }
        }
    }

    /// <summary>
    /// Displays the next line in the array. If we are out of lines, it hides the dialogue.
    /// </summary>
    private void DisplayNextLine()
    {
        if (currentDialogueLines == null)
        {
            Debug.LogWarning("No dialogue loaded. Make sure to call StartDialogue first.");
            return;
        }

        // Check if we have displayed all lines
        if (currentLineIndex >= currentDialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        // If a typing coroutine is already running, stop it
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Start typing the next line
        typingCoroutine = StartCoroutine(TypeLine(currentDialogueLines[currentLineIndex]));
        currentLineIndex++;
    }

    /// <summary>
    /// Coroutine to type out a single line character by character.
    /// </summary>
    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear any previous text

        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Finished typing the line
        isTyping = false;
    }

    /// <summary>
    /// Instantly finish typing the current line (skip the character-by-character effect).
    /// </summary>
    private void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Fill the current line instantly
        string fullLine = currentDialogueLines[currentLineIndex - 1];
        // currentLineIndex was already incremented in DisplayNextLine()

        dialogueText.text = fullLine;
        isTyping = false;
    }

    /// <summary>
    /// Called when we run out of lines, or want to close the dialogue manually.
    /// </summary>
    private void EndDialogue()
    {
        // Hide the panel and clear data
        dialoguePanel.SetActive(false);
        currentDialogueLines = null;
        currentLineIndex = 0;
    }

    /// <summary>
    /// Manually hide the dialogue if needed (public).
    /// </summary>
    public void HideDialogue()
    {
        EndDialogue();
    }
}
