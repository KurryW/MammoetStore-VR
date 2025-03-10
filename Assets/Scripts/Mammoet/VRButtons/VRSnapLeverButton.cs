using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SnapLeverPositions
{
    Up, Center, Down
}

public class VRSnapLeverButton : VRButton<SnapLeverPositions>
{
    [SerializeField] private bool FlipUpDown = false;
    public bool UpEnabled, CenterEnabled, DownEnabled;
    [SerializeField] private float angleUp, angleDown, angleCenter;
    [SerializeField] private Transform pivot;
    [SerializeField] private SnapLeverPositions defaultPosition;
    private float angle = 0;

    [SerializeField] private SnapLeverPositions currentPosition;
    [SerializeField] private bool overrideGrab;

    private float defaultAngle
    {
        get
        {
            if (defaultPosition == SnapLeverPositions.Center)
                return angleCenter;
            else 
                return (defaultPosition == SnapLeverPositions.Up) != FlipUpDown ? angleUp : angleDown;
        }
    }

    private float targetAngle
    {
        get
        {
            if (currentPosition == SnapLeverPositions.Center)
                return angleCenter;
            else
                return (currentPosition == SnapLeverPositions.Up) != FlipUpDown ? angleUp : angleDown;
        }
    }

    public override SnapLeverPositions GetCurrentState()
    {
        return currentPosition;
    }

    private void Start()
    {
        angle = defaultAngle;
        currentPosition = defaultPosition;
    }

    // Update is called once per frame
    void Update()
    {
        InteractionState state = GetInteractionState();

        if (Locked)
            return;

        Vector3 heldPos = transform.position;
        bool defaulting = false;

        if (state.LeftHeld || overrideGrab)
        {
            heldPos = VRControllerReferenceFinder.LeftObject.transform.position;
        }
        else if (state.RightHeld)
        {
            heldPos = VRControllerReferenceFinder.RightObject.transform.position;
        }
        else if (angle != targetAngle)
        {
            angle = Mathf.Lerp(angle, targetAngle, 0.1f);
            defaulting = true;
        }

        if (!defaulting && (state || overrideGrab))
        { 
            Vector3 localHeldPos = pivot.transform.worldToLocalMatrix.MultiplyPoint(heldPos);

            localHeldPos.x = 0;
            localHeldPos.Normalize();

            angle += Vector3.SignedAngle(Vector3.up, localHeldPos, Vector3.right);

            float toUp = Mathf.Abs(angle - (FlipUpDown? angleDown : angleUp));
            float toDown = Mathf.Abs(angle - (FlipUpDown ? angleUp : angleDown));
            float toCenter = Mathf.Abs(angle - angleCenter);

            float smallest = 400;

            if (CenterEnabled && toCenter < smallest)
            {
                smallest = toCenter;
                currentPosition = SnapLeverPositions.Center;
            }

            if (UpEnabled && toUp < smallest)
            {
                smallest = toUp;
                currentPosition = SnapLeverPositions.Up;
            }

            if (DownEnabled && toDown < smallest)
            {
                currentPosition = SnapLeverPositions.Down;
            }
        }

        float min = Mathf.Min(angleUp, angleDown, angleCenter);
        float max = Mathf.Max(angleUp, angleDown, angleCenter);

        angle = Mathf.Clamp(angle, min, max);

        Vector3 currentAngles = pivot.localEulerAngles;

        pivot.localEulerAngles = new Vector3(angle, currentAngles.y, currentAngles.z);
    }

    public override void Reset()
    {
        currentPosition = defaultPosition;
    }
}
