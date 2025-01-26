using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // for Image
using TMPro;

public class BetGame : MonoBehaviour
{
    [Header("References")]
    public CardUIManager cardUIManager; // Drag in from inspector
    public GameObject scoreIndicatorObject;
    public GameObject Canvas;

    // NEW: The 7 sprites for each treasure card (indexed 0..6)
    [Header("Treasure Graphics")]
    public Sprite[] treasureSprites; // 7 different treasure sprites
    public Sprite[] treasureSymbolSprites; // 7 different treasure sprites
    public Image currentTreasureImage; // UI Image that shows the current round's treasure

    // NEW: The 7 different prefabs you want to instantiate when treasure is won
    [Header("Treasure Prefabs (Spawn on Win)")]
    public GameObject[] treasureSymbols; // 7 prefabs, one for each treasure #1..7

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

        int currentTreasureValue = treasures[currentRound];
        Debug.Log($"Round {currentRound + 1} - Treasure worth {currentTreasureValue} points!");

        // Show the numeric value
        scoreCardsText.text = currentTreasureValue.ToString();

        // --- NEW: Update the treasure graphic in the UI Image ---
        // If your treasureValues are 1..7, map that to index 0..6 with [value-1]
        if (treasureSprites != null && treasureSprites.Length >= currentTreasureValue)
        {
            currentTreasureImage.sprite = treasureSprites[currentTreasureValue - 1];
        }

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

        // Start a coroutine to handle animations + fade to black, then continue
        StartCoroutine(OnCardChosenRoutine(chosenCard, computerCard));
    }

    private IEnumerator OnCardChosenRoutine(int playerCard, int computerCard)
    {
        // 1) Animate the player's chosen card to some position
        Vector2 playerTargetPos = new Vector2(-13f, 516f);
        yield return StartCoroutine(
            cardUIManager.AnimateCardToPosition(playerCard, playerTargetPos, .5f)
        );

        yield return new WaitForSeconds(.6f);

        // 2) Determine the winner for this round
        int currentTreasureValue = treasures[currentRound];
        bool playerWins = false;

        if (playerCard > computerCard)
        {
            playerScore += currentTreasureValue;
            playerWins = true;
            Debug.Log($"Player wins {currentTreasureValue} points!");
        }
        else if (computerCard > playerCard)
        {
            computerScore += currentTreasureValue;
            playerWins = false;
            Debug.Log($"Computer wins {currentTreasureValue} points!");
        }
        else
        {
            Debug.Log("Tie! No one gets points this round.");
        }

        // 3) If there's a winner, instantiate the treasure symbol
        //    corresponding to `currentTreasureValue`.
        if (playerCard != computerCard) // no tie
        {
            // Only do this if the arrays are set up properly
            if (treasureSymbols != null && treasureSymbols.Length >= currentTreasureValue)
            {
                // Instantiate the symbol for the *won* treasure
                GameObject symbolPrefab = treasureSymbols[currentTreasureValue - 1];
                if (symbolPrefab != null)
                {
                    // You can choose a parent or a position for these won symbols
                    // Here, for example, we just put them under the Canvas at default position.
                    

                    // Optionally, position it in some location:
                    // symbolInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);

                    //not used

                    // Or handle any other logic for the new treasure symbol
                }
            }
        }

        // 4) Identify the loser’s card (fade to black if not a tie)
        int loserCard = -1;
        if (playerCard > computerCard) loserCard = computerCard;
        else if (computerCard > playerCard) loserCard = playerCard;

        if (loserCard != -1)
        {
            // We'll do the fade logic as in your code
            GameObject scoreIndicatorInstance = Instantiate(scoreIndicatorObject, Canvas.transform);
            scoreIndicatorInstance.GetComponent<Image>().sprite = treasureSymbolSprites[currentTreasureValue - 1];
            scoreIndicatorInstance.GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(262, 52, 0);
            scoreIndicatorInstance.transform.localScale = new Vector3(1, 1.2f, 1);

            if (!playerWins)
            {
                // player lost, fade player's card
                yield return StartCoroutine(cardUIManager.FadeCardToBlack(playerCard, 1f));
                yield return StartCoroutine(cardUIManager.AnimateObjectToPosition(
                    scoreIndicatorInstance,
                    enemyPointText.rectTransform.anchoredPosition,
                    .5f
                ));
            }
            else
            {
                // computer lost, fade enemy's card
                yield return StartCoroutine(cardUIManager.FadeEnemyCardToBlack(1f));
                yield return StartCoroutine(cardUIManager.AnimateObjectToPosition(
                    scoreIndicatorInstance,
                    playerPointText.rectTransform.anchoredPosition,
                    .5f
                ));
            }

            DestroyImmediate(scoreIndicatorInstance, true);
        }

        // 5) Wait a short time with the black card shown
        yield return new WaitForSeconds(.4f);

        Debug.Log($"Scores -> Player: {playerScore}, Computer: {computerScore}");
        playerPointText.text = "Your Point: " + playerScore;
        enemyPointText.text = "Their Point: " + computerScore;

        // 6) Discard both cards
        cardUIManager.DiscardCardImmediate(playerCard);

        // Turn the enemy card text back to hidden
        cardUIManager.returnEnemyCardToWhite();
        enemyCardValueText.text = "";

        // Example scene transition (your code)
        

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
            DialogueManager.Instance.StartDialogue(dialoguesWining, () => {
                SceneTransition.Instance.TransitionToScene("Mask");

            });
        }
        else if (computerScore > playerScore)
        {
            Debug.Log("Computer wins the game!");
            DialogueManager.Instance.StartDialogue(dialoguesLosing, () => {
                SceneTransition.Instance.TransitionToScene("Mask");

            });
        }
        else
        {
            Debug.Log("It's a tie!");
            DialogueManager.Instance.StartDialogue(dialoguesLosing, () => {
                SceneTransition.Instance.TransitionToScene("Mask");

            });
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
