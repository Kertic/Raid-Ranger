//#define PLAYERDEBUG

using System;
using Code.Management;
using Code.Player.Skills.Skillsets;
using Code.UI;
using Interfaces;
using UnityEngine;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour, IEntity
    {
        #region DebugVars

#if PLAYERDEBUG
        private bool _isTimerRunning = false;
        private double _elapsedTime = 0.0;
        private Vector2 _startingPos;
#endif

        #endregion

        public static PlayerController Instance { get; private set; }

        [Header("Classes")]
        [SerializeField]
        private BulletLauncher bulletLauncher;

        [SerializeField]
        private FloatingHealthBar playerHealthBar;

        [SerializeField]
        private SkillSet skills;

        [Header("PlayerVariables")]
        [SerializeField]
        private int maxHealth;

        [SerializeField]
        private float moveSpeed,
            shootingSpeedModifier,
            cooldown, //TODO: Cooldown should probably stem from the weapon, not the player. Not every weapon is like that.
            redFlashDuration,
            dashSpeed; //TODO: When refactoring dashing into skills, remove this

        private float _cooldownHeat;
        private float _redFlashTimeRemaining;
        private int _currentHealth;
        private Vector2 _movement, _forcedMovement;
        private Camera _camera;
        private Animator _animator;
        private static readonly int Damaged = Animator.StringToHash("Damaged");

        public Vector3 MouseWorldPosition { get; private set; } = Vector3.zero;
        public float InitialMoveSpeed { get; private set; }


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            _camera = Camera.main;
            _currentHealth = maxHealth;
            InitialMoveSpeed = moveSpeed;
            _cooldownHeat = 0;
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            Vector3 position = transform.position;
            MouseWorldPosition = _camera.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    _camera.gameObject.transform.position.z * -1.0f));

            if (Input.GetButton("Fire1"))
            {
                if (_cooldownHeat <= 0)
                {
                    bulletLauncher.Launch(MouseWorldPosition - position, position);
                    _cooldownHeat = cooldown;
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                skills.GetSkills()[0].Execute();
            }
        }

        void FixedUpdate()
        {
            ApplyPhysics();
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

        private void ApplyPhysics()
        {
            if (_forcedMovement != Vector2.zero)
            {
                Vector2 forcedTravel = _forcedMovement.normalized * (moveSpeed * Time.deltaTime);
                if (forcedTravel.magnitude >= _forcedMovement.magnitude)
                {
                    transform.position += (Vector3)_forcedMovement;
                    _forcedMovement = Vector2.zero;
                }
                else
                {
                    transform.position += (Vector3)forcedTravel;
                    _forcedMovement -= forcedTravel;
                }
            }
            else
            {
                if (_cooldownHeat > 0)
                {
                    moveSpeed = InitialMoveSpeed * shootingSpeedModifier;
                    _cooldownHeat -= Time.deltaTime;
                }
                else
                {
                    moveSpeed = InitialMoveSpeed;
                }

                if (_redFlashTimeRemaining > 0)
                {
                    _redFlashTimeRemaining -= Time.deltaTime;
                    if (_redFlashTimeRemaining <= 0)
                    {
                    }
                }

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

                transform.position += (Vector3)_movement.normalized * (moveSpeed * Time.deltaTime);
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

        public PlayerController TravelToPoint(Vector2 destination, float duration)
        {
            moveSpeed = Vector2.Distance(destination, transform.position) / duration;
            _forcedMovement = destination - (Vector2)transform.position;
            return this;
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Vector3 mousePos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    _camera.gameObject.transform.position.z * -1.0f));
                Vector3 position = transform.position;
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(position, mousePos);
                Gizmos.color = new Color(0, 1, 0.5f);
                Gizmos.DrawCube(mousePos, Vector3.one * 0.25f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(position, position + (Vector3)_forcedMovement);
            }
        }
    }
}