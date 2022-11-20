using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAndVFXHandler : MonoBehaviour
{

    [SerializeField] private GameObject stepVFX;
    private Animator anim;
    [SerializeField] private Transform footTransform;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void StepVFXStart()
    {
        Instantiate(stepVFX, footTransform.position, Quaternion.identity, transform);
    }

    public void StartRunning()
    {
        anim.SetBool(Constants.ANIM_MOVING_PARAMETER, true);
    }

    public void StopRunning()
    {
        anim.SetBool(Constants.ANIM_MOVING_PARAMETER, false);
    }
}
