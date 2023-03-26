using UnityEngine.UI;

namespace Code.Player.Skills
{
    [System.Serializable]
    public abstract class SkillAbstract
    {
        private Image icon;

        public abstract void ExecuteSkill(PlayerController playerController);
    }

}
