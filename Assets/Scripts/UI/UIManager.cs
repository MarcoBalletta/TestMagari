using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameMode gameMode;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private GameObject cardsPanel;
    [SerializeField] private CardHandler cardPrefab;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI totalTurnsText;
    [SerializeField] private Button skipPhaseButton;

    private void Awake()
    {
        gameMode.playerChooseStartingTile += ChooseStartingTile;
        gameMode.pickCard += TellWhichPlayerTurn;
        gameMode.pickCard += ShowSkipButton;
        gameMode.pickCard += ShowCardsPanel;
        gameMode.pickedCard += DisableCardsPanel;
        gameMode.movePlayerToken += MoveTokenStartUI;
        gameMode.updateUICards += ShowCardsPanel;
        gameMode.discardCard += ShowCardsPanel;
        gameMode.discardCard += HideSkipButton;
        gameMode.playerMoved += HideSkipButton;
        cameraManager.enableMainCamera += ShowCardsPanel;
        cameraManager.enableTableCamera += DisableCardsPanel;
    }

    private void ChooseStartingTile()
    {
        if (gameMode.StateManager.Current.Name != Constants.STATE_PLAYERCHOOSING_ID) return;
        infoText.text = Constants.INFO_SELECT_STARTING_POINT + (gameMode.GameState.PlayerTurn +1).ToString();
    }

    private void TellWhichPlayerTurn()
    {
        infoText.text = Constants.INFO_PLAYER_PICKCARD + (gameMode.GameState.PlayerTurn + 1).ToString();
        totalTurnsText.text = gameMode.GameState.TotalTurns.ToString();
    }

    private void ShowCardsPanel()
    {
        cardsPanel.SetActive(true);
        ShowCards(gameMode.GameState.PlayersInGame[gameMode.GameState.PlayerTurn].PlayerCards);
    }

    private void DisableCardsPanel()
    {
        cardsPanel.SetActive(false);
    }

    private void ShowCards(Card[] cards)
    {
        foreach(var cardToDestroy in cardsPanel.GetComponentsInChildren<CardHandler>())
        {
            Destroy(cardToDestroy.gameObject);
        }
        if(cards != null)
        {
            foreach(var card in cards)
            {
                if(card != null)
                {
                    var cardImage = Instantiate(cardPrefab, cardsPanel.transform);
                    cardImage.Setup(card, gameMode);
                }
            }
        }
    }

    private void MoveTokenStartUI()
    {
        infoText.text = Constants.INFO_PLAYER_MOVETOKEN + (gameMode.GameState.PlayerTurn + 1).ToString();
    }

    private void ShowSkipButton()
    {
        skipPhaseButton.gameObject.SetActive(true);
    }    
    
    private void HideSkipButton()
    {
        skipPhaseButton.gameObject.SetActive(false);
    }
}
