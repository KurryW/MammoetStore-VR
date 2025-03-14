using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SPMT : MonoBehaviour
{
	#region "Variables"
	[SerializeField] private Transform[] _bogies;
	public IReadOnlyList<Transform> Bogies { get { return _bogies; } }
	[SerializeField] private Transform _lookAtBox = null;
	[SerializeField] private Transform _axleHelper = null;

	private InputControls _actions;
	private List<Transform> _wheels;
	private List<Transform> _lowerAxles;

	private bool _isDriving = false;
	private bool _driveAroundLookAt = false;

	float _wheelLerpDuration = 3;// sec

	private Vector3 _debugLastRotation = Vector3.zero;
	#endregion

	#region "Unity built-in methods"
	void Start()
	{
		Transform wheel;
		Transform lowerAxle;
		_wheels = new List<Transform>();
		_lowerAxles = new List<Transform>();
		foreach (var item in _bogies)
		{
			wheel = item.GetComponentsInChildren<Transform>().Where(x => x.name.StartsWith("Wheel 0")).First();
			_wheels.Add(wheel);
			lowerAxle = item.GetComponentsInChildren<Transform>().Where(x => x.name.StartsWith("LowerAxle 0")).First();
			_lowerAxles.Add(lowerAxle);
		}

		_actions = new InputControls();
		_actions.Enable();
	}
	void FixedUpdate()
	{
		if (_isDriving)
		{
			if (_driveAroundLookAt)
				transform.RotateAround(_lookAtBox.position, _lookAtBox.up, .04f);
			else
			{
				float moveChange = 1 / DriveManager.Static.MoveSpeedReduction;
				transform.position += Bogies[11].right * moveChange;
				RotateAllWheels(moveChange);
			}
			RotateAllWheels(.01f);
		}
		else
		{
			Vector2 moveChange = _actions.SPMT.Move.ReadValue<Vector2>() / DriveManager.Static.MoveSpeedReduction;
			if (moveChange != Vector2.zero)
			{
				if (_driveAroundLookAt)
					transform.RotateAround(_lookAtBox.position, _lookAtBox.up, moveChange.y * 5);
				else
					transform.position += Bogies[11].right * moveChange.y;
				RotateAllWheels(moveChange.y);
			}

			Vector2 rotateChange = _actions.SPMT.Rotate.ReadValue<Vector2>() / DriveManager.Static.TurnSpeedReduction;
			if (rotateChange != Vector2.zero)
			{
				RotateBogies(rotateChange);
			}
		}
	}
	#endregion

	public void RotateBogies(Vector2 change)
	{
		int i = 0;
		foreach (var item in _bogies)
		{
			// limit rotation to +/- 90 deg
			var rotation = item.transform.localRotation.eulerAngles.y;
			// there has to be an easier way..
			if (change.x < 0 && (rotation > 270 || rotation < 100) || (change.x > 0 && (rotation > 260 || rotation < 90)))
			{
				item.transform.localRotation *= Quaternion.Euler(0, 0, change.x);
			}
			i++;
		}
	}

	public void RotateAllWheels(float change)
	{
		foreach (var item in _wheels)
		{
			item.transform.localRotation *= Quaternion.Euler(0, change * 30, 0);
		}
	}

	public void StartDriving()
	{
		_isDriving = true;
	}

	public void StopDriving()
	{
		_isDriving = false;
	}

	public void RotateBogiesAngle(Quaternion localEndRotation)
	{
		if (_isDriving)
			return;

		_driveAroundLookAt = false;

		Transform bogie;

		for (int i = 0; i < _bogies.Length; i++)
		{
			bogie = _bogies[i];

			StartCoroutine(RotateBogieOverTime(bogie, bogie.transform.localRotation, localEndRotation));
		}
	}

	public void RotateBogiesLookAt()
	{
		if (_isDriving)
			return;

		_driveAroundLookAt = true;

		// create an helper object to use LookAt on
		Transform bogie;
		Transform axle;

		// is the indexing the same for _bogies and _wheels? Looks like it..
		for (int i = 0; i < _bogies.Length; i++)
		{
			bogie = _bogies[i];
			axle = _wheels[i].parent;

			// print(bogie.name + " " + axle.name);

			// set the helper at the same height as the axle
			_lookAtBox.position = new Vector3(_lookAtBox.position.x, axle.position.y, _lookAtBox.position.z);

			// let the axle look at the centerpoint
			_axleHelper.position = axle.parent.position;
			_axleHelper.LookAt(_lookAtBox);

			// rotate the bogie over time
			Quaternion targetRotation = _axleHelper.localRotation * Quaternion.Euler(-90, -90, 0);
			StartCoroutine(RotateBogieOverTime(bogie, bogie.transform.localRotation, targetRotation));
		}
	}

	private IEnumerator RotateBogieOverTime(Transform bogie, Quaternion startValue, Quaternion endValue)
	{
		//if (bogie.gameObject.name == "Axle 014") print("<b>started:</b> " + startValue + " " + endValue);

		float timeElapsed = 0;
		while (timeElapsed < _wheelLerpDuration)
		{
			//if (bogie.gameObject.name == "Axle 014") print(startValue.eulerAngles + " " + timeElapsed);
			bogie.localRotation = Quaternion.Lerp(startValue, endValue, timeElapsed / _wheelLerpDuration);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
	}
}
