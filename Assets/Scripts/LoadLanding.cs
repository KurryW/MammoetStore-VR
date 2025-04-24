using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLanding : MonoBehaviour
{
    #region "Variables"
    private MeshRenderer _renderer;

    public GameObject UIfinish;
    #endregion

    #region "Unity built-in methods"
    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }
    private void FixedUpdate()
    {
        if (_firstOn && _secondOn)
        {
            _renderer.sharedMaterial.color = new Color(
            _renderer.sharedMaterial.color.r,
            _renderer.sharedMaterial.color.g,
            _renderer.sharedMaterial.color.b,
            .5f);

            UIfinish.SetActive(true);
        }
        else
        {
            _renderer.sharedMaterial.color = new Color(
            _renderer.sharedMaterial.color.r,
            _renderer.sharedMaterial.color.g,
            _renderer.sharedMaterial.color.b,
            0);
        }

        UIfinish.SetActive(false);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "OutriggerColliderA")
            _firstOn = true;
        if (collision.collider.name == "OutriggerColliderB")
            _secondOn = true;

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.name == "OutriggerColliderA")
            _firstOn = false;
        if (collision.collider.name == "OutriggerColliderB")
            _secondOn = false;
    }
    #endregion

    bool _firstOn = false;
    bool _secondOn = false;
}