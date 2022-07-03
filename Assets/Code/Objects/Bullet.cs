using UnityEngine;
using UnityEngine.Serialization;

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

        public void Spawn(Vector2 startingDirection)
        {
            _direction = startingDirection.normalized * speed;
        }
    }
}