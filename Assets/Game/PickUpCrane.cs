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

    public float heightCorrection;

    private void Awake()
    {
        controls = new JoystickController();

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger hit by: " + other.name);
        PickupUI.SetActive(true);

        // Change color of the assigned cube, not the one that entered
        if (rend != null)
        {
            rend.material.color = new Color(100, 69, 50);
        }

        Transform parentTransform = other.transform.parent;

        if (parentTransform != null)
        {
            pendingObjectToPickUp = parentTransform.gameObject;
            Debug.Log("Object ready to pick up: " + pendingObjectToPickUp.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited trigger: " + other.name);

        if (rend != null)
        {
            rend.material.color = originalColor; // Restore original color
        }

        PickupUI.SetActive(false);
    }

    private void OnPickUpPressed(InputAction.CallbackContext context)
    {
        if (pendingObjectToPickUp != null)
        {
            // Reparent the object to this hook (or your crane / hand)
            pendingObjectToPickUp.transform.SetParent(transform);

            pendingObjectToPickUp.transform.localPosition = new Vector3(0, heightCorrection, 0);
            
            Debug.Log("Picked up: " + pendingObjectToPickUp.name);

            // Clear reference so it doesn't pick the same thing again
            pendingObjectToPickUp = null;

            rend.material.color = new Color(0, 0, 0);

            kabel.SetActive(true);

            Destroy(Cube);

        }
        else
        {
            Debug.Log("No object to pick up.");
        }
    }
}
