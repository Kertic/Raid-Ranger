using UnityEngine;

namespace Code.Player.Skills.SkillSets
{
    [CreateAssetMenu(fileName = "SkillSetName", menuName = "ScriptableObjects/SkillSet", order = 1)]
    public class SkillSet : ScriptableObject
    {
        [SerializeField]
        private Skill primarySkill;

        [SerializeField]
        private Skill secondarySkill;

        [SerializeField]
        private Skill movementSkill;

        [SerializeField]
        private Skill ultimateSkill;

        public Skill[] GetSkills()
        {
            return new[] { primarySkill, secondarySkill, movementSkill, ultimateSkill };
        }
    }
}