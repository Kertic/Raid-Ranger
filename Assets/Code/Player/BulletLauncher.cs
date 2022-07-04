using Code.Objects;
using UnityEngine;

namespace Code.Player
{
    public class BulletLauncher : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;

        public void Launch(Vector2 velocity, Vector2 position)
        {
            Bullet launchedBullet = Instantiate(bulletPrefab.GetComponent<Bullet>(), position, Quaternion.identity);
            launchedBullet.Spawn(velocity);
        }
    }
}