using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickControls : MonoBehaviour
{
    private JoystickController controls; // Reference to the generated Input Actions class

    public Transform craneBase;  // Assign the base of the crane to rotate
    public Transform hook;       // Assign the hook to rotate
    public Transform cable;      // Assign the cable or hook to move up and down

    public float rotationSpeed = 50f;  // Speed for rotating the crane and hook
    public float cableSpeed = 5f;      // Speed for moving the cable up/down

    private Vector2 rotateCraneInput;
    private Vector2 rotateHookInput;
    private float cableInput;

    void Awake()
    {
        controls = new JoystickController(); // Instantiate the input system

        // Bind joystick actions
        controls.Gameplay.RotatePTC.performed += ctx => rotateCraneInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.RotatePTC.canceled += ctx => rotateCraneInput = Vector2.zero;

        controls.Gameplay.RotateHook.performed += ctx => rotateHookInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.RotateHook.canceled += ctx => rotateHookInput = Vector2.zero;

        controls.Gameplay.CraneUpanddown.performed += ctx => cableInput = ctx.ReadValue<float>();
        controls.Gameplay.CraneUpanddown.canceled += ctx => cableInput = 0f;
    }

    void OnEnable() => controls.Gameplay.Enable();
    void OnDisable() => controls.Gameplay.Disable();

    void Update()
    {
        // Rotate the crane
        if (rotateCraneInput.x != 0)
        {
            craneBase.Rotate(Vector3.up * rotateCraneInput.x * rotationSpeed * Time.deltaTime);
        }

        // Rotate the hook
        if (rotateHookInput.x != 0)
        {
            hook.Rotate(Vector3.forward * rotateHookInput.x * rotationSpeed * Time.deltaTime);
        }

        // Move the cable up and down
        if (cableInput != 0)
        {
            cable.Translate(Vector3.up * cableInput * cableSpeed * Time.deltaTime);
        }
    }
}


