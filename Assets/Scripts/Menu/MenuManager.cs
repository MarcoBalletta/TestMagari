using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private AudioClip clickSound;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(Constants.GAME_SCENE_NAME);
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene(Constants.TUTORIAL_SCENE_NAME);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetRows(TMP_InputField text)
    {
        int.TryParse(text.text, out GridDataSelection.rows);
    }    
    
    public void SetColumns(TMP_InputField text)
    {
        int.TryParse(text.text, out GridDataSelection.columns);
    }

    public void ReproduceSound()
    {
        source.PlayOneShot(clickSound);
    }
}
