using System;
using Code.Objects;
using Code.UI;
using Code.UI.Weapons;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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
        private BulletLauncherClipUI clipUI;

        [SerializeField]
        private float cooldown, shootingMoveSpeedModifier, perfectReloadDuration, totalReloadTime, snipeBulletDamageModifier;

        [SerializeField]
        private int maxBullets;

        private float _cooldownHeat, _reloadDurationRemaining, _earliestPerfectReload, _latestPerfectReload;
        private int _bulletCount;
        private GunState _gunState;

        [SerializeField]
        private GameObject bulletPrefab; //TODO: Refactor this into an object pool instead of instantiating them all dynamically 

        public int BulletCount
        {
            get => _bulletCount;
            set
            {
                _bulletCount = math.max(0, value);
                clipUI.SetAmmoRemaining(BulletCount);
            }
        }

        private void Start()
        {
            _cooldownHeat = 0;
            BulletCount = maxBullets;
            ChangeState(GunState.READY);
            PlayerController.PlayerUpdate += BulletLauncherUpdate;
        }

        public override void Attack(PlayerController playerController)
        {
            switch (_gunState)
            {
                case GunState.READY:
                    if (BulletCount > 0)
                    {
                        if (_cooldownHeat <= 0)
                        {
                            LaunchBullet(playerController);
                            _cooldownHeat = cooldown * (1.0f / playerController.AttackSpeed);
                            BulletCount -= 1;
                        }
                    }
                    else
                    {
                        ChangeState(GunState.EMPTY);
                        Reload();
                    }

                    break;
                case GunState.EMPTY:
                    Reload();
                    break;
                case GunState.RELOADING:
                    Reload();
                    break;
                case GunState.NUMOFSTATES:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            clipUI.SetAmmoRemaining(BulletCount);
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

            if (_gunState == GunState.RELOADING)
            {
                if (_reloadDurationRemaining > 0)
                {
                    _reloadDurationRemaining -= math.max(Time.deltaTime, 0);
                    timingBar.SetCursorPercentage(1.0f - (_reloadDurationRemaining / totalReloadTime));
                    if (_reloadDurationRemaining <= 0.0f)
                    {
                        Reload();
                    }
                }
            }
        }

        public bool Snipe(PlayerController playerController)
        {
            switch (_gunState)
            {
                case GunState.READY:
                    Bullet launchedBullet = LaunchBullet(playerController);
                    launchedBullet.SetDamage((int)(launchedBullet.GetDamage() * snipeBulletDamageModifier) * BulletCount);
                    BulletCount = 0;
                    Reload();
                    return true;
                case GunState.EMPTY:
                    return false;
                case GunState.RELOADING:
                    return false;
            }

            return false;
        }

        private Bullet LaunchBullet(PlayerController playerController)
        {
            Vector3 position = playerController.transform.position;
            Vector3 direction = playerController.MouseWorldPosition - position;
            Bullet launchedBullet = Instantiate(bulletPrefab.GetComponent<Bullet>(), position, Quaternion.identity);
            launchedBullet.Spawn(direction);
            return launchedBullet;
        }

        private void Reload()
        {
            switch (_gunState)
            {
                case GunState.READY:
                case GunState.EMPTY:
                    _reloadDurationRemaining = totalReloadTime;
                    ChangeState(GunState.RELOADING);
                    break;
                case GunState.RELOADING:
                    if (Time.time >= _earliestPerfectReload && Time.time <= _latestPerfectReload)
                    {
                        PerfectReload();
                    }
                    else
                    {
                        Debug.Log("Failed Perfect Reload");
                        timingBar.SetTimingWindow(0, 0);
                    }

                    _earliestPerfectReload = 0;
                    _latestPerfectReload = 0;

                    if (_reloadDurationRemaining <= 0.0f)
                    {
                        ChangeState(GunState.READY);
                    }

                    break;
            }
        }

        private void PerfectReload()
        {
            Debug.Log("This means Perfectly reloaded");
            ChangeState(GunState.READY);
        }

        private void ChangeState(GunState newState)
        {
            _gunState = newState;
            switch (newState)
            {
                case GunState.READY:
                    BulletCount = maxBullets;
                    timingBar.SetTimingWindow(0, 0);
                    timingBar.SetCursorPercentage(0);
                    break;
                case GunState.EMPTY:
                    break;
                case GunState.RELOADING:
                    float beginning = Random.Range(0.4f, 0.8f);
                    float end = (perfectReloadDuration / totalReloadTime) + beginning;
                    timingBar.SetTimingWindow(beginning, end);
                    _earliestPerfectReload = Time.time + (totalReloadTime * beginning);
                    _latestPerfectReload = Time.time + (totalReloadTime * end);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            clipUI.SetAmmoRemaining(BulletCount);
        }
    }
}