using UnityEngine;
using System.Collections.Generic;

public class BubbleManager : MonoBehaviour
{
    [Header("Assign your scene bubbles here")]
    [SerializeField]
    private List<GameObject> bubbleGameObjects;

    // We’ll store the original positions and scales of each bubble at runtime.
    private Vector3[] initialPositions;
    private Vector3[] initialScales;

    private void Awake()
    {
        // Prepare arrays to hold initial transforms
        int count = bubbleGameObjects.Count;
        initialPositions = new Vector3[count];
        initialScales = new Vector3[count];

        // Record each bubble's position and scale
        for (int i = 0; i < count; i++)
        {
            if (bubbleGameObjects[i] != null)
            {
                initialPositions[i] = bubbleGameObjects[i].transform.position;
                initialScales[i] = bubbleGameObjects[i].transform.localScale;
            }
        }
    }

    /// <summary>
    /// Enables (summons) all bubbles at their initial positions/scales.
    /// </summary>
    public void SummonAllBubbles()
    {
        for (int i = 0; i < bubbleGameObjects.Count; i++)
        {
            var bubble = bubbleGameObjects[i];
            if (bubble != null)
            {
                bubble.SetActive(true);
                bubble.transform.position = initialPositions[i];
                bubble.transform.localScale = initialScales[i];
            }
        }
    }

    /// <summary>
    /// Summons a single bubble by index in the list, restoring original transform.
    /// </summary>
    public void SummonBubble(int index)
    {
        if (index < 0 || index >= bubbleGameObjects.Count) return;

        var bubble = bubbleGameObjects[index];
        if (bubble != null)
        {
            bubble.SetActive(true);
            bubble.transform.position = initialPositions[index];
            bubble.transform.localScale = initialScales[index];
        }
    }

    /// <summary>
    /// Disables (hides) all bubbles in the list.
    /// </summary>
    public void DisableAllBubbles()
    {
        foreach (var bubble in bubbleGameObjects)
        {
            if (bubble != null)
            {
                bubble.SetActive(false);
            }
        }
    }
}
