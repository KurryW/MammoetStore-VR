using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpCrane : MonoBehaviour
{
    private JoystickController controls; // Your Input Actions class
    private GameObject pendingObjectToPickUp; // Store object to pick up

    public Renderer rend;
    private Color originalColor;

    public GameObject kabel;
    public GameObject PickupUI;
    public GameObject Cube;

    public float minDistanceFromGround = 0.05f; // Minimum height above ground
    public float raycastLength = 1f; // How far down to check for the ground
    public Transform craneRoot; // Drag the crane's real parent here in the Inspector

    public float heightCorrection;
    [SerializeField] private Transform minHeightReference;

    private bool hasPickedUp = false;

    private Rigidbody rb;

    private void Update()
    {
        if (hasPickedUp)
        {
            RaycastHit baseHit;
            if (Physics.Raycast(craneRoot.position, Vector3.down, out baseHit, raycastLength))
            {
                Debug.DrawRay(craneRoot.position, Vector3.down * raycastLength, Color.green);

                Debug.Log("Raycast hit: " + baseHit.collider.name);

                if (baseHit.collider.CompareTag("Ground"))
                {
                    float distance = baseHit.distance;
                    if (distance < minDistanceFromGround)
                    {
                        float correction = minDistanceFromGround - distance;
                        Vector3 correctionVector = new Vector3(0, correction, 0);
                        craneRoot.position += correctionVector;
                        Debug.Log("Corrected position by: " + correction);
                    }
                }
            }
        }
    }

    private void Awake()
    {
        controls = new JoystickController();
        rb = GetComponent<Rigidbody>();

        kabel.SetActive(false);
        PickupUI.SetActive(false);

        if (rend != null)
        {
            originalColor = rend.material.color; // Save the original color
        }
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Gameplay.Pick.performed += OnPickUpPressed;
    }

    private void OnDisable()
    {
        controls.Gameplay.Pick.performed -= OnPickUpPressed;
        controls.Disable();
    }

    private void OnTriggerStay(Collider other)
    {
        

        if (other.CompareTag("Crane") && other.name == "Cube")
        {
            hasPickedUp = true;

            // Change color of the assigned cube, not the one that entered
            if (rend != null)
            {
                rend.material.color = new Color(100, 69, 50);
            }
            Debug.Log("Trigger hit by: " + other.name);
            PickupUI.SetActive(true);

            Transform parentTransform = other.transform.parent;

            if (parentTransform != null)
            {
                pendingObjectToPickUp = parentTransform.gameObject;
                Debug.Log("Object ready to pick up: " + pendingObjectToPickUp.name);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Crane") && other.name == "Cube")
        {
            Debug.Log("Exited trigger: " + other.name);

            if (rend != null)
            {
                rend.material.color = originalColor; // Restore original color
            }

            PickupUI.SetActive(false);

            hasPickedUp = false;
        }
    }

    private void OnPickUpPressed(InputAction.CallbackContext context)
    {


        if (pendingObjectToPickUp != null && hasPickedUp == true)
        {

            float minY = minHeightReference.position.y;
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Max(transform.position.y, minY),
                transform.position.z
            );

            // Reparent the object to this hook (or your crane / hand)
            pendingObjectToPickUp.transform.SetParent(transform);

            pendingObjectToPickUp.transform.localPosition = new Vector3(0, heightCorrection, 0);

            Debug.Log("Picked up: " + pendingObjectToPickUp.name);

            // Clear reference so it doesn't pick the same thing again
            pendingObjectToPickUp = null;

            rend.material.color = new Color(0, 0, 0);

            kabel.SetActive(true);
            PickupUI.SetActive(false);

            Destroy(Cube);

            
        }
        else
        {
            Debug.Log("No object to pick up.");
        }

    }

}
