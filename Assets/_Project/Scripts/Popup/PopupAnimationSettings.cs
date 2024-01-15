using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[Serializable]
public class PopupAnimationSettings
{
    [Header("Animation Settings")] 
    public float Duration;
    public Vector3 ScaleVector;
    public LeanTweenType TweenType;
    public Action OnAnimationEnd;
    public Action OnAnimationStart;

    PopupAnimationSettings(float duration,Vector3 scaleVector, LeanTweenType tweenType, Action onAnimationEnd = null, Action onAnimationStart = null)
    {
        Duration = duration;
        ScaleVector = scaleVector;
        TweenType = tweenType;
        OnAnimationStart = onAnimationStart;
        OnAnimationEnd = onAnimationEnd;
    }
}
