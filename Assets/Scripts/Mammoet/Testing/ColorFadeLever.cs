using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VRLeverButton))]
public class ColorFadeLever : MonoBehaviour
{
    private VRLeverButton lever;
    [SerializeField] private Renderer colorBlock;
    [SerializeField] private Color colorOne, colorTwo;

    void Start()
    {
        lever = GetComponent<VRLeverButton>();
    }

    // Update is called once per frame
    void Update()
    {
        colorBlock.material.color = Color.Lerp(colorOne, colorTwo, lever.GetCurrentState());
    }
}
