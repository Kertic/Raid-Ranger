using System;
using Interfaces;
using UnityEngine;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour, IEntity
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float shootingSpeedModifier;
        [SerializeField] private BulletLauncher bulletLauncher;
        [SerializeField] private int maxHealth;
        [SerializeField] private float cooldown;

        private float _cooldownHeat;
        private int _currentHealth;
        private Vector2 _movement;
        private Camera _camera;
        private float _initialMoveSpeed;

        private void Start()
        {
            _camera = Camera.main;
            _currentHealth = maxHealth;
            _initialMoveSpeed = moveSpeed;

            _cooldownHeat = 0;
        }

        void FixedUpdate()
        {
            Vector3 position = transform.position;
            if (Input.GetButton("Fire1"))
            {
                if (_cooldownHeat <= 0)
                {
                    bulletLauncher.Launch(Input.mousePosition - _camera.WorldToScreenPoint(position), position);
                    _cooldownHeat = cooldown;
                }
            }


            if (_cooldownHeat > 0)
            {
                moveSpeed = _initialMoveSpeed * shootingSpeedModifier;
                _cooldownHeat -= Time.deltaTime;
            }
            else
            {
                moveSpeed = _initialMoveSpeed;
            }

            ApplyMovement();
        }

        private void ApplyMovement()
        {
            _movement = Vector2.zero;
            if (Input.GetAxis("up") > 0)
            {
                AddMovementDirection(Vector2.right);
            }

            if (Input.GetAxis("up") < 0)
            {
                AddMovementDirection(Vector2.left);
            }

            if (Input.GetAxis("right") > 0)
            {
                AddMovementDirection(Vector2.up);
            }

            if (Input.GetAxis("right") < 0)
            {
                AddMovementDirection(Vector2.down);
            }

            transform.position += (Vector3)_movement.normalized * moveSpeed;
        }

        void AddMovementDirection(Vector2 motion)
        {
            _movement += motion;
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                var position = transform.position;
                Gizmos.DrawLine(position, Input.mousePosition - _camera.WorldToScreenPoint(position));
            }
        }

        int IEntity.GetCurrentHealth()
        {
            return _currentHealth;
        }

        int IEntity.GetMaxHealth()
        {
            return maxHealth;
        }

        void IEntity.TakeDamage(int damage)
        {
            _currentHealth -= Math.Min(_currentHealth, damage);
        }
    }
}