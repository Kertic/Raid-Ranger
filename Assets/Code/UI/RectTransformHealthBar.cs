using System;
using UnityEngine;

namespace Code.UI
{
    public class RectTransformHealthBar : MonoBehaviour
    {
        [SerializeField] private Transform healthBackground;
        [SerializeField] private Transform scalingGraphic;

        private RectTransform _healthBackground;
        private RectTransform _scalingGraphic;


        // Start is called before the first frame update
        void Start()
        {
            _healthBackground = healthBackground.GetComponent<RectTransform>();
            _scalingGraphic = scalingGraphic.GetComponent<RectTransform>();
            UpdateFillPercent(1);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void UpdateFillPercent(float percent)
        {
            if (percent > 1 || percent < 0)
            {
                percent = Math.Clamp(percent, 0, 1);
            }

            float width = _healthBackground.rect.width * percent;
            Rect rect = _scalingGraphic.rect;
            _scalingGraphic.sizeDelta = new Vector2(width, rect.height);
        }
    }
}