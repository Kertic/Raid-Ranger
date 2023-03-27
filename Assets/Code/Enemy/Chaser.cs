using System;
using Code.Management;
using Code.Player;
using Code.UI;
using Interfaces;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

namespace Code.Enemy
{
    public class Chaser : Enemy
    {
        [Header("ExtraChaserClasses")] [SerializeField]
        private FloatingHealthBar aggroBar;

        [SerializeField] private FloatingChatBox chatBox;

        [SerializeField] [Header("ChaserVariables")]
        private int touchDamage;

        [SerializeField] 

        private float maxAggro, percentToHealBossOnTouch;

        [SerializeField] private float aggroCooldownRate, moveSpeed;

        private PlayerController _player;
        private float _aggro;

        private Vector3 _lastMovementDirection;

        void Start()
        {
            _player = GameMaster.Instance.GetPlayer();
            _aggro = maxAggro;
        }

        private void FixedUpdate()
        {
            if (_aggro > 0)
            {
                chatBox.SetText("I'm gonna get you, Ranger!");
                _aggro -= aggroCooldownRate;

                MoveToPlayer();
                aggroBar.UpdateHealthPercent((_aggro / maxAggro));
            }
            else
            {
                chatBox.SetText("I'm helpin ya Boss!");
                MoveToBoss();
            }
        }


        void MoveToPlayer()
        {
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            direction.z = 0;
            transform.Translate(direction * (moveSpeed));
            _lastMovementDirection = direction;
        }

        void MoveToBoss()
        {
            Vector3 direction = (Vector3.zero - transform.position).normalized;
            transform.Translate(direction * (moveSpeed * 1.5f));
            _lastMovementDirection = direction;
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _aggro = Math.Min(damage + _aggro, maxAggro);
        }


        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage(touchDamage);
            }
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            base.OnTriggerEnter2D(col);
            if (col.gameObject.CompareTag("Boss"))
            {
                GameMaster.Instance.HealBoss(percentToHealBossOnTouch);
                _aggro = maxAggro;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _lastMovementDirection);
        }
    }
}