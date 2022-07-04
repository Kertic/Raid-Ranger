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
                collidingBullet.OnHitTarget(GetComponent<Collider2D>());
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

        public virtual void TakeDamage(int damage)
        {
            _currentHealth -= Math.Min(_currentHealth, damage);
            if (myHealth != null)
            {
                myHealth.UpdateHealthPercent((float)_currentHealth / (float)maxHealth);
            }
        }
    }
}