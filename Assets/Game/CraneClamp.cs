using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneClamp : MonoBehaviour
{
    public Transform craneRoot; // Drag the crane's real parent here in the Inspector
    public float minDistanceFromGround = 0.05f; // Minimum height above ground
    public float raycastLength = 1f; // How far down to check for the ground

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * raycastLength, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastLength))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                float distance = hit.distance;
                if (distance < minDistanceFromGround)
                {
                    // Lift the crane up so the child doesn't clip into the ground
                    float correction = minDistanceFromGround - distance;
                    craneRoot.position += new Vector3(0, correction, 0);
                }
            }
        }
    }
}
