using UnityEngine;

namespace Code.Player.Skills
{
    public class Roll
    {
        public static void PlayerRoll(float dashSpeed)
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