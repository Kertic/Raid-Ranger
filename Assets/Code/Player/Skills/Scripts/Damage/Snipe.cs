using Code.Player.Weapons;
using UnityEngine;

namespace Code.Player.Skills.Scripts.Damage
{
    [CreateAssetMenu(fileName = "Snipe", menuName = "ScriptableObjects/Skill/Snipe", order = 1)]
    public class Snipe : Skill
    {
        public override void OnCastStart(PlayerController playerController)
        {
            playerController.PlayerMovement.SetMovementSpeed(0.01f);
        }

        public override void OnCastFinish(PlayerController playerController)
        {
            Weapon weapon = playerController.Weapon;

            if (weapon.GetType() != typeof(BulletLauncher))
            {
                return;
            }

            ((BulletLauncher)weapon).Snipe(playerController);
        }

        public override void OnActiveEnd(PlayerController playerController)
        {
            playerController.PlayerMovement.ResetMovementSpeed();
        }
    }
}