using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TilePush
{
    public class RectTransformFitScreen : MonoBehaviour
    {
        [SerializeField] private bool fitFromAwake;
        [SerializeField] private Vector2 resolution;

        private void Awake()
        {
            if (fitFromAwake) Fit();
        }

        public void Fit()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            float screenRatio = (float)Screen.width / Screen.height;
            float baseRatio = resolution.x / resolution.y;

            if (screenRatio > baseRatio)
            {
                // 3/4,...
                Vector2 sizeDelta = rectTransform.sizeDelta;
                sizeDelta.y = resolution.y;
                sizeDelta.x = resolution.x * (screenRatio / baseRatio);
                rectTransform.sizeDelta = sizeDelta;
            }
            else
            {
                // 9/20, 9/21,...
                Vector2 sizeDelta = rectTransform.sizeDelta;
                sizeDelta.x = resolution.x;
                sizeDelta.y = resolution.y * (baseRatio / screenRatio);
                rectTransform.sizeDelta = sizeDelta;
            }
        }
    }

}