//#if DOTWEEN_INSTALLED
using UnityEngine;
using DG.Tweening;

namespace Milo
{
    public class ScaleDOTween : MonoBehaviour
    {
        private static readonly Vector3 DEFAULT_SCALE = Vector3.one * 1.2f;
        private const float DEFAULT_DURATION = 0.75f;

        [SerializeField] private bool tweenOnEnable;
        [SerializeField] private float duration;
        [SerializeField] private Vector3 scale;

        private Vector3 startValue;
        private Vector3 endValue;
        private Transform tf;
        private Tween scaleTween;

        private void Awake()
        {
            tf = transform;
            startValue = tf.localScale;

            if (scale.sqrMagnitude < 0.01f) scale = DEFAULT_SCALE;
            endValue = Vector3.Scale(startValue, scale);
            if (Mathf.Approximately(duration, 0f)) duration = DEFAULT_DURATION;
        }

        private void OnEnable()
        {
            if (tweenOnEnable)
            {
                StartTweening();
            }
        }

        public void StartTweening()
        {
            if (scaleTween != null) scaleTween.Kill();
            tf.localScale = startValue;
            scaleTween = tf.DOScale(endValue, duration);
            scaleTween.SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        public void StopTweening()
        {
            scaleTween.Kill();
            //tf.localScale = startValue;
        }
    }
}
//#endif
