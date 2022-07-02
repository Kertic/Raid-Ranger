using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private BulletLauncher bulletLauncher;

        private Vector2 _movement;

        void FixedUpdate()
        {
            ApplyMovement();

            if (Input.GetButton("Fire1"))
            {
                var position = transform.position;
                bulletLauncher.Launch(Input.mousePosition - Camera.main.WorldToScreenPoint(position),position);
            }
        }

        private void ApplyMovement()
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

        void AddMovementDirection(Vector2 motion)
        {
            _movement += motion;
        }

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            Gizmos.DrawLine(position,Input.mousePosition -  Camera.main.WorldToScreenPoint(position));
        }
    }
}