using Unity.Mathematics;
using UnityEngine;

namespace Code.UI
{
    public class TimingBar : MonoBehaviour
    {
        [SerializeField]
        private RectTransform barBackground, timingWindow, cursor;

        public void SetCursorPercentage(float percent)
        {
            Rect rect = barBackground.rect;
            float leftBound = barBackground.localPosition.x - (barBackground.rect.width / 2.0f);
            float rightBound = barBackground.localPosition.x + (barBackground.rect.width / 2.0f);
            Vector3 newPos = cursor.localPosition;
            newPos.x = math.lerp(leftBound, rightBound, percent);
            cursor.localPosition = newPos;
        }

        public void SetTimingWindow(float beginningPercent, float endPercent)
        {
            Vector2 offsetMin = timingWindow.offsetMin;
            Vector2 offsetMax = timingWindow.offsetMax;
            offsetMin.x = math.lerp(0, barBackground.rect.width, beginningPercent);
            offsetMax.x = math.lerp(barBackground.rect.width, 0, endPercent) * -1.0f;
            timingWindow.offsetMin = offsetMin;
            timingWindow.offsetMax = offsetMax;
        }
    }
}