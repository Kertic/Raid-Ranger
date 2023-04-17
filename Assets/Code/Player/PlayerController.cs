//#define PLAYERDEBUG

using System;
using System.Collections.Generic;
using Code.Management;
using Code.Player.Weapons;
using Code.UI;
using Interfaces;
using UnityEngine;

namespace Code.Player
{
    public delegate void PlayerEvent(PlayerController playerController);
    public class PlayerController : MonoBehaviour, IEntity
    {
        #region DebugVars

#if PLAYERDEBUG
        private bool _isTimerRunning = false;
        private double _elapsedTime = 0.0;
        private Vector2 _startingPos;
#endif

        #endregion
        
        #region Events
        /// <summary>
        /// This will be invoked each time the PlayerController invokes FixedUpdate
        /// </summary>
        public static event PlayerEvent PlayerFixedUpdate;
        /// <summary>
        /// This will not be invoked when the game is paused 
        /// </summary>
        public static event PlayerEvent PlayerUpdate;
        #endregion
        
        [Header("Classes")]
        [SerializeField]
        private Weapon weapon;

        [SerializeField]
        private PlayerMovement playerMovement;

        [SerializeField]
        private PlayerSkills skills;

        [SerializeField]
        private FloatingHealthBar playerHealthBar;

        [SerializeField]
        private RectTransformHealthBar rectHealthBar;

        [SerializeField]
        private List<SkillIcon> skillIcons = new List<SkillIcon>();

        [Header("PlayerStats")]
        [SerializeField]
        private int maxHealth;

        [SerializeField]
        private float attackSpeed = 1.0f;

        [SerializeField]
        private float redFlashDuration;

        private float _redFlashTimeRemaining, _startingAttackSpeed;
        private int _startingMaxHealth, _currentHealth;
        private Camera _camera;
        private Animator _animator;
        private static readonly int Damaged = Animator.StringToHash("Damaged");

        public Vector3 MouseWorldPosition { get; private set; } = Vector3.zero;
        public float AttackSpeed => attackSpeed;
        public PlayerMovement PlayerMovement => playerMovement;
        public PlayerSkills Skills => skills;
        public Weapon Weapon => weapon;
        public List<SkillIcon> SkillIcons => skillIcons;

        private void Start()
        {
            _camera = Camera.main;
            _currentHealth = maxHealth;
            _startingMaxHealth = maxHealth;
            _startingAttackSpeed = attackSpeed;
            _animator = GetComponent<Animator>();
            for (int i = 0; i < skillIcons.Count; i++)
            {
                PlayerSkills.SkillSlot skillSlot = (PlayerSkills.SkillSlot)i;
                skillIcons[i].SetIconTexture(Skills.GetIcon(skillSlot));
            }

            GameMaster.GameMasterUnpausedUpdate += PlayerControllerUpdate;
        }

        private void PlayerControllerUpdate()
        {
            MouseWorldPosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.gameObject.transform.position.z * -1.0f));

            for (int i = 0; i < skillIcons.Count; i++)
            {
                PlayerSkills.SkillSlot skillSlot = (PlayerSkills.SkillSlot)i;
                PlayerSkills.SkillState state = Skills.GetStateOfSkill(skillSlot);
                switch (state)
                {
                    case PlayerSkills.SkillState.READY:
                        skillIcons[i].SetTimeRemaining(0).SetOverlayPercent(0);
                        break;
                    case PlayerSkills.SkillState.ACTIVE:
                        skillIcons[i].SetTimeRemaining(Skills.GetActiveTimeRemaining(skillSlot)).SetOverlayPercent(0);
                        break;
                    case PlayerSkills.SkillState.COOLING:
                        skillIcons[i].SetTimeRemaining(Skills.GetTimeUntilReadyToUse(skillSlot)).SetOverlayPercent(Skills.GetFractionOfCoolingDownProgress(skillSlot));
                        break;
                }
            }

            if (Input.GetButton("Fire1"))
            {
                weapon.Attack(this);
            }

            if (Input.GetButtonDown("PrimarySkill"))
            {
                skills.PressSkill(PlayerSkills.SkillSlot.PRIMARY);
            }

            if (Input.GetButtonDown("MovementSkill"))
            {
                skills.PressSkill(PlayerSkills.SkillSlot.UTILITY);
            }

            PlayerMovement.StopWalking();
            if (Input.GetAxis("up") > 0)
            {
                PlayerMovement.AddWalkingDirection(Vector2.right);
            }

            if (Input.GetAxis("up") < 0)
            {
                PlayerMovement.AddWalkingDirection(Vector2.left);
            }

            if (Input.GetAxis("right") > 0)
            {
                PlayerMovement.AddWalkingDirection(Vector2.up);
            }

            if (Input.GetAxis("right") < 0)
            {
                PlayerMovement.AddWalkingDirection(Vector2.down);
            }

            if (_redFlashTimeRemaining > 0)
            {
                _redFlashTimeRemaining -= Time.deltaTime;
            }

            PlayerUpdate?.Invoke(this);
        }

        void FixedUpdate()
        {
            PlayerFixedUpdate?.Invoke(this);
            DebugLogic();
        }

        void DebugLogic()
        {
#if PLAYERDEBUG
            if (Input.GetButton("Enable Debug Button 1"))
            {
                if (!_isTimerRunning)
                {
                    _isTimerRunning = true;
                    _elapsedTime = 0;
                    _startingPos = transform.position;
                    Debug.Log("Starting Timer");
                }
            }

            if (_isTimerRunning)
            {
                _elapsedTime += Time.deltaTime;

                if (_elapsedTime >= 1)
                {
                    Vector2 endingPos = transform.position;
                    Vector2 delta = endingPos - _startingPos;
                    Debug.Log("Delta / time is: " + delta / (float)_elapsedTime + " ");
                    Debug.Log("Time Elapsed was: " + _elapsedTime);
                    GameMaster.Instance.Pause();
                    _isTimerRunning = false;
                    _elapsedTime = 0;
                }
            }

#endif
        }

        int IEntity.GetCurrentHealth()
        {
            return _currentHealth;
        }

        int IEntity.GetMaxHealth()
        {
            return maxHealth;
        }

        public void MultiplyAttackSpeed(float multiplier)
        {
            attackSpeed *= multiplier;
        }

        public void ResetAttackSpeed()
        {
            attackSpeed = _startingAttackSpeed;
        }

        public void TakeDamage(int damage)
        {
            if (_redFlashTimeRemaining <= 0)
            {
                _currentHealth -= Math.Min(_currentHealth, damage);
                playerHealthBar.UpdateHealthPercent(_currentHealth / (float)maxHealth);
                rectHealthBar.UpdateFillPercent(_currentHealth / (float)maxHealth);
                _redFlashTimeRemaining = redFlashDuration;
                _animator.SetTrigger(Damaged);
                if (_currentHealth == 0)
                {
                    GameMaster.Instance.Death();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;
            Vector3 mousePos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                _camera.gameObject.transform.position.z * -1.0f));
            Vector3 position = transform.position;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(position, mousePos);
            Gizmos.color = new Color(0, 1, 0.5f);
            Gizmos.DrawCube(mousePos, Vector3.one * 0.25f);
        }
    }
}