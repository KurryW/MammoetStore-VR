using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneClamp : MonoBehaviour
{
    public Transform craneRoot; // Drag the crane's real parent here in the Inspector
    public Transform hook;
    public float minDistanceFromGround = 0.05f; // Minimum height above ground
    public float raycastLength = 1f; // How far down to check for the ground

    void Update()
    {
        float maxCorrection = 0f;

        // Raycast from crane base
        RaycastHit baseHit;
        if (Physics.Raycast(transform.position, Vector3.down, out baseHit, raycastLength))
        {
            if (baseHit.collider.CompareTag("Ground"))
            {
                float distance = baseHit.distance;
                if (distance < minDistanceFromGround)
                {
                    float correction = minDistanceFromGround - distance;
                    maxCorrection = Mathf.Max(maxCorrection, correction);
                }
            }
        }

        RaycastHit hookHit;
        if (Physics.Raycast(hook.position, Vector3.down, out hookHit, raycastLength))
        {
            if (hookHit.collider.CompareTag("Ground"))
            {
                float distance = hookHit.distance;
                if (distance < minDistanceFromGround)
                {
                    float correction = minDistanceFromGround - distance;
                    hook.position += new Vector3(0, correction, 0);
                }
            }
        }

        // Apply correction to both craneRoot and hook
        if (maxCorrection > 0f)
        {
            Vector3 correctionVector = new Vector3(0, maxCorrection, 0);
            craneRoot.position += correctionVector;
            hook.position += correctionVector; // keep in sync
        }
    }
}
