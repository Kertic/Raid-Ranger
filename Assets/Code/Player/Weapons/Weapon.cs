using UnityEngine;

namespace Code.Player.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract void Attack(PlayerController playerController);
        public abstract void SecondaryAttack(PlayerController playerController);
        public abstract void BulletLauncherUpdate(PlayerController playerController);
    }
}