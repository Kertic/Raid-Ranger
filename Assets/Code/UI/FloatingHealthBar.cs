using System;
using Unity.Mathematics;
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
                Math.Clamp(percent, 0, 1);
            }

            scalingGraphic.localScale = Vector3.Scale(new Vector3(percent, 1, 1), scalingGraphic.localScale);
        }
    }
}