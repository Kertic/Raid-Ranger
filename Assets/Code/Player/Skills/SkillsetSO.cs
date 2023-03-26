using System.Collections;
using System.Collections.Generic;
using Code.Player.Skills;
using UnityEngine;


[CreateAssetMenu(fileName = "SkillsetName", menuName = "ScriptableObjects/Skillset", order = 1)]
public class SkillsetSO : ScriptableObject
{
    [SerializeReference] private SkillAbstract skill1;
    public SkillAbstract skill2;
    public SkillAbstract skill3;
    public SkillAbstract skill4;

    public SkillAbstract[] GetSkills()
    {
        return new[] { skill1, skill2, skill3, skill4 };
    }
}