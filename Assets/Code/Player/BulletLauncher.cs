using Code.Objects;
using UnityEngine;

namespace Code.Player
{
    public class BulletLauncher : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;

        public void Launch(Vector2 direction, Vector2 position)
        {
            //TODO: Refactor this into an object pool instead of instantiating them all dynamically 
            Bullet launchedBullet = Instantiate(bulletPrefab.GetComponent<Bullet>(), position, Quaternion.identity);
            launchedBullet.Spawn(direction);
        }
    }
}