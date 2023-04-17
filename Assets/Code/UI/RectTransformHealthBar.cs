using System;
using UnityEngine;

namespace Code.UI
{
    public class RectTransformHealthBar : MonoBehaviour
    {
        [SerializeField]
        private RectTransform healthBackground;

        [SerializeField]
        private RectTransform scalingGraphic;


        void Start()
        {
            UpdateFillPercent(1);
        }

        public void UpdateFillPercent(float percent)
        {
            if (percent > 1 || percent < 0)
            {
                percent = Math.Clamp(percent, 0, 1);
            }

            float width = healthBackground.rect.width * percent;
            scalingGraphic.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,width);
        }
    }
}