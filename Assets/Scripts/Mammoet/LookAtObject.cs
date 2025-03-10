using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    public Transform Target;

    void Update()
    {
        transform.LookAt(Target, Target.up);
    }
}
