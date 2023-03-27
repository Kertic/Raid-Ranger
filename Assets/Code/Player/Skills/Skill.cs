using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.Player.Skills
{
    [CreateAssetMenu(fileName = "SkillName", menuName = "ScriptableObjects/Skill", order = 1)]
    public class Skill : ScriptableObject
    {
        [SerializeField]
        private Image icon;

        [SerializeField]
        private float cooldown;

        [SerializeField]
        private UnityEvent execute;

        public float Cooldown => cooldown;


        public void Execute()
        {
            execute.Invoke();
        }
    }
}