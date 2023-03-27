using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class SkillIcon : MonoBehaviour
    {
        [SerializeField]
        private RectTransform overlay;

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private Image iconRenderer;

        private Rect _originalSize;

        private void Start()
        {
            _originalSize = overlay.rect;
        }

        public SkillIcon SetIconTexture(Sprite texture)
        {
            iconRenderer.sprite = texture;
            return this;
        }

        public SkillIcon SetOverlayPercent(float ratio)
        {
            if (ratio > 1 || ratio < 0)
            {
                ratio = Math.Clamp(ratio, 0, 1);
            }

            float height = _originalSize.height * ratio;
            
            overlay.sizeDelta = new Vector2(
                _originalSize.width,
                height
            );

            return this;
        }

        public SkillIcon SetTimeRemaining(float timeRemainingInSeconds)
        {
            if (timeRemainingInSeconds <= 0)
            {
                text.text = "";
                return this;
            }
            string textToChangeTo = timeRemainingInSeconds > 5 ? ((int)timeRemainingInSeconds).ToString() : timeRemainingInSeconds.ToString("F1");
            text.text = textToChangeTo;
        
            return this;
        }
    }
}