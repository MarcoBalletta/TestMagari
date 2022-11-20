using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUIManager : UIManager
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI infoPanelText;

    protected override void Awake()
    {
        base.Awake();
        gameMode.pickCard += ShowInfoPanel;
        gameMode.pickedCard += ShowInfoPanel;
        gameMode.movePlayerToken += ShowInfoPanel;
        gameMode.playerMoved += ShowInfoPanel;
        gameMode.tilePlaced += ShowInfoPanel;
    }

    private void ShowInfoPanel()
    {
        SetTheTextInInfoPanel();
        infoPanel.gameObject.SetActive(true);
    }

    private void SetTheTextInInfoPanel()
    {
        if (gameMode.StateManager.Current.Name == Constants.STATE_PICKCARD_ID)
        {
            if (infoPanelText.text == "") infoPanelText.text = Constants.TUTORIAL_INTRO_TEXT;
            else if (infoPanelText.text == Constants.TUTORIAL_INTRO_TEXT) infoPanelText.text = Constants.TUTORIAL_CREATE_PATH_TEXT;
            else if (infoPanelText.text == Constants.TUTORIAL_CREATE_PATH_TEXT) infoPanelText.text = Constants.TUTORIAL_SKIP_BUTTON_TEXT;
        }
        else if (gameMode.StateManager.Current.Name == Constants.STATE_MOVEPLAYERTOKEN_ID)
        {
            if (infoPanelText.text == Constants.TUTORIAL_SKIP_BUTTON_TEXT) infoPanelText.text = Constants.TUTORIAL_MOVEPLAYER_TEXT;
            else if (infoPanelText.text == Constants.TUTORIAL_MOVEPLAYER_TEXT) infoPanelText.text = Constants.TUTORIAL_ENDTILE_TEXT;
        }
    }

    public void HideInfoPanel()
    {
        infoPanel.gameObject.SetActive(false);
    }
}
