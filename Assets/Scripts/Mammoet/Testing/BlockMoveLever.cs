using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VRLeverButton))]
public class BlockMoveLever : MonoBehaviour
{
    private VRLeverButton lever;
    [SerializeField] private Transform blockTransform;
    [SerializeField] private float blockMinY, blockMaxY, blockMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        lever = GetComponent<VRLeverButton>();
    }

    // Update is called once per frame
    void Update()
    {
        float state = lever.GetCurrentState();

        blockTransform.localPosition += Vector3.up * state * blockMoveSpeed * Time.deltaTime;

        if (blockTransform.localPosition.y > blockMaxY)
            blockTransform.localPosition = new Vector3(blockTransform.localPosition.x, blockMaxY, blockTransform.localPosition.z);

        if (blockTransform.localPosition.y < blockMinY)
            blockTransform.localPosition = new Vector3(blockTransform.localPosition.x, blockMinY, blockTransform.localPosition.z);
    }
}
