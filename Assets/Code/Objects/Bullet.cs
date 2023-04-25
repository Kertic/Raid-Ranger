using Unity.Mathematics;
using UnityEngine;

namespace Code.Objects
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float maxLifespan;
        private float _lifespan;
        private Vector2 _direction;
        [SerializeField] protected int damage;

        void FixedUpdate()
        {
            transform.Translate(_direction);
            _lifespan += Time.deltaTime;
            if (_lifespan > maxLifespan)
            {
                Destroy(gameObject);
            }
        }

        public int GetDamage()
        {
            return damage;
        }

        public void SetDamage(int newDamage)
        {
            damage = math.max(newDamage, 0);
            Debug.Log("My new damage: " + damage);
        }

        public void Spawn(Vector2 startingDirection)
        {
            _direction = startingDirection.normalized * speed;
        }

        //This function is able to be invoked when a bullet hits an entity
        public void OnHitTarget(Collider2D collidingObject)
        {
            Destroy(gameObject);
        }
    }
}