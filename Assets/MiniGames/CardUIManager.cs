using System.Collections.Generic;
using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    [Header("References")]
    public BetGame betGame;          // Reference to the main BetGame script
    public GameObject cardUIPrefab;  // The prefab for our CardUI

    [Header("Fan Layout Settings")]
    public float angleStep = 10f;  // Degrees between each card
    public float radius = 200f;    // Radius of the circle arc
    public float verticalFlip = -1f; // Use -1 to arc downward, +1 to arc upward

    // We store the currently active card objects (not yet discarded)
    private List<GameObject> cardUIList = new List<GameObject>();

    /// <summary>
    /// Creates the initial player hand (e.g., 7 cards).
    /// </summary>
    public void CreatePlayerHand(int[] cardValues)
    {
        ClearExistingCards();

        // Instantiate card prefab for each value
        for (int i = 0; i < cardValues.Length; i++)
        {
            GameObject cardObj = Instantiate(cardUIPrefab, transform);
            CardUI cardUI = cardObj.GetComponent<CardUI>();
            if (cardUI != null)
            {
                // Initialize the card with the game reference and its value
                cardUI.InitCard(betGame, cardValues[i]);
            }
            // Add to our list
            cardUIList.Add(cardObj);
        }

        // Now position them in a fan
        RefreshHand();
    }

    /// <summary>
    /// Discards a card visually from the hand, then re-layout the remaining cards.
    /// </summary>
    public void DiscardCard(int cardValue)
    {
        // Find the card object in our list that has the given value
        for (int i = 0; i < cardUIList.Count; i++)
        {
            CardUI cardUI = cardUIList[i].GetComponent<CardUI>();
            if (cardUI != null && cardUI.cardValue == cardValue)
            {
                // Remove from the list
                GameObject cardObj = cardUIList[i];
                cardUIList.RemoveAt(i);

                // Destroy the card GameObject
                Destroy(cardObj);

                // Re-layout remaining cards
                RefreshHand();
                break;
            }
        }
    }

    /// <summary>
    /// Repositions and rotates the active card objects in a fan layout.
    /// Call this any time the hand changes (card added/removed).
    /// </summary>
    private void RefreshHand()
    {
        int cardCount = cardUIList.Count;
        if (cardCount == 0)
            return;

        // Calculate a start angle so the hand is centered around 0
        float totalSpan = (cardCount - 1) * angleStep;
        float startAngle = -totalSpan / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            float angleRad = currentAngle * Mathf.Deg2Rad;

            // Position on a circle (fan arc).
            // verticalFlip = -1 => arcs downward, +1 => arcs upward
            float xPos = -radius * Mathf.Sin(angleRad);
            float yPos = radius * verticalFlip * Mathf.Cos(angleRad);

            // Access the RectTransform of the card
            RectTransform cardRect = cardUIList[i].GetComponent<RectTransform>();
            if (cardRect == null) continue;

            // Position relative to the CardUIManager
            cardRect.anchoredPosition = new Vector2(xPos, yPos);

            // Rotate card
            cardRect.localRotation = Quaternion.Euler(0, 0, currentAngle * verticalFlip);

            // If your card pivot is set to (0.5, 0) (bottom center),
            // the card rotates around its bottom edge, etc. Adjust as desired.
        }
    }

    /// <summary>
    /// Removes all existing card objects (if re-dealing or debugging).
    /// </summary>
    private void ClearExistingCards()
    {
        for (int i = 0; i < cardUIList.Count; i++)
        {
            Destroy(cardUIList[i]);
        }
        cardUIList.Clear();
    }
}
