using Code.Objects;
using Unity.Mathematics;
using UnityEngine;

namespace Code.Player.Weapons
{
    public class BulletLauncher : Weapon
    {
        [SerializeField]
        private float cooldown, shootingSpeedModifier;

        private float _cooldownHeat;

        [SerializeField]
        private GameObject bulletPrefab; //TODO: Refactor this into an object pool instead of instantiating them all dynamically 

        private void Start()
        {
            _cooldownHeat = 0;
        }

        public override void Attack(PlayerController playerController)
        {
            if (_cooldownHeat > 0) return;

            Vector3 position = playerController.transform.position;
            Vector3 direction = playerController.MouseWorldPosition - position;
            Bullet launchedBullet = Instantiate(bulletPrefab.GetComponent<Bullet>(), position, Quaternion.identity);
            launchedBullet.Spawn(direction);
            _cooldownHeat = cooldown * (1.0f / playerController.AttackSpeed);
        }

        public override void SecondaryAttack(PlayerController playerController) { }

        public override void PlayerFixedUpdate(PlayerController playerController)
        {
            if (_cooldownHeat <= 0) return;

            PlayerMovement playerMovement = playerController.PlayerMovement;
            _cooldownHeat = math.max(_cooldownHeat - Time.deltaTime, 0);

            if (_cooldownHeat == 0.0f)
            {
                playerMovement.ResetMovementSpeed();
            }
            else
            {
                playerMovement.SetMovementSpeed(playerMovement.InitialMoveSpeed * shootingSpeedModifier);
            }
        }
    }
}