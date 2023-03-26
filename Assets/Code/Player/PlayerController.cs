using System;
using Code.Management;
using Code.Player.Skills;
using Code.UI;
using Interfaces;
using UnityEngine;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour, IEntity
    {
        [Header("Classes")] [SerializeField] private BulletLauncher bulletLauncher;
        [SerializeField] private FloatingHealthBar playerHealthBar;
        [SerializeField] private SkillsetSO _skills;
        [SerializeField] private int[] cooldowns;

        [Header("PlayerVariables")] [SerializeField]
        private int maxHealth;

        [SerializeField] private float moveSpeed, shootingSpeedModifier, cooldown, redFlashDuration;

        private float _cooldownHeat, _initialMoveSpeed, _redFlashTimeRemaining, _dashSpeed;
        private int _currentHealth;
        private Vector2 _movement, _dash;
        private Camera _camera;
        private Animator _animator;
        private static readonly int Damaged = Animator.StringToHash("Damaged");

        private void Start()
        {
            _camera = Camera.main;
            _currentHealth = maxHealth;
            _initialMoveSpeed = moveSpeed;
            _dashSpeed = moveSpeed;
            _cooldownHeat = 0;
            _animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            Vector3 position = transform.position;
            if (Input.GetButton("Fire"))
            {
                if (_cooldownHeat <= 0)
                {
                    bulletLauncher.Launch(Input.mousePosition - _camera.WorldToScreenPoint(position), position);
                    _cooldownHeat = cooldown;
                }
            }

            for (int i = 0; i < _skills.GetSkills().Length; i++)
            {
                if (Input.GetButton("Skill" + i))
                    _skills.GetSkills()[i].ExecuteSkill(this);
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

            if (_redFlashTimeRemaining > 0)
            {
                _redFlashTimeRemaining -= Time.deltaTime;
                if (_redFlashTimeRemaining <= 0)
                {
                }
            }

            ApplyMovement();
        }

        private void ApplyMovement()
        {
            if (_dash != Vector2.zero)
            {
                
                transform.position += (Vector3)_dash.normalized * _dashSpeed;// TODO: Figure out a way to satisfy what you intuitively know as a dash, where you pick a direction and move forward some amount rapidly that way
                
            }
            else
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
        }

        private void AddMovementDirection(Vector2 motion)
        {
            _movement += motion;
        }

        int IEntity.GetCurrentHealth()
        {
            return _currentHealth;
        }

        int IEntity.GetMaxHealth()
        {
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (_redFlashTimeRemaining <= 0)
            {
                _currentHealth -= Math.Min(_currentHealth, damage);
                playerHealthBar.UpdateHealthPercent(_currentHealth / (float)maxHealth);
                _redFlashTimeRemaining = redFlashDuration;
                _animator.SetTrigger(Damaged);
                if (_currentHealth == 0)
                {
                    GameMaster.Instance.Death();
                }
            }
        }

        public PlayerController Dash(Vector2 dashDirection, float movespeedMultiplier)
        {
            _dash = dashDirection;
            _dashSpeed = moveSpeed * movespeedMultiplier;

            return this;
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                var position = transform.position;
                Gizmos.DrawLine(position, Input.mousePosition - _camera.WorldToScreenPoint(position));
            }
        }
    }
}