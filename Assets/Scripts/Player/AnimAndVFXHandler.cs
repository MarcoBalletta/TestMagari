using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAndVFXHandler : MonoBehaviour
{

    [SerializeField] private GameObject stepVFX;
    private Animator anim;
    private AudioSource source;
    [SerializeField] private Transform footTransform;
    [SerializeField] private AudioClip runningClip;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    public void StepVFXStart()
    {
        Debug.Log("step");
        Instantiate(stepVFX, footTransform.position, Quaternion.identity, transform);
        source.PlayOneShot(runningClip);
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
