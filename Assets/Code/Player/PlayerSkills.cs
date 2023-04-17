using Code.Management;
using Code.Player.Skills;
using Code.Player.Skills.SkillSets;
using Unity.Mathematics;
using UnityEngine;

namespace Code.Player
{
    public class PlayerSkills : MonoBehaviour
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
        private PlayerController myPlayerController;

        private SkillState[] skillStates = new SkillState[(int)SkillSlot.NUMOFSKILLSLOTS];
        private float[] activeDurationRemaining = new float[(int)SkillSlot.NUMOFSKILLSLOTS];
        private float[] cooldownRemaining = new float[(int)SkillSlot.NUMOFSKILLSLOTS];

        private void Start()
        {
            PlayerController.PlayerUpdate += SkillsUpdate;
            myPlayerController = GameMaster.Instance.GetPlayer();
        }

        public void PressSkill(SkillSlot skillType)
        {
            Skill[] skills = skillSet.GetSkills();
            int index = (int)skillType;
            if (skillStates[index] == SkillState.READY)
            {
                skills[index].Execute(myPlayerController);
                skillStates[index] = SkillState.ACTIVE;
                activeDurationRemaining[index] = skills[index].activeDuration;
                cooldownRemaining[index] = skills[index].cooldown;
            }
        }

        public void SkillsUpdate(PlayerController playerController)
        {
            for (int i = 0; i < (int)SkillSlot.NUMOFSKILLSLOTS; i++)
            {
                switch (skillStates[i])
                {
                    case SkillState.ACTIVE:
                        activeDurationRemaining[i] = math.max(activeDurationRemaining[i] -= Time.deltaTime, 0.0f);
                        if (activeDurationRemaining[i] <= 0)
                        {
                            skillStates[i] = SkillState.COOLING;
                            skillSet.GetSkills()[i].Cleanup(playerController);
                        }

                        break;
                    case SkillState.COOLING:
                        cooldownRemaining[i] = math.max(cooldownRemaining[i] -= Time.deltaTime, 0.0f);
                        if (cooldownRemaining[i] <= 0)
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
            return math.max(activeDurationRemaining[(int)skillType], 0.0f);
        }

        public float GetTimeUntilReadyToUse(SkillSlot skillType)
        {
            return math.max(activeDurationRemaining[(int)skillType]+cooldownRemaining[(int)skillType], 0.0f);
        }

        public float GetCooldownRemaining(SkillSlot skillType)
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