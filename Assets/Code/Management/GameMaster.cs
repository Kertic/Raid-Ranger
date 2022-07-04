using System;
using Code.Player;
using Code.UI;
using Interfaces;
using UnityEngine;

namespace Code.Management
{
    public class GameMaster : MonoBehaviour
    {
        [SerializeField] private float arenaExtents;
        [SerializeField] private PlayerController player;
        [SerializeField] private float bossTimer;
        [SerializeField] private RectTransformHealthBar timerBar;
        [SerializeField] private float environmentDamage;
        private float _elapsedTime;
        
        public static GameMaster Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void FixedUpdate()
        {
            _elapsedTime += Time.deltaTime;
            timerBar.UpdateFillPercent( 1.0f - (_elapsedTime / bossTimer));
            float distanceFromCenter = Vector3.Distance(player.transform.position, Vector3.zero);
            if (distanceFromCenter > arenaExtents)
            {
                PlayerOutOfBounds();
            }
        }


        public PlayerController GetPlayer()
        {
            return player;
        }

        void PlayerOutOfBounds()
        {
            ((IEntity)player).TakeDamage(10);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Vector3.zero, arenaExtents);
        }
    }
}