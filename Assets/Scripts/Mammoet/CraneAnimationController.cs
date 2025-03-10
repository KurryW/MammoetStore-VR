using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraneAnimationController : MonoBehaviour
{
    private Animator animationToControl;
    [SerializeField] private PlayStates playState;
    private PlayStates directionState = PlayStates.Playing;
    [SerializeField] private float startTimeOnClipForRewinding = 1;

    private float speed
    {
        get
        {
            return (float)playState;
        }
    }
    
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private InputActionReference forwardsAction;
    [SerializeField] private InputActionReference backwardsAction;

    public enum PlayStates
    {
        Paused = 0, Playing = 1, Rewinding = -11

    }

    // Start is called before the first frame update
    void Start()
    {
        animationToControl = GetComponent<Animator>();
        UpdatePlayState();
    }

    // Update is called once per frame
    void Update()
    {
        if(pauseAction.action.triggered)
            playState = playState == PlayStates.Paused ? directionState : PlayStates.Paused;

        if (forwardsAction.action.triggered)
            playState = PlayStates.Playing;

        if (backwardsAction.action.triggered)
            playState = PlayStates.Rewinding;

        if (playState == PlayStates.Rewinding)
        {
            if(animationToControl.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0)
            {
                for (int i = 0; i < animationToControl.runtimeAnimatorController.animationClips.Length; i++)
                {
                    if(animationToControl.GetCurrentAnimatorStateInfo(0).IsName(animationToControl.runtimeAnimatorController.animationClips[i].name) && i != 0)
                    {
                        animationToControl.Play(animationToControl.runtimeAnimatorController.animationClips[i - 1].name, 0, startTimeOnClipForRewinding);
                        Debug.Log("Jump Back");
                    }
                }
            }
        }

        UpdatePlayState();
    }

    private void UpdatePlayState()
    {
        if(playState == PlayStates.Playing)
            directionState = PlayStates.Playing;

        if (playState == PlayStates.Rewinding)
            directionState = PlayStates.Rewinding;

        animationToControl.SetFloat("Speed", speed);
    }
}
