using Unity.Mathematics;
using UnityEngine;

public class TimingBar : MonoBehaviour
{
    [SerializeField]
    private GameObject cursor;

    [SerializeField]
    private RectTransform barBackground, timingWindow;

    private void Start() { }

    // Update is called once per frame
    void Update() { }

    public void SetCursorPercentage(float percent)
    {
        float backgroundLeft = barBackground.rect.min.x;
        float backgroundRight = barBackground.rect.max.x;
        float cursorX = math.lerp(backgroundLeft, backgroundRight, percent);
        Vector3 newPos = cursor.transform.position;
        newPos.x = cursorX;
        cursor.transform.position = newPos;
    }

    public void SetTimingWindow(float beginningPercent, float endPercent)
    {
        float backgroundLeft = barBackground.rect.min.x;
        float backgroundRight = barBackground.rect.max.x;
        Vector2 offsetMin = timingWindow.offsetMin;
        Vector2 offsetMax = timingWindow.offsetMax;
        offsetMin.x = math.lerp(backgroundLeft, backgroundRight, beginningPercent);
        offsetMax.x = math.lerp(backgroundLeft, backgroundRight, endPercent);
        timingWindow.offsetMin = offsetMin;
        timingWindow.offsetMax = offsetMax;
    }
}