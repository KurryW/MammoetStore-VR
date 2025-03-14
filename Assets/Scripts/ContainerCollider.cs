using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ContainerCollider : MonoBehaviour
{
	private MeshRenderer _renderer;
    public XRBaseController leftController;
    public XRBaseController rightController;

    private void Awake()
	{
		_renderer = GetComponent<MeshRenderer>();
		OnCollisionExit(null);

    }
	private void OnCollisionEnter(Collision collision)
	{
		_renderer.material.color = new Color(
			_renderer.material.color.r,
			_renderer.material.color.g,
			_renderer.material.color.b,
			.5f);
 
		// Trigger haptic feedback on both controllers 
		if (leftController != null)
		{
			leftController.SendHapticImpulse(0.5f, 0.3f); // Intensity and duration
		}

		if (rightController != null)
		{
			rightController.SendHapticImpulse(0.5f, 0.5f); // Intensity and duration
		}


	}

	private void OnCollisionExit(Collision collision)
	{
		_renderer.material.color = new Color(
				_renderer.material.color.r,
				_renderer.material.color.g,
				_renderer.material.color.b,
				0);
	}
}
