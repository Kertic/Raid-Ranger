using System.Collections.Generic;
using UnityEngine;

namespace Code.Player.Skills
{
    [CreateAssetMenu(fileName = "SkillsetName", menuName = "ScriptableObjects/Skillset", order = 1)]
    public class SkillsetSo : ScriptableObject
    {
        [SerializeField] private Skill skill1;
        public Skill skill2;
        public Skill skill3;
        public Skill skill4;

        public Skill[] GetSkills()
        {
            return new[] { skill1, skill2, skill3, skill4 };
        }
    }
}