using System;
using Code.Player;
using Code.UI;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Management
{
    public delegate void GameMasterEvent();

    public class GameMaster : MonoBehaviour
    {
        [SerializeField]
        private float arenaExtents;

        [SerializeField]
        private PlayerController player;

        [SerializeField]
        private float bossTimer;

        [SerializeField]
        private RectTransformHealthBar timerBar;

        [SerializeField]
        private int environmentDamage;

        [SerializeField]
        private GameObject deathScreen, pauseMenu;

        private float _elapsedTime;
        public bool IsPaused { get; private set; }

        public static GameMaster Instance;
        public static event GameMasterEvent GameMasterFixedUpdate;
        public static event GameMasterEvent GameMasterUpdate;
        public static event GameMasterEvent GameMasterUnpausedUpdate;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            deathScreen.SetActive(false);
            Time.timeScale = 1;
        }

        private void Start()
        {
            pauseMenu.SetActive(IsPaused);
        }

        private void FixedUpdate()
        {
            if (IsPaused)
            {
                GameMasterFixedUpdate?.Invoke();
                return;
            }

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= bossTimer)
            {
                SceneManager.LoadScene("End Screen");
                return;
            }

            timerBar.UpdateFillPercent(1.0f - (_elapsedTime / bossTimer));
            float distanceFromCenter = Vector3.Distance(player.transform.position, Vector3.zero);
            if (distanceFromCenter > arenaExtents)
            {
                PlayerOutOfBounds();
            }

            GameMasterFixedUpdate?.Invoke();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                Pause();
            }

            GameMasterUpdate?.Invoke();
            if (!IsPaused)
            {
                GameMasterUnpausedUpdate?.Invoke();
            }
        }

        public void HealBoss(float percentOfHealthGainedBack)
        {
            float healthValue = percentOfHealthGainedBack * bossTimer;
            _elapsedTime = Math.Max(_elapsedTime - healthValue, 0.0f);
        }

        public void Death()
        {
            Time.timeScale = 0;
            deathScreen.SetActive(true);
        }

        public PlayerController GetPlayer()
        {
            return player;
        }

        void PlayerOutOfBounds()
        {
            ((IEntity)player).TakeDamage(environmentDamage);
        }

        public void Pause()
        {
            IsPaused = !IsPaused;
            pauseMenu.SetActive(IsPaused);
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Vector3.zero, arenaExtents);
        }
    }
}