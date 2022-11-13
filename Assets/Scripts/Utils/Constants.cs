using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    #region States names
    public static string STATE_PREGAME_ID = "state:pregame";
    public static string STATE_STARTGAME_ID = "state:startgame";
    public static string STATE_PLAYERCHOOSING_ID = "state:playerchoosing";
    public static string STATE_MOVEPLAYERTOKEN_ID = "state:moveplayertoken";
    public static string STATE_PICKCARD_ID = "state:pickcard";
    public static string STATE_ENDGAME_ID = "state:endgame";
    #endregion
    #region UI
    public static string INFO_SELECT_STARTING_POINT = "Please select starting point player ";
    public static string INFO_PLAYER_PLAYING = "Turn player ";
    public static string INFO_DISCARD_CARD = "Please discard a card";
    #endregion
    #region PlayerStats
    public static int MAX_NUMBER_OF_CARDS = 5;
    #endregion
    #region GameRules
    public static float MINIMUM_DISTANCE_FROM_STARTINGTILE = 3f;
    #endregion
}
