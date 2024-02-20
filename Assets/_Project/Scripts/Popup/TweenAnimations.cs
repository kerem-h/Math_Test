using System;
using UnityEngine;

public static class TweenAnimations
{
    public static void PopUpAnimation(GameObject panel, PopupAnimationSettings settings)
    {
        var duration = settings.Duration;
        var tweenType = settings.TweenType;
        var onStart = settings.OnAnimationStart;
        var onEnd = settings.OnAnimationEnd;
        var scaleVector = settings.ScaleVector;
        LeanTween.scale(panel, scaleVector, duration).setOnStart(onStart).setOnComplete(onEnd).setLoopType(tweenType).setLoopOnce();
    }
}
