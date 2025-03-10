using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VRToggleButton))]
public class ColorToggleButton : MonoBehaviour
{
    [SerializeField] private Material red, green;
    [SerializeField] private Renderer colorRenderer;
    private VRToggleButton vrButton;

    private void Start()
    {
        vrButton = GetComponent<VRToggleButton>();
    }

    void Update()
    {
        colorRenderer.material = vrButton.GetCurrentState() ? green : red;
    }
}
