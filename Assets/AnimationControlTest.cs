using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControlTest : MonoBehaviour
{
    [SerializeField] private VRToggleButton Switch1, Switch2, Switch3, Switch4;
    [SerializeField] private VRSnapLeverButton Lever;
    private Animator animator;
    int state = 0;

    [SerializeField] private bool to1, to0, to2;

    // Start is called before the first frame update
    void Start()
    {
        Switch1.Locked = false;
        Switch2.Locked = true;
        Switch3.Locked = true;
        Switch4.Locked = true;
        Lever.Locked = true;

        animator = GetComponent<Animator>();
        animator.Play("Forwards");
    }

    // Update is called once per frame
    void Update()
    {
        Switch1.Locked = Switch2.GetCurrentState();
        Switch2.Locked = !Switch1.GetCurrentState() || Switch3.GetCurrentState();
        Switch3.Locked = !Switch2.GetCurrentState() || Switch4.GetCurrentState();

        Switch4.Locked = !Switch3.GetCurrentState() || Lever.GetCurrentState() != SnapLeverPositions.Center;
        Lever.Locked = !Switch4.GetCurrentState();

        if (state != 1 && (Lever.GetCurrentState() == SnapLeverPositions.Up || to1))
        {
            state = 1;
            animator.SetFloat("speed", 1);
        }

        if (state != 2 && (Lever.GetCurrentState() == SnapLeverPositions.Down || to2))
        {
            state = 2;
            animator.SetFloat("speed", -1);
        }

        if (state != 0 && (Lever.GetCurrentState() == SnapLeverPositions.Center || to0))
        {
            state = 0;
            animator.SetFloat("speed", 0);
        }
    }
}
