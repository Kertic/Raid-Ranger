using System;
using UnityEngine;

namespace Code.CameraCode
{
    public class PlayerCam : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float zoomSpeed;
        [SerializeField] private float lerpPace; 

        // Start is called before the first frame update
        void Start()
        {
        }

        private void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                transform.Translate(0, 0, zoomSpeed * Input.mouseScrollDelta.y);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 delta = player.position - transform.position;
            Vector3 deltalerp = Vector3.Lerp(transform.position, player.position, Time.deltaTime * lerpPace);
            delta.z = 0; // Don't change the Z
            deltalerp.z = transform.position.z; // Don't change the Z
            transform.position = deltalerp;
//            MoveCam(deltalerp);
        }

        void MoveCam(Vector3 delta)
        {
            transform.position += delta;
        }
    }
}