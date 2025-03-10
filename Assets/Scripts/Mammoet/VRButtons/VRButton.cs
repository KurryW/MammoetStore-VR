using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class VRButton<T> : MonoBehaviour
{
    [SerializeField] private InputActionReference activateInputActionLeft, activateInputActionRight;
    [SerializeField] private float interactRange = 0.05f;
    [SerializeField] private Material highlightMaterial, lockedHighlightMaterial;
    private Material defaultMaterial;
    [SerializeField] private Renderer rendererToHighligt;
    [SerializeField] private int materialIndex = 0;
    public bool Locked = false;

    private static List<VRDistanceTracker> vrButtons = new List<VRDistanceTracker>();
    private VRDistanceTracker tracker = new VRDistanceTracker();

    private bool leftHeld, rightHeld;

    public abstract T GetCurrentState();

    private void Awake()
    {
        tracker.button = this;
        vrButtons.Add(tracker);
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        if (rendererToHighligt)
            defaultMaterial = rendererToHighligt.materials[materialIndex];
    }

    protected InteractionState GetInteractionState()
    {
        InteractionState state = new InteractionState();
        bool materialOverride = false;

        tracker.LDistance = Vector3.Distance(transform.position, VRControllerReferenceFinder.LeftObject.transform.position);
        tracker.RDistance = Vector3.Distance(transform.position, VRControllerReferenceFinder.RightObject.transform.position);

        bool closestL = vrButtons.Min(btn => btn.LDistance) == tracker.LDistance;
        bool closestR = vrButtons.Min(btn => btn.RDistance) == tracker.RDistance;

        if (tracker.LDistance < interactRange && closestL)
        {
            if (activateInputActionLeft.action.WasPressedThisFrame())
            {
                state.LeftPressedThisFrame = true;
                leftHeld = true;
            }
            
            materialOverride = true;
        }

        if (tracker.RDistance < interactRange && closestR)
        {
            if (activateInputActionRight.action.WasPressedThisFrame())
            {
                state.RightPressedThisFrame = true;
                rightHeld = true;
            }

            materialOverride = true;
        }

        if (activateInputActionLeft.action.WasReleasedThisFrame())
        {
            leftHeld = false;
        }

        if (activateInputActionRight.action.WasReleasedThisFrame())
        {
            rightHeld = false;
        }

        state.LeftHeld = leftHeld;
        state.RightHeld = rightHeld;

        if (rendererToHighligt != null && highlightMaterial != null)
        {
            Material[] mats = rendererToHighligt.materials;
            Material lockedBlinker = Time.realtimeSinceStartup % 0.5f < 0.25f ? lockedHighlightMaterial : defaultMaterial;
            mats[materialIndex] = materialOverride ? (Locked? lockedBlinker : highlightMaterial) : defaultMaterial;
            rendererToHighligt.materials = mats;
        }

        return state;
    }

    public abstract void Reset();

    protected struct InteractionState
    {
        public bool LeftHeld;
        public bool RightHeld;
        public bool LeftPressedThisFrame;
        public bool RightPressedThisFrame;

        //Convenience operator; when you simply want to know if anything is pressing this at all, just use if(state){...}
        public static implicit operator bool(InteractionState state)
        {
            return state.LeftHeld || state.RightHeld;
        }
    }

    private class VRDistanceTracker
    {
        public float LDistance, RDistance;
        public VRButton<T> button;
    }
}
