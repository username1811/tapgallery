using Milo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TilePush
{
    public class FingerTutorial : MonoBehaviour
    {
        [SerializeField] private ScaleDOTween scaleTween;
        public void DestroySelf()
        {
            scaleTween.StopTweening();
            Destroy(gameObject);
        }
    }
}