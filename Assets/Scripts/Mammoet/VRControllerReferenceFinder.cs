using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerReferenceFinder : MonoBehaviour
{
    public static GameObject LeftObject, RightObject, HeadObject;
    [SerializeField] private GameObject leftController, rightController, hmd;

    private void Start()
    {
        LeftObject = leftController;
        RightObject = rightController;
        HeadObject = hmd;
    }
}
