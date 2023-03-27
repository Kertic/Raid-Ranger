using Code.Player.Interfaces;
using UnityEngine;

namespace Code.Player.Weapons
{
    public abstract class Weapon : MonoBehaviour, IPlayerUpdatable
    {
        public abstract void Attack(PlayerController playerController);
        public abstract void SecondaryAttack(PlayerController playerController);
        public abstract void PlayerFixedUpdate(PlayerController playerController);
    }
}