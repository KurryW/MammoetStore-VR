using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class VRToggleButton : VRButton<bool>
{
    [SerializeField] private bool isHeldToggle = false;
    [SerializeField] private bool currentState = false;
    private bool startState = false;

#if UNITY_EDITOR
    [SerializeField] private bool copyCurrentForOff;
#endif
    [SerializeField] private Vector3 offPosition, offRotation, offScale;
#if UNITY_EDITOR
    [SerializeField] private bool copyCurrentForOn;
#endif
    [SerializeField] private Vector3 onPosition, onRotation, onScale;

    public override bool GetCurrentState()
    {
        return currentState;
    }

    public override void Reset()
    {
        currentState = startState;
        UpdateTransform();
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        startState = currentState;
        UpdateTransform();
    }

    void Update()
    {
#if UNITY_EDITOR
        if(copyCurrentForOff)
        {
            copyCurrentForOff = false;
            offPosition = transform.localPosition;
            offRotation = transform.localEulerAngles;
            offScale = transform.localScale;
        }

        if (copyCurrentForOn)
        {
            copyCurrentForOn = false;
            onPosition = transform.localPosition;
            onRotation = transform.localEulerAngles;
            onScale = transform.localScale;
        }

        if (!Application.isPlaying)
            return;
#endif

        InteractionState interaction = GetInteractionState();

        if (Locked)
            return;

        if (isHeldToggle)
        {
            currentState = interaction.LeftHeld || interaction.RightHeld;
            UpdateTransform();
        }
        else
        {
            if (interaction.LeftPressedThisFrame || interaction.RightPressedThisFrame)
            {
                currentState = !currentState;
                UpdateTransform();
            }
        }
    }

    void UpdateTransform()
    {
        transform.localPosition = currentState ? onPosition : offPosition;
        transform.localEulerAngles = currentState ? onRotation : offRotation;
        transform.localScale = currentState ? onScale : offScale;
    }
}
