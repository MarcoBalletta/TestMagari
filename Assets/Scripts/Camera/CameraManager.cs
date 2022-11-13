using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera onlyTableCamera;
    [SerializeField] private GameMode gameMode;
    public delegate void EnableMainCamera();
    public EnableMainCamera enableMainCamera;
    public delegate void EnableTableCamera();
    public EnableMainCamera enableTableCamera;

    private void Start()
    {
        gameMode.pickCard += ShowMainCamera;
        gameMode.pickedCard += ShowTableCamera;
        gameMode.movePlayerToken += ShowTableCamera;
        enableMainCamera += ShowMainCamera;
        enableTableCamera += ShowTableCamera;
    }


    public void SwitchTheCameras()
    {
        if (mainCamera.isActiveAndEnabled)
        {
            enableTableCamera();
        }
        else
        {
            enableMainCamera();
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
