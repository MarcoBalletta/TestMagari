using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    protected AudioSource source;
    [SerializeField] protected GameMode gameMode;
    [SerializeField] protected GameObject cardsPanel;
    [SerializeField] protected GameObject endGamePanel;
    [SerializeField] protected TextMeshProUGUI endGameWinningPlayer;
    [SerializeField] protected CardHandler cardPrefab;
    [SerializeField] protected TextMeshProUGUI infoText;
    [SerializeField] protected TextMeshProUGUI totalTurnsText;
    [SerializeField] protected Button skipPhaseButton;
    [SerializeField] protected AudioClip newTextSound;

    protected virtual void Awake()
    {
        gameMode.playerChooseStartingTile += ChooseStartingTile;
        gameMode.pickCard += TellWhichPlayerTurn;
        gameMode.pickCard += ShowSkipButton;
        gameMode.pickCard += ShowCardsPanel;
        gameMode.pickedCard += DisableCardsPanel;
        gameMode.movePlayerToken += MoveTokenStartUI;
        gameMode.movePlayerToken += DisableCardsPanel;
        gameMode.movePlayerToken += ShowSkipButton;
        gameMode.updateUICards += ShowCardsPanel;
        gameMode.discardCard += ShowCardsPanel;
        gameMode.discardCard += DiscardCardStartUI;
        gameMode.endGame += EndGameUI;
        gameMode.enableMainCamera += ShowCardsPanel;
        gameMode.enableTableCamera += DisableCardsPanel;
        source = GetComponent<AudioSource>();
    }

    protected void ChooseStartingTile()
    {
        if (gameMode.StateManager.Current.Name != Constants.STATE_PLAYERCHOOSING_ID) return;
        ChangeInfoText(Constants.INFO_SELECT_STARTING_POINT + (gameMode.GameState.PlayerTurn + 1).ToString());
        //infoText.text = Constants.INFO_SELECT_STARTING_POINT + (gameMode.GameState.PlayerTurn +1).ToString();
    }

    protected void TellWhichPlayerTurn()
    {
        ChangeInfoText(Constants.INFO_PLAYER_PICKCARD + (gameMode.GameState.PlayerTurn + 1).ToString());
        //infoText.text = Constants.INFO_PLAYER_PICKCARD + (gameMode.GameState.PlayerTurn + 1).ToString();
        totalTurnsText.text = gameMode.GameState.TotalTurns.ToString();
    }

    protected void ChangeInfoText(string newText)
    {
        infoText.text = newText;
        source.PlayOneShot(newTextSound);
    }

    protected void ShowCardsPanel()
    {
        cardsPanel.SetActive(true);
        ShowCards(gameMode.GameState.PlayersInGame[gameMode.GameState.PlayerTurn].PlayerCards);
    }

    protected void DisableCardsPanel()
    {
        cardsPanel.SetActive(false);
    }

    protected void ShowCards(Card[] cards)
    {
        foreach(var cardToDestroy in cardsPanel.GetComponentsInChildren<CardHandler>())
        {
            Destroy(cardToDestroy.gameObject);
        }
        if(cards != null)
        {
            StartCoroutine(ShowCardsWithTimer(cards));
        }
    }

    protected IEnumerator ShowCardsWithTimer(Card[] cards)
    {
        foreach (var card in cards)
        {
            if (card != null)
            {
                var cardImage = Instantiate(cardPrefab, cardsPanel.transform);
                cardImage.Setup(card, gameMode);
            }
            yield return new WaitForSeconds(0.6f);
        }
    }

    protected void MoveTokenStartUI()
    {
        ChangeInfoText(Constants.INFO_PLAYER_MOVETOKEN + (gameMode.GameState.PlayerTurn + 1).ToString());
        //infoText.text = Constants.INFO_PLAYER_MOVETOKEN + (gameMode.GameState.PlayerTurn + 1).ToString();
    }    
    
    protected void DiscardCardStartUI()
    {
        ChangeInfoText(Constants.INFO_DISCARD_CARD + (gameMode.GameState.PlayerTurn + 1).ToString());
        //infoText.text = Constants.INFO_DISCARD_CARD + (gameMode.GameState.PlayerTurn + 1).ToString();
    }

    protected void ShowSkipButton()
    {
        skipPhaseButton.gameObject.SetActive(true);
    }    
    
    protected void HideSkipButton()
    {
        skipPhaseButton.gameObject.SetActive(false);
    }

    protected void EndGameUI()
    {
        endGameWinningPlayer.text = (gameMode.GameState.PlayerTurn + 1).ToString();
        endGamePanel.SetActive(true);
    }
}
