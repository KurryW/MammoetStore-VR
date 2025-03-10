using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VRLeverButton))]
public class BlockRotateLever : MonoBehaviour
{
    private VRLeverButton lever;
    [SerializeField] private Transform blockTransform;
    [SerializeField] private float blockMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        lever = GetComponent<VRLeverButton>();
    }

    // Update is called once per frame
    void Update()
    {
        float state = lever.GetCurrentState();

        blockTransform.Rotate(new Vector3(0, state * blockMoveSpeed * Time.deltaTime, 0));
    }
}
