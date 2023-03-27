using UnityEngine;

namespace Code.Player.Skills.Movement
{
    public class Roll : MonoBehaviour
    {
        public void PlayerRoll(float dashSpeed)
        {
            PlayerController playerController = PlayerController.Instance;
            
            if (playerController == null)
            {
                return;
            }

            Vector3 destinationDirection = playerController.MouseWorldPosition - playerController.transform.position;
            Vector3 destinationPoint = destinationDirection.normalized * playerController.InitialMoveSpeed;
            playerController.TravelToPoint(destinationPoint + playerController.transform.position, 1.0f / dashSpeed);
        }
    }
}