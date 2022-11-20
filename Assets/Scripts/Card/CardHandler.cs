using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardHandler : MonoBehaviour, IPointerClickHandler
{
    private Card card;
    private GameMode gameMode;
    private AudioSource source;
    [SerializeField] private AudioClip spawnClip;
    [SerializeField] private AudioClip discardClip;

    public void Setup(Card cardData, GameMode gm)
    {
        card = cardData;
        gameMode = gm;
        GetComponent<Image>().sprite = card.Image;
        GetComponent<Animator>().SetTrigger("Spawn");
        source = GetComponent<AudioSource>();
        source.PlayOneShot(spawnClip);
    }

    public void DiscardPlayAudio()
    {
        source.PlayOneShot(discardClip);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(gameMode.StateManager.Current.Name == Constants.STATE_PICKCARD_ID)
        {
            gameMode.GameState.PlayersInGame[gameMode.GameState.PlayerTurn].CardSelected = card;
            gameMode.pickedCard();
        }else if(gameMode.StateManager.Current.Name == Constants.STATE_DISCARDCARD_ID)
        {
            DiscardPlayAudio();
            gameMode.GameState.PlayersInGame[gameMode.GameState.PlayerTurn].DiscardCard(card);
        }
    }
}
