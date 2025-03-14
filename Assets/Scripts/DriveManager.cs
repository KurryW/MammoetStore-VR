using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class DriveManager : MonoBehaviour
{
	#region "Variables"
	public static DriveManager Static { get; private set; }

	[SerializeField] private float _moveSpeedReduction = 80;
	public float MoveSpeedReduction { get { return _moveSpeedReduction; }}
	[SerializeField] private float _turnSpeedReduction = 8;
	public float TurnSpeedReduction { get { return _turnSpeedReduction; }}
	[SerializeField] private SPMT _spmt = null;
	public SPMT SPMT { get { return _spmt; } }
	[SerializeField] private XROrigin _xrOrigin = null;

	#endregion

	#region "Unity built-in methods"
	private void Awake()
	{
		if (Static == null) Static = this;
		else Destroy(this.gameObject);
	}

	private void Update()
	{
		
	}
	#endregion

	public void OnTeleport(int anchorIndex)
	{
		switch (anchorIndex)
		{
			case 0:
			case 1:
				_xrOrigin.transform.SetParent(null, false);
				break;
			case 2:
				_xrOrigin.transform.SetParent(_spmt.transform, true);
				break;
			default:
				break;
		}
	}
}
