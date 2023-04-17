using System.Collections.Generic;
using UnityEngine;

namespace Code.UI.Weapons
{
    public class BulletLauncherClipUI : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> bulletIndicators;

        private void SetAllBullets(bool isEnabled)
        {
            for (int i = 0; i < bulletIndicators.Count; i++)
            {
                bulletIndicators[i].SetActive(isEnabled);
            }
        }

        public void SetAmmoRemaining(int ammoCount)
        {
            SetAllBullets(false);
            for (int i = 0; i < bulletIndicators.Count && i < ammoCount; i++)
            {
                bulletIndicators[i].SetActive(true);
            }
            
        }
        

    }
}