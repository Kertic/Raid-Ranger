using UnityEngine;

namespace Code.CameraCode
{
    public class PlayerCam : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float zoomSpeed;
        [SerializeField] private float lerpPace; 



        private void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                transform.Translate(0, 0, zoomSpeed * Input.mouseScrollDelta.y);
            }
        }

        void FixedUpdate()
        {
            Vector3 position = transform.position;
            Vector3 deltalerp = Vector3.Lerp(position, player.position, Time.deltaTime * lerpPace);
            deltalerp.z = position.z; // Don't change the Z
            position = deltalerp;
            transform.position = position;
        }
    }
}