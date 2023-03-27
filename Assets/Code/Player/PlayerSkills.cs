using System;
using Code.Player.Interfaces;
using Code.Player.Skills;
using Code.Player.Skills.SkillSets;
using Unity.Mathematics;
using UnityEngine;

namespace Code.Player
{
    public class PlayerSkills : MonoBehaviour, IPlayerUpdatable
    {
        public enum SkillSlot
        {
            PRIMARY,
            SECONDARY,
            UTILITY,
            ULTIMATE,
            NUMOFSKILLSLOTS
        }

        public enum SkillState
        {
            READY,
            ACTIVE,
            COOLING,
            NUMOFSTATES
        }

        [SerializeField]
        private SkillSet skillSet;

        [SerializeField]
        private PlayerController playerController;

        private SkillState[] skillStates = new SkillState[(int)SkillSlot.NUMOFSKILLSLOTS];
        private float[] timeWhenCoolingStarts = new float[(int)SkillSlot.NUMOFSKILLSLOTS];
        private float[] timeWhenReadyAgain = new float[(int)SkillSlot.NUMOFSKILLSLOTS];

        public void PressSkill(SkillSlot skillType)
        {
            Skill[] skills = skillSet.GetSkills();
            int index = (int)skillType;
            if (skillStates[index] == SkillState.READY)
            {
                skills[index].Execute(playerController);
                skillStates[index] = SkillState.ACTIVE;
                timeWhenCoolingStarts[index] = Time.time + skills[index].activeDuration;
                timeWhenReadyAgain[index] = timeWhenCoolingStarts[index] + skills[index].cooldown;
            }
        }

        public void PlayerFixedUpdate(PlayerController playerController)
        {
            for (int i = 0; i < (int)SkillSlot.NUMOFSKILLSLOTS; i++)
            {
                switch (skillStates[i])
                {
                    case SkillState.ACTIVE:
                        if (timeWhenCoolingStarts[i] <= Time.time)
                        {
                            skillStates[i] = SkillState.COOLING;
                            skillSet.GetSkills()[i].Cleanup(playerController);
                        }

                        break;
                    case SkillState.COOLING:
                        if (timeWhenReadyAgain[i] <= Time.time)
                        {
                            skillStates[i] = SkillState.READY;
                        }

                        break;
                }
            }
        }

        public SkillState GetStateOfSkill(SkillSlot skillType)
        {
            return skillStates[(int)skillType];
        }

        public float GetActiveTimeRemaining(SkillSlot skillType)
        {
            return math.max(timeWhenCoolingStarts[(int)skillType] - Time.time, 0.0f);
        }

        public float GetTimeUntilReadyToUse(SkillSlot skillType)
        {
            return math.max(timeWhenReadyAgain[(int)skillType] - Time.time, 0.0f);
        }

        public float GetFractionOfCoolingDownProgress(SkillSlot skillType)
        {
            switch (GetStateOfSkill(skillType))
            {
                case SkillState.READY:
                    return 0.0f;
                case SkillState.ACTIVE:
                    return 1.0f;
                case SkillState.COOLING:
                    return GetTimeUntilReadyToUse(skillType) / skillSet.GetSkills()[(int)skillType].cooldown;
            }

            return 0.0f;
        }

        public Sprite GetIcon(SkillSlot skillType)
        {
            Skill skill = skillSet.GetSkills()[(int)skillType];
            return skill == null ? null : skill.Icon;
        }
    }
}