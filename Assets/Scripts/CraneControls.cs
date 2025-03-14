using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneControls : MonoBehaviour
{
	#region Variables
	[SerializeField] private Transform _slewObject = null;
	[SerializeField] private Transform _hoistObject = null;
	[SerializeField] private Transform _hook = null;

	[SerializeField] private float _slewFactor = .5f;
	[SerializeField] private float _hoistFactor = .5f;
	[SerializeField] private float _hookRotateFactor = .5f;
	[SerializeField] private float _initialHoistPosition = 5;
	[SerializeField] private float _initialSlewingRotation = 0;
	[SerializeField] private float _initialHookRotation = 0;
	
	[SerializeField] private Transform _groundIndicatorBlock = null;
	[SerializeField] private Transform test = null;

	public InputControls InputControls { get; private set; }
	#endregion

	#region Unity built-in methods
	private void Start()
	{
		InputControls = new InputControls();
		InputControls.Crane.Enable();
		InputControls.Crane.Reset.performed += Reset_performed;
	}
	private void Update()
	{
		// slew, limit between -25 & +60 deg
		float slewInput = InputControls.Crane.Slew.ReadValue<Vector2>().x;
		if (slewInput != 0)
		{
			// normalize
			float currentAngle = _slewObject.rotation.eulerAngles.y;
			if (currentAngle + slewInput % 360 < -180)
				currentAngle += 360;
			if (currentAngle + slewInput % 360 > 180)
				currentAngle -= 360;

			if (currentAngle + slewInput > -25 && currentAngle + slewInput < 60)
			{
				_slewObject.rotation *= Quaternion.Euler(0, _slewFactor * slewInput, 0);
			}
			else 
				print(currentAngle); // debug
		}

		// hoist, limit between 0 and 50m
		float hoistBothInput = InputControls.Crane.Hoist.ReadValue<Vector2>().y;
		float groundLevel = 1.5f;
		if (hoistBothInput != 0
			&& _groundIndicatorBlock.position.y + _hoistFactor * hoistBothInput > groundLevel
			&& _groundIndicatorBlock.position.y + _hoistFactor * hoistBothInput < groundLevel + 50)
		{
			_hoistObject.position += new Vector3(0, _hoistFactor * hoistBothInput, 0);
		}

        // hookrotate, no limit
        float hookRotateInput = InputControls.Crane.HookRotate.ReadValue<float>();
        if (hookRotateInput != 0)
        {
            _hook.rotation *= Quaternion.Euler(0, _hookRotateFactor * hookRotateInput, 0);
        }
    }
	private void OnDestroy()
	{
		InputControls.Crane.Disable();
		InputControls.Crane.Reset.performed -= Reset_performed;
	}
    #endregion

    private void Reset_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
		_slewObject.localRotation = Quaternion.Euler(0,_initialSlewingRotation,0);
        _hoistObject.localPosition =new Vector3(0, _initialHoistPosition, 0);
        _hook.localRotation = Quaternion.Euler(0, _initialHookRotation, 0);
    }
}
