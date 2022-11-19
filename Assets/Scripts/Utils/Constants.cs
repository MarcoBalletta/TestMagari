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
    public static int STANDARD_GRID_ROW_SIZE = 8;
    public static int STANDARD_GRID_COLUMN_SIZE = 8;
    public static float PERCENTAGE_IF_TRAP = 15f;
    #endregion
    #region Scenes Names
    public static string GAME_SCENE_NAME = "Game";
    public static string MENU_SCENE_NAME = "MainMenu";
    public static string TUTORIAL_SCENE_NAME = "Tutorial";
    #endregion
    #region Tutorial info text
    public static string TUTORIAL_INTRO_TEXT = "Welcome to the tutorial of this game. Your goal will be to reach the ending tile. To do that you need to create your path using the cards in your hands. Click on your card to select it.";
    public static string TUTORIAL_CREATE_PATH_TEXT = "Click on an empty tile to create your path. (You can also select a non-empty tile, as soon as there isn't any player on it or it isn't a starting/ending tile.)";
    public static string TUTORIAL_SKIP_BUTTON_TEXT = "If you don't have cards to play or don't want to play any card, or if you don't want to move your player or don't have moves to make you can skip the phase and go to the next one. Try it now!";
    public static string TUTORIAL_MOVEPLAYER_TEXT = "Click on the created tile to move your player there. (You can move on a tile where there is another player on, you will switch positions!)";
    public static string TUTORIAL_ENDTILE_TEXT = "Click on the ending tile move your player to your goal.";
    #endregion
}
