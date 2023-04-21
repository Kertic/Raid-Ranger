using UnityEngine;
using UnityEngine.UI;

namespace Code.Player.Skills
{
    public abstract class Skill : ScriptableObject
    {
        [SerializeField]
        private Sprite icon;

        public float cooldown;
        public float activeDuration;
        public float castTime;
        public Sprite Icon => icon;

        public abstract void Execute(PlayerController playerController);
        public abstract void Cleanup(PlayerController playerController);
    }
}