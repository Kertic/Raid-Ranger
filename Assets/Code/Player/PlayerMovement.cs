using Code.Player.Interfaces;
using UnityEngine;

namespace Code.Player
{
    public class PlayerMovement : MonoBehaviour, IPlayerUpdatable, IPlayerInitializable
    {
        public float InitialMoveSpeed { get; private set; }
        public Vector2 WalkingMovement => _walkingMovement;

        [SerializeField]
        private float moveSpeed;

        private float _forcedMoveSpeed;

        [SerializeField]
        private PlayerController _playerController;

        private Vector2 _walkingMovement, _forcedMovement;

        public void PlayerInitialize(PlayerController playerController)
        {
            InitialMoveSpeed = moveSpeed;
        }

        private void ApplyPhysics()
        {
            if (_forcedMovement != Vector2.zero)
            {
                Vector2 forcedTravel = _forcedMovement.normalized * (_forcedMoveSpeed * Time.deltaTime);
                Vector2 forcedTranslation = forcedTravel.magnitude >= _forcedMovement.magnitude ? _forcedMovement : forcedTravel;
                transform.position += (Vector3)forcedTranslation;
                _forcedMovement -= forcedTranslation;
                
                if (_forcedMovement == Vector2.zero)
                {
                    _forcedMoveSpeed = 0.0f;
                }
            }
            else
            {
                transform.position += (Vector3)_walkingMovement.normalized * (moveSpeed * Time.deltaTime);
            }
        }

        public void AddWalkingDirection(Vector2 motion)
        {
            _walkingMovement += motion;
        }

        public void StopWalking()
        {
            _walkingMovement = Vector2.zero;
        }

        public void TravelToPoint(Vector2 destination, float duration)
        {
            _forcedMoveSpeed = Vector2.Distance(destination, transform.position) / duration;
            _forcedMovement = destination - (Vector2)transform.position;
        }

        public bool SetMovementSpeed(float speed)
        {
            if (_forcedMovement != Vector2.zero) return false;

            moveSpeed = speed;
            return true;
        }

        public void ResetMovementSpeed()
        {
            SetMovementSpeed(InitialMoveSpeed);
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;
            Vector3 position = transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(position, position + (Vector3)_forcedMovement);
        }

        public void PlayerFixedUpdate(PlayerController playerController)
        {
            ApplyPhysics();
        }
    }
}