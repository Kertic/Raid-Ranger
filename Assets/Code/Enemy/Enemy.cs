using System;
using Code.Objects;
using Code.UI;
using Interfaces;
using UnityEngine;

namespace Code.Enemy
{
    public class Enemy : MonoBehaviour, IEntity
    {
        [SerializeField] private FloatingHealthBar myHealth;
        [SerializeField] private int maxHealth;
        private int _currentHealth;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("PlayerBullet"))
            {
                Bullet collidingBullet = col.gameObject.GetComponent<Bullet>();
                ((IEntity)this).TakeDamage(collidingBullet.GetDamage());
            }
        }

        private void Start()
        {
            _currentHealth = maxHealth;
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
            Debug.Log("My hp is: " + _currentHealth);
            myHealth.UpdateHealthPercent((float)_currentHealth / (float) maxHealth);
        }
    }
}