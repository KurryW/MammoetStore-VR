using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCheckmarkButton : VRButton<bool>
{
    [SerializeField] private bool currentState = false;
    private bool startState = false;

    [SerializeField] private Material checkedOn, checkedOff;
    [SerializeField] private Renderer checkedRenderer;
    [SerializeField] private int rendererMaterialIndex;

    public override bool GetCurrentState()
    {
        return currentState;
    }

    public override void Reset()
    {
        currentState = startState;
        UpdateImage();
    }

    private void Start()
    {
        startState = currentState;
        UpdateImage();
    }

    void Update()
    {
        InteractionState interaction = GetInteractionState();

        if (Locked)
            return;

        if (interaction.LeftPressedThisFrame || interaction.RightPressedThisFrame)
        {
            currentState = !currentState;
            UpdateImage();
        }
    }

    void UpdateImage()
    {
        Material[] mats = checkedRenderer.materials;
        mats[rendererMaterialIndex] = currentState ? checkedOn : checkedOff;
        checkedRenderer.materials = mats;
    }
}
