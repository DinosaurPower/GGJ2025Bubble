using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BetGame : MonoBehaviour
{
    [Header("References")]
    public CardUIManager cardUIManager; // Drag in from inspector

    private List<int> playerHand;
    private List<int> computerHand;
    private List<int> treasures;

    private int playerScore;
    private int computerScore;
    private int currentRound;
    private bool waitingForPlayerChoice;

    [Header("UI")]
    public TextMeshProUGUI scoreCardsText;
    public TextMeshProUGUI enemyCardValueText;


    void Start()
    {
        // Example: Initialize the player's and computer's hands
        playerHand = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
        computerHand = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };

        // Shuffle or randomize the treasure values [1..7]
        treasures = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
        ShuffleList(treasures);

        playerScore = 0;
        computerScore = 0;
        currentRound = 0;

        // Create the player's UI hand
        cardUIManager.CreatePlayerHand(playerHand.ToArray());

        // Start the first round
        StartNewRound();
    }

    void StartNewRound()
    {
        if (currentRound >= 7)
        {
            DetermineWinner();
            return;
        }

        Debug.Log($"Round {currentRound + 1} - Treasure worth {treasures[currentRound]} points!");
        scoreCardsText.text = "" + treasures[currentRound];
        //Debug.Log("Player Hand: " + string.Join(", ", playerHand));
        //Debug.Log("Computer Hand: " + string.Join(", ", computerHand));

        waitingForPlayerChoice = true;
    }

    /// <summary>
    /// Called by the CardUI when the player clicks a card.
    /// </summary>
    public void OnPlayerChooseCard(int chosenCard)
    {
        if (!waitingForPlayerChoice)
        {
            Debug.LogWarning("Not expecting a player choice right now.");
            return;
        }

        // Verify card is in player's hand
        if (!playerHand.Contains(chosenCard))
        {
            Debug.LogError("The chosen card is not in the player's hand!");
            return;
        }

        waitingForPlayerChoice = false;

        // Remove from player's hand
        playerHand.Remove(chosenCard);

        // Discard from UI
        cardUIManager.DiscardCard(chosenCard);

        // Computer picks a card
        int computerCard = ComputerPickCard();
        computerHand.Remove(computerCard);
        enemyCardValueText.text = "" + computerCard;

        Debug.Log($"Player chose card: {chosenCard}");
        Debug.Log($"Computer chose card: {computerCard}");

        // Compare
        if (chosenCard > computerCard)
        {
            playerScore += treasures[currentRound];
            Debug.Log($"Player wins {treasures[currentRound]} points!");
        }
        else if (computerCard > chosenCard)
        {
            computerScore += treasures[currentRound];
            Debug.Log($"Computer wins {treasures[currentRound]} points!");
        }
        else
        {
            Debug.Log("Tie! No one gets points this round.");
        }

        Debug.Log($"Scores -> Player: {playerScore}, Computer: {computerScore}");

        currentRound++;
        StartNewRound();
    }

    private int ComputerPickCard()
    {
        // Simple random pick from the computer hand
        int index = Random.Range(0, computerHand.Count);
        return computerHand[index];
    }

    private void DetermineWinner()
    {
        Debug.Log("All 7 rounds completed!");
        Debug.Log($"Final Scores -> Player: {playerScore}, Computer: {computerScore}");

        if (playerScore > computerScore)
        {
            Debug.Log("Player wins the game!");
        }
        else if (computerScore > playerScore)
        {
            Debug.Log("Computer wins the game!");
        }
        else
        {
            Debug.Log("It's a tie!");
        }
    }

    /// <summary>
    /// Utility method to shuffle a list (Fisher-Yates).
    /// </summary>
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int r = Random.Range(i, list.Count);
            list[i] = list[r];
            list[r] = temp;
        }
    }
}
