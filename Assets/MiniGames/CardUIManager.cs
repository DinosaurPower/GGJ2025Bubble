using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIManager : MonoBehaviour
{
    [Header("References")]
    public GameObject cardUIPrefab;
    public BetGame betGame; // if needed for referencing BetGame
    public GameObject EnemyCardObj;
    public GameObject ScoreCardObj;

    [Header("Fan Layout Settings")]
    public float angleStep = 10f;
    public float radius = 200f;
    public float verticalFlip = -1f;


    // Store all card objects (player's hand)
    private List<GameObject> cardUIList = new List<GameObject>();

    /// <summary>
    /// Create initial hand
    /// </summary>
    public void CreatePlayerHand(int[] cardValues)
    {
        ClearExistingCards();

        for (int i = 0; i < cardValues.Length; i++)
        {
            GameObject cardObj = Instantiate(cardUIPrefab, transform);
            CardUI c = cardObj.GetComponent<CardUI>();
            if (c != null)
            {
                c.InitCard(betGame, cardValues[i]);
            }
            cardUIList.Add(cardObj);
        }

        RefreshHand();
    }

    /// <summary>
    /// Immediately discards a card (no animation).
    /// </summary>
    public void DiscardCardImmediate(int cardValue)
    {
        for (int i = 0; i < cardUIList.Count; i++)
        {
            CardUI c = cardUIList[i].GetComponent<CardUI>();
            if (c != null && c.cardValue == cardValue)
            {
                Destroy(cardUIList[i]);
                cardUIList.RemoveAt(i);
                RefreshHand();
                return;
            }
        }
    }

    // ================
    //   COROUTINES
    // ================

    /// <summary>
    /// Moves a card from its current position to a target anchored position in 'duration' seconds.
    /// This DOES NOT discard the card. Useful if you want the card to remain, or later fade to black, etc.
    /// </summary>
    public IEnumerator AnimateCardToPosition(int cardValue, Vector2 targetPos, float duration)
    {
        GameObject selectedCard = FindCardByValue(cardValue);
        if (selectedCard == null) yield break; // not found

        RectTransform cardRect = selectedCard.GetComponent<RectTransform>();
        if (cardRect == null) yield break;

        Vector2 startPos = cardRect.anchoredPosition;
        float elapsed = 0f;
        cardRect.rotation = new Quaternion(0, 0, 0, 0);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cardRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            yield return null;
        }

        // Ensure final position
        cardRect.anchoredPosition = targetPos;
    }

    public IEnumerator AnimateObjectToPosition(GameObject MoveObject, Vector2 targetPos, float duration)
    {
        GameObject selectedCard = MoveObject;
        if (selectedCard == null) yield break; // not found

        RectTransform cardRect = selectedCard.GetComponent<RectTransform>();
        if (cardRect == null) yield break;

        Vector2 startPos = cardRect.anchoredPosition;
        float elapsed = 0f;
        cardRect.rotation = new Quaternion(0, 0, 0, 0);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cardRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            yield return null;
        }

        // Ensure final position
        cardRect.anchoredPosition = targetPos;
    }

    /// <summary>
    /// Fades a card's visual color to black over 'duration' seconds.
    /// If your card has multiple images/text, you might need to fade them as well.
    /// </summary>
    public IEnumerator FadeCardToBlack(int cardValue, float duration)
    {
        GameObject selectedCard = FindCardByValue(cardValue);
        if (selectedCard == null) yield break; // not found

        // Example: fade the first Image component. 
        // If you have multiple images or text components, you can fade them all in a loop.
        Image cardImage = selectedCard.GetComponentInChildren<Image>();
        if (cardImage == null) yield break;

        Color startColor = cardImage.color;
        Color endColor = Color.black;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cardImage.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }

        cardImage.color = endColor;
    }

    public IEnumerator FadeEnemyCardToBlack(float duration)
    {
        GameObject selectedCard = EnemyCardObj;
        if (selectedCard == null) yield break; // not found

        // Example: fade the first Image component. 
        // If you have multiple images or text components, you can fade them all in a loop.
        Image cardImage = selectedCard.GetComponentInChildren<Image>();
        if (cardImage == null) yield break;

        Color startColor = cardImage.color;
        Color endColor = Color.black;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cardImage.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }

        cardImage.color = endColor;
    }

    public void returnEnemyCardToWhite()
    {
        EnemyCardObj.GetComponentInChildren<Image>().color = Color.white;
    }

    // ================
    //   LAYOUT
    // ================

    /// <summary>
    /// Repositions and rotates the remaining cards in a fan layout.
    /// </summary>
    private void RefreshHand()
    {
        int cardCount = cardUIList.Count;
        if (cardCount == 0) return;

        float totalSpan = (cardCount - 1) * angleStep;
        float startAngle = -totalSpan / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            float angleRad = currentAngle * Mathf.Deg2Rad;

            RectTransform rect = cardUIList[i].GetComponent<RectTransform>();
            if (rect == null) continue;

            float xPos = -radius * Mathf.Sin(angleRad);
            float yPos = radius * verticalFlip * Mathf.Cos(angleRad);

            rect.anchoredPosition = new Vector2(xPos, yPos);
            rect.localRotation = Quaternion.Euler(0, 0, currentAngle * verticalFlip);
        }
    }

    /// <summary>
    /// Utility to destroy all current cards (e.g., when re-dealing).
    /// </summary>
    private void ClearExistingCards()
    {
        for (int i = 0; i < cardUIList.Count; i++)
        {
            Destroy(cardUIList[i]);
        }
        cardUIList.Clear();
    }

    /// <summary>
    /// Helper: Finds a card GameObject by its cardValue in the cardUIList.
    /// </summary>
    private GameObject FindCardByValue(int cardValue)
    {
        foreach (var cardObj in cardUIList)
        {
            CardUI c = cardObj.GetComponent<CardUI>();
            if (c != null && c.cardValue == cardValue)
            {
                return cardObj;
            }
        }
        return null;
    }
}
