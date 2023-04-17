using System;
using Code.Objects;
using Unity.Mathematics;
using UnityEngine;

namespace Code.Player.Weapons
{
    public class BulletLauncher : Weapon
    {
        enum GunState
        {
            READY,
            EMPTY,
            RELOADING,
            NUMOFSTATES
        }

        [SerializeField]
        private TimingBar timingBar;
        [SerializeField]
        private float cooldown, shootingMoveSpeedModifier, perfectReloadDuration;

        [SerializeField]
        private int maxBullets;

        private float _cooldownHeat, _reloadDuration;
        private int _bulletCount;
        private GunState _gunState;

        [SerializeField]
        private GameObject bulletPrefab; //TODO: Refactor this into an object pool instead of instantiating them all dynamically 

        private void Start()
        {
            _cooldownHeat = 0;
            _bulletCount = maxBullets;
            ChangeState(GunState.READY);
            PlayerController.PlayerUpdate += BulletLauncherUpdate;
        }

        public override void Attack(PlayerController playerController)
        {
            switch (_gunState)
            {
                case GunState.READY:
                    if (_cooldownHeat > 0) return;
                    Vector3 position = playerController.transform.position;
                    Vector3 direction = playerController.MouseWorldPosition - position;
                    Bullet launchedBullet = Instantiate(bulletPrefab.GetComponent<Bullet>(), position, Quaternion.identity);
                    launchedBullet.Spawn(direction);
                    _cooldownHeat = cooldown * (1.0f / playerController.AttackSpeed);
                    _bulletCount -= 1;
                    if (_bulletCount <= 0)
                    {
                        ChangeState(GunState.EMPTY);
                    }

                    break;
                case GunState.EMPTY:
                    break;
                case GunState.RELOADING:
                    Reload();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void SecondaryAttack(PlayerController playerController)
        {
            Reload();
        }

        public override void BulletLauncherUpdate(PlayerController playerController)
        {
            if (_cooldownHeat > 0)
            {
                PlayerMovement playerMovement = playerController.PlayerMovement;
                _cooldownHeat = math.max(_cooldownHeat - Time.deltaTime, 0);

                if (_cooldownHeat == 0.0f)
                {
                    playerMovement.ResetMovementSpeed();
                }
                else
                {
                    playerMovement.SetMovementSpeed(playerMovement.InitialMoveSpeed * shootingMoveSpeedModifier);
                }
            }

            if (_reloadDuration > 0)
            {
                _reloadDuration -= math.max(Time.deltaTime, 0);
                if (_reloadDuration == 0.0f)
                {
                    ChangeState(GunState.READY);
                }
            }
        }

        private void Reload() { }

        private void ChangeState(GunState newState)
        {
            _gunState = newState;
            switch (newState)
            {
                case GunState.READY:
                    _bulletCount = maxBullets;
                    break;
                case GunState.EMPTY:
                    break;
                case GunState.RELOADING:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
    }
}