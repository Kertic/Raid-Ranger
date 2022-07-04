using System;
using Code.Management;
using Code.Player;
using Interfaces;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

namespace Code.Enemy
{
    public class Chaser : Enemy, IEntity
    {
        [SerializeField] private float maxAggro, aggroCooldownRate, moveSpeed;
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
                _aggro -= aggroCooldownRate;

                MoveToPlayer();
            }
        }

        

        void MoveToPlayer()
        {
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            direction.z = 0;
            transform.Translate(direction * (moveSpeed));
            _lastMovementDirection = direction;
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _aggro = Math.Max(damage + _aggro, maxAggro);
            Debug.Log("My Aggro is at: " + _aggro);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _lastMovementDirection);
        }
    }
}