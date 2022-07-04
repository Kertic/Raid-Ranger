using System;
using Code.Player;
using Code.UI;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Management
{
    public class GameMaster : MonoBehaviour
    {
        [SerializeField] private float arenaExtents;
        [SerializeField] private PlayerController player;
        [SerializeField] private float bossTimer;
        [SerializeField] private RectTransformHealthBar timerBar;
        [SerializeField] private int environmentDamage;
        [SerializeField] private GameObject deathScreen;
        private float _elapsedTime;

        public static GameMaster Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            deathScreen.SetActive(false);
            Time.timeScale = 1;
        }

        private void FixedUpdate()
        {
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Vector3.zero, arenaExtents);
        }
    }
}