using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BetGame : MonoBehaviour
{
    [Header("References")]
    public CardUIManager cardUIManager; // Drag in from inspector
    public GameObject scoreIndicatorObject;
    public GameObject Canvas;

    private List<int> playerHand;
    private List<int> computerHand;
    private List<int> treasures;

    public Dialogue dialoguesWining;
    public Dialogue dialoguesLosing;

    private int playerScore;
    private int computerScore;
    private int currentRound;
    private bool waitingForPlayerChoice;


    [Header("UI")]
    public TextMeshProUGUI scoreCardsText;
    public TextMeshProUGUI enemyCardValueText;
    public TextMeshProUGUI playerPointText;
    public TextMeshProUGUI enemyPointText;

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
        scoreCardsText.text = treasures[currentRound].ToString();

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

        // Computer picks a card
        int computerCard = ComputerPickCard();
        computerHand.Remove(computerCard);

        enemyCardValueText.text = computerCard.ToString();

        Debug.Log($"Player chose card: {chosenCard}");
        Debug.Log($"Computer chose card: {computerCard}");

        // Start a coroutine to handle animations + fade to black, then continue game
        StartCoroutine(OnCardChosenRoutine(chosenCard, computerCard));
    }

    private IEnumerator OnCardChosenRoutine(int playerCard, int computerCard)
    {
        // 1) Animate the player's chosen card to some position
        Vector2 playerTargetPos = new Vector2(-24f, 470f);
        yield return StartCoroutine(
            cardUIManager.AnimateCardToPosition(playerCard, playerTargetPos, .5f)
        );

        yield return new WaitForSeconds(.6f);

        // 3) Determine the winner for this round
        bool playerWins = false;
        if (playerCard > computerCard)
        {
            playerScore += treasures[currentRound];
            playerWins = true;
            Debug.Log($"Player wins {treasures[currentRound]} points!");
        }
        else if (computerCard > playerCard)
        {
            computerScore += treasures[currentRound];
            playerWins = false;
            Debug.Log($"Computer wins {treasures[currentRound]} points!");
        }
        else
        {
            Debug.Log("Tie! No one gets points this round.");
        }


        // 4) Fade the *loser’s* card to black (or both, if tie, etc.)
        //    We'll wait for 3 seconds, then discard them.

        int loserCard = (playerWins) ? computerCard : playerCard;
        if (playerCard == computerCard)
        {
            // If tie, you could handle differently, or fade none, or fade both.
            // We'll just fade none for a tie.
            loserCard = -1;
        }



        // 5) If there's a loser card, fade it to black
        if (loserCard != -1)
        {
            GameObject scoreIndicatorInstance = Instantiate(scoreIndicatorObject, Canvas.transform);
            if (!playerWins)
            {
                yield return StartCoroutine(cardUIManager.FadeCardToBlack(playerCard, 1f));
                yield return StartCoroutine(cardUIManager.AnimateObjectToPosition(scoreIndicatorInstance, enemyPointText.rectTransform.anchoredPosition, .5f));
            }
            else
            {
                yield return StartCoroutine(cardUIManager.FadeEnemyCardToBlack(1f));
                yield return StartCoroutine(cardUIManager.AnimateObjectToPosition(scoreIndicatorInstance, playerPointText.rectTransform.anchoredPosition, .5f));
            }
            DestroyImmediate(scoreIndicatorInstance, true);
        }

        // 6) Wait 3 seconds with the black card shown
         
         
         yield return new WaitForSeconds(.4f);

        //now display the differnet color

        Debug.Log($"Scores -> Player: {playerScore}, Computer: {computerScore}");
        playerPointText.text = "Your Point: " + playerScore;
        enemyPointText.text = "Their Point: " + computerScore;

        // 7) Now discard BOTH cards from the UI manager

        cardUIManager.DiscardCardImmediate(playerCard);

        // Turn the enemy card text back to hidden
        cardUIManager.returnEnemyCardToWhite();
        enemyCardValueText.text = "";

        // Move to the next round
        currentRound++;
        StartNewRound();
    }

    private int ComputerPickCard()
    {
        // Simple random pick
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
            DialogueManager.Instance.StartDialogue(dialoguesWining);
        }
        else if (computerScore > playerScore)
        {
            Debug.Log("Computer wins the game!");
            DialogueManager.Instance.StartDialogue(dialoguesLosing);
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
