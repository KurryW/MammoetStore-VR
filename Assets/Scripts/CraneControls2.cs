using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraneControls2 : MonoBehaviour
{
	#region Variables
	[SerializeField] private Transform _slewObject = null;
	[SerializeField] private Transform _hoistObject = null;

	[SerializeField] private float _slewFactor = .5f;
	[SerializeField] private float _hoistFactor = .5f;
	[SerializeField] private Transform _groundIndicatorBlock = null;

	public NewInputControls InputControls { get; private set; }
	#endregion

	#region Unity built-in methods
	private void OnEnable()
	{
		InputControls = new NewInputControls();
		InputControls.Crane.Enable();
		InputControls.Crane.Reset.performed += Reset_performed;
		InputControls.Crane.Test.performed += Test_performed;
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

			if ((slewInput < 0 && currentAngle + slewInput > -25)
				|| (slewInput > 0 && currentAngle + slewInput < 60))
			{
				_slewObject.rotation *= Quaternion.Euler(0, _slewFactor * slewInput, 0);
			}
			else
				print(currentAngle); // debug
		}

		// hoist, limit between 0 and 50m
		float hoistBothInput = InputControls.Crane.Hoist.ReadValue<Vector2>().y;
		float groundLevel = 1.5f;
		if (
			(hoistBothInput < 0 && _groundIndicatorBlock.position.y + _hoistFactor * hoistBothInput > groundLevel)
			|| (hoistBothInput > 0 && _groundIndicatorBlock.position.y + _hoistFactor * hoistBothInput < groundLevel + 50))
		{
			_hoistObject.position += new Vector3(0, _hoistFactor * hoistBothInput, 0);
		}
	}
	private void OnDisable()
	{
		InputControls.Crane.Disable();
		InputControls.Crane.Reset.performed -= Reset_performed;
	}
	#endregion

	private void Reset_performed(InputAction.CallbackContext obj)
	{
		_slewObject.rotation = Quaternion.identity;
		_hoistObject.localPosition = new Vector3(0, -147.5f, 0);
	}

	private void Test_performed(InputAction.CallbackContext context)
	{
		print("Test");
	}
}
