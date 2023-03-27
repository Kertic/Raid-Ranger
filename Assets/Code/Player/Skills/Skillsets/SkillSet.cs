using UnityEngine;

namespace Code.Player.Skills.Skillsets
{
    [CreateAssetMenu(fileName = "SkillSetName", menuName = "ScriptableObjects/SkillSet", order = 1)]
    public class SkillSet : ScriptableObject
    {
        [SerializeField] private Skill skill1;
        [SerializeField] private Skill skill2;
        [SerializeField] private Skill skill3;
        [SerializeField] private Skill skill4;

        public Skill[] GetSkills()
        {
            return new[] { skill1, skill2, skill3, skill4 };
        }
    }
}