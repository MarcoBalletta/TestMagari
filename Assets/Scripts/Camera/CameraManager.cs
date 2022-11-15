using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera onlyTableCamera;
    [SerializeField] private GameMode gameMode;


    private void Start()
    {
        gameMode.pickCard += ShowMainCamera;
        gameMode.pickedCard += ShowTableCamera;
        gameMode.movePlayerToken += ShowTableCamera;
        gameMode.discardCard += ShowMainCamera;
        gameMode.enableMainCamera += ShowMainCamera;
        gameMode.enableTableCamera += ShowTableCamera;
    }


    public void SwitchTheCameras()
    {
        if (mainCamera.isActiveAndEnabled)
        {
            gameMode.enableTableCamera();
        }
        else
        {
            gameMode.enableMainCamera();
        }
    }

    private void ShowMainCamera()
    {
        mainCamera.gameObject.SetActive(true);
        onlyTableCamera.gameObject.SetActive(false);
    }

    private void ShowTableCamera()
    {
        onlyTableCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }
}
