using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private BaseState current;
    private Dictionary<string, BaseState> listOfStates = new Dictionary<string, BaseState>();
    public BaseState Current { get => current; set => current = value; }

    private void Awake()
    {
        CreateDictionary();
    }
    private void CreateDictionary()
    {
        listOfStates.Add(Constants.STATE_PREGAME_ID, new PreGame());
        listOfStates.Add(Constants.STATE_PLAYERCHOOSING_ID, new PlayerChoosing());
        listOfStates.Add(Constants.STATE_STARTGAME_ID, new GameStart());
        listOfStates.Add(Constants.STATE_PICKCARD_ID, new PickCardState());
        listOfStates.Add(Constants.STATE_MOVEPLAYERTOKEN_ID, new MovePlayerTokenState());
        listOfStates.Add(Constants.STATE_DISCARDCARD_ID, new DiscardCard());
        listOfStates.Add(Constants.STATE_ENDTURN_ID, new EndTurn());
        listOfStates.Add(Constants.STATE_ENDGAME_ID, new EndGame());
    }

    public void ChangeState(string id, GameMode gm)
    {
        current?.Exit(gm);
        current = listOfStates[id];
        current.Enter(gm);
    }
}
