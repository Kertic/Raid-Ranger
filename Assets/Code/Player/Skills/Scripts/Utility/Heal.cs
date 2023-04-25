using Code.Player.Weapons;
using UnityEngine;

namespace Code.Player.Skills.Scripts.Utility
{
    [CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Skill/Heal", order = 2)]
    public class Heal : Skill
    {
        [SerializeField] private int healAmount;
        [SerializeField] private float healWalkspeedMultiplier;
        public override void OnCastStart(PlayerController playerController)
        {
            playerController.PlayerMovement.SetMovementSpeed(healWalkspeedMultiplier);
            Weapon weap = playerController.Weapon;
            if (weap.GetType() == typeof(BulletLauncher))
            {
                ((BulletLauncher)weap).BulletCount = 0;
            }
        }

        public override void OnCastFinish(PlayerController playerController)
        {
            playerController.CurrentHealth += healAmount;
        }

        public override void OnActiveEnd(PlayerController playerController)
        {
            playerController.PlayerMovement.ResetMovementSpeed();
        }
    }
}
