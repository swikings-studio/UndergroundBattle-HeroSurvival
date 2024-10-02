using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SwiKinGsStudio
{
    public static class AnimationsManager
    {
        public static Tween Soaring(Transform transform, float range, float duration)
        {
            return transform.DOLocalMoveY(transform.localPosition.y + range, duration).SetLoops(-1, LoopType.Yoyo);
        }
        public static Tween Rotating(Transform transform, Vector3 endValue, float duration)
        {
            return transform.DOLocalRotate(endValue, duration, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
    }
    public enum AnimationType
    {
        Soaring,
        Rotating,
    }
}