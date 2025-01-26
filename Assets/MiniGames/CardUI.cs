
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Represents a single card in the player's hand.
/// </summary>
public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Card Value")]
    public int cardValue;

    [Header("UI References")]
    public TextMeshProUGUI cardValueText; // Display the number on the card (or use an Image)

    // You can store reference to the manager or the BetGame script 
    // that will handle the actual logic
    private BetGame betGameRef;

    private Vector3 originalPosition;
    private bool isHovered = false;

    private void Start()
    {
        // Show card value in UI
        if (cardValueText != null)
        {
            cardValueText.text = cardValue.ToString();
        }

        // Save original position for hover effect
        originalPosition = transform.localPosition;
    }

    /// <summary>
    /// Use this to initialize references (like the betGameRef) after instantiation.
    /// </summary>
    public void InitCard(BetGame gameRef, int value)
    {
        betGameRef = gameRef;
        cardValue = value;
        if (cardValueText != null)
        {
            cardValueText.text = cardValue.ToString();
        }
    }

    // Mouse hover logic
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovered)
        {
            originalPosition = transform.localPosition;
            // For example, move the card up slightly or scale it.
            transform.localPosition = originalPosition + new Vector3(0, 20f, 0);
            isHovered = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHovered)
        {
            transform.localPosition = originalPosition;
            isHovered = false;
        }
    }

    // Clicking the card => choose that card
    public void OnPointerClick(PointerEventData eventData)
    {
        // Tell the BetGame script that this card was chosen
        if (betGameRef != null)
        {
            betGameRef.OnPlayerChooseCard(cardValue);
        }
    }
}

