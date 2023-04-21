using UnityEngine;

namespace Code.Player.Skills.Scripts.Utility
{
    [CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Skill/Heal", order = 2)]
    public class Heal : Skill
    {
        [SerializeField] private int healAmount;
        [SerializeField] private float healWalkspeedMultiplier;
        public override void Execute(PlayerController playerController)
        {
            playerController.Weapon.SecondaryAttack(playerController);
            playerController.CurrentHealth += healAmount;
            playerController.PlayerMovement.SetMovementSpeed(healWalkspeedMultiplier);
        }

        public override void Cleanup(PlayerController playerController)
        {
            playerController.PlayerMovement.ResetMovementSpeed();
        }
    }
}
