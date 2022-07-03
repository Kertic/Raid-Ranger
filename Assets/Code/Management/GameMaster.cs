using Code.Player;
using UnityEngine;

namespace Code.Management
{
    public class GameMaster : MonoBehaviour
    {
        [SerializeField] private float arenaExtents;

        [SerializeField] private PlayerController player;

        public static GameMaster Instance;

        // Start is called before the first frame update
        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        // Update is called once per frame
        void Update()
        {
            float distanceFromCenter = Vector3.Distance(player.transform.position, Vector3.zero);

            if (distanceFromCenter > arenaExtents)
            {
                PlayerOutOfBounds();
            }
        }

        void PlayerOutOfBounds()
        {
            Debug.Log("Out of bounds");
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Vector3.zero, arenaExtents);
        }
    }
}