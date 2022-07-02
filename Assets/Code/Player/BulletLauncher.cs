using Code.Objects;
using UnityEngine;

namespace Code.Player
{
    public class BulletLauncher : MonoBehaviour
    {
        [SerializeField] private GameObject bullet;

        public void Launch(Vector2 velocity, Vector2 position)
        {
            var launchedBullet = Instantiate(bullet.GetComponent<Bullet>(), position, Quaternion.identity);
            launchedBullet.Spawn(velocity);
        }
    }
}