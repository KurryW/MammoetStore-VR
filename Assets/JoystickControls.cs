using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickControls : MonoBehaviour
{
    private JoystickController controls; // Reference to the generated Input Actions class

    public Transform craneBase;  // Assign the base of the crane to rotate
    public Transform cable;      // Assign the cable or hook to move up and down
    public Transform hookPivot;

    public float rotationSpeed = 20f;  // Speed for rotating the crane and hook
    public float cableSpeed = 5f;      // Speed for moving the cable up/down

    private float rotateCraneInput;
    private float rotateHookInput;
    private float cableInput;

    

    void Awake()
    {
        controls = new JoystickController(); // Instantiate the input system
        float hookInput = controls.Gameplay.RotateHook.ReadValue<float>();

        // Bind joystick actions

        controls.Gameplay.RotatePTC.performed += ctx => rotateCraneInput = ctx.ReadValue<float>();
        controls.Gameplay.RotatePTC.canceled += ctx => rotateCraneInput = 0f;

        controls.Gameplay.RotateHook.performed += ctx => rotateHookInput = ctx.ReadValue<float>();
        controls.Gameplay.RotateHook.canceled += ctx => rotateHookInput = 0f;

        controls.Gameplay.CableUpanddown.performed += ctx => cableInput = ctx.ReadValue<float>();
        controls.Gameplay.CableUpanddown.canceled += ctx => cableInput = 0f;
    }

    void OnEnable() => controls.Gameplay.Enable();
    void OnDisable() => controls.Gameplay.Disable();

    void Update()
    {
        // Rotate the crane
        if (rotateCraneInput != 0)
        {
            craneBase.Rotate(Vector3.up * rotateCraneInput * rotationSpeed * Time.deltaTime);
        }

        // Rotate the hook
        if (rotateHookInput != 0)
        {
            hookPivot.Rotate(Vector3.up * rotateHookInput * rotationSpeed * Time.deltaTime);

        }

        // Move the cable up and down
        if (cableInput != 0)
        {
            cable.Translate(Vector3.up * cableInput * cableSpeed * Time.deltaTime);
        }
    }
}


