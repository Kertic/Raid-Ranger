using UnityEngine;

namespace Code.Player.Skills.Scripts.Damage
{
    [CreateAssetMenu(fileName = "AttackSpeedAmp", menuName = "ScriptableObjects/Skill/AttackSpeedAmp", order = 1)]
    public class AttackSpeedAmp : Skill
    {
        [SerializeField]
        private float attackSpeedMultiplier;
        public override void Execute(PlayerController playerController)
        {
            playerController.MultiplyAttackSpeed(attackSpeedMultiplier);
        }

        public override void Cleanup(PlayerController playerController)
        {
            playerController.ResetAttackSpeed();
        }
    }
}
