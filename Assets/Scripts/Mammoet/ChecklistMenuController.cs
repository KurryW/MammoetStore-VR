using Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChecklistMenuController : MonoBehaviour
{
    [SerializeField] private Transform trackedController;
    [SerializeField] private float menuOpenCloseTime = 0.50f;
    [SerializeField] private float openMenuScale = 1;
    [SerializeField] private float closedMenuScale = 0;

    private Tween openMenuTween;
    private bool open = false;
    private bool canOpenClose = true;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;

        openMenuTween = new Tween(Easing.EaseInBack);

        openMenuTween.onChange = (t) =>
        {
            transform.localScale = Vector3.one * t;
        };
    }

    void Update()
    {
        if(!openMenuTween.Active)
        {
            if(open && trackedController.transform.forward.y < 0)
            {
                //menu is open, so open->closed
                openMenuTween.Start(openMenuScale, closedMenuScale, menuOpenCloseTime);
            }
            else if(!open && trackedController.transform.forward.y > 0)
            {
                //menu is closed, so closed->open
                openMenuTween.Start(closedMenuScale, openMenuScale, menuOpenCloseTime);
            }
        }
    }
}
