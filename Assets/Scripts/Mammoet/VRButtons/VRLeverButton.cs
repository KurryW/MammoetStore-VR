using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRLeverButton : VRButton<float>
{
    [SerializeField] private Transform pivot;
    [SerializeField] private float maxAnglePositive, maxAngleNegative, minValue, maxValue;
    [SerializeField] private float defaultAngle;
    [SerializeField] private bool autoDefaultWhenNotHeld = false;
    private float angle = 0;
    private float outputValue = 0;

    public override float GetCurrentState()
    {
        return outputValue;
    }

    public override void Reset()
    {
        angle = defaultAngle;
    }

    private void Start()
    {
        angle = defaultAngle;
    }

    // Update is called once per frame
    void Update()
    {
        InteractionState state = GetInteractionState();

        if (Locked)
            return;

        Vector3 heldPos = transform.position;
        bool defaulting = false;

        if (state.LeftHeld)
        {
            heldPos = VRControllerReferenceFinder.LeftObject.transform.position;
        }
        else if (state.RightHeld)
        {
            heldPos = VRControllerReferenceFinder.RightObject.transform.position;
        }
        else if (autoDefaultWhenNotHeld && angle != defaultAngle)
        {
            angle = defaultAngle;
            defaulting = true;
        }

        if (!defaulting && state)
        { 
            Vector3 localHeldPos = pivot.transform.worldToLocalMatrix.MultiplyPoint(heldPos);

            localHeldPos.x = 0;
            localHeldPos.Normalize();

            angle += Vector3.SignedAngle(Vector3.up, localHeldPos, Vector3.right);
        }

        angle = Mathf.Clamp(angle, maxAngleNegative, maxAnglePositive);

        Vector3 currentAngles = pivot.localEulerAngles;

        pivot.localEulerAngles = new Vector3(angle, currentAngles.y, currentAngles.z);

        outputValue = Mathf.Lerp(minValue, maxValue, (angle - maxAngleNegative) / (maxAnglePositive - maxAngleNegative));
    }
}
