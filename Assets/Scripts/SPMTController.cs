using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPMTController : MonoBehaviour
{
	#region "Variables"
	[SerializeField] private GameObject _headCamera = null;
	#endregion

	#region "Unity built-in methods"
	private void Start() { }
	private void Update()
	{
		// set the height below the camera and a bit to the front
		transform.position = _headCamera.transform.position - new Vector3(0, 0.5f, 0);

		// set the rotation 0 to the floor
		transform.rotation = Quaternion.Euler(0, _headCamera.transform.eulerAngles.y, 0);

	}
	#endregion


}
