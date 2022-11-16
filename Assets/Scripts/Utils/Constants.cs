using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    #region States names
    public static string STATE_PREGAME_ID = "state:pregame";
    public static string STATE_PLAYERCHOOSING_ID = "state:playerchoosing";
    public static string STATE_STARTGAME_ID = "state:startgame";
    public static string STATE_PICKCARD_ID = "state:pickcard";
    public static string STATE_MOVEPLAYERTOKEN_ID = "state:moveplayertoken";
    public static string STATE_DISCARDCARD_ID = "state:discardcard";
    public static string STATE_ENDTURN_ID = "state:endturn";
    public static string STATE_ENDGAME_ID = "state:endgame";
    #endregion
    #region UI
    public static string INFO_SELECT_STARTING_POINT = "Please select starting point player ";
    public static string INFO_PLAYER_PICKCARD = "Choose a card to place, player ";
    public static string INFO_PLAYER_MOVETOKEN = "Move your token, player ";
    public static string INFO_DISCARD_CARD = "Discard a card, player ";
    #endregion
    #region PlayerStats
    public static int MAX_NUMBER_OF_CARDS = 5;
    public static string ANIM_MOVING_PARAMETER = "Moving";
    #endregion
    #region GameRules
    public static float MINIMUM_DISTANCE_FROM_STARTINGTILE = 3f;
    public static int MAXIMUM_CARDS_PLAYABLE = 3;
    public static int MAXIMUM_TOKEN_STEPS = 3;
    public static float PERCENTAGE_IF_TRAP = 15f;
    #endregion
}
