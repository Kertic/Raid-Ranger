using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Objects
{
    public class Bullet : MonoBehaviour
    {
        private Vector2 _direction;
        [SerializeField] private float speed;
        private float _lifespan;
        [SerializeField] private float maxLifespan;

        private void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += (Vector3)_direction;
            _lifespan += Time.deltaTime;
            if (_lifespan > maxLifespan)
            {
                Destroy(gameObject);
            }
        }

        public void Spawn(Vector2 startingDirection)
        {
            _direction = startingDirection.normalized * speed;
            Debug.Log("I've been spawned with direction " + _direction);
        }
    }
}