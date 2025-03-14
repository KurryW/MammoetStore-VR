using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMDManager : MonoBehaviour
{
	#region "Variables"
	#endregion

	#region "Unity built-in methods"
	private void Start() {
		print(XRSettings.loadedDeviceName);
		if (XRSettings.isDeviceActive && XRSettings.loadedDeviceName == "MockHMD Display") 
			print("MOCK SET ACTIVE!");
	}
	private void Update() { }
	#endregion


}
