using UnityEngine;

public class TweenController : MonoBehaviour
{
    void Update()
    {
        Tweening.Tween.UpdateTweens(Time.deltaTime);
    }
}
