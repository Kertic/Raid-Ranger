using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.UI
{
    public class FloatingHealthBar : MonoBehaviour
    {
        [SerializeField] private Transform healthBackground;
        [SerializeField] private Transform scalingGraphic;

        // Start is called before the first frame update
        void Start()
        {
            UpdateHealthPercent(1);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void UpdateHealthPercent(float percent)
        {
            if (percent > 1 || percent < 0)
            {
                percent = Math.Clamp(percent, 0, 1);
            }

            Vector3 localScale = scalingGraphic.localScale;
            localScale = new Vector3(percent, localScale.y, localScale.z);
            scalingGraphic.localScale = localScale;
        }
    }
}