using UnityEngine;

namespace Code.Player.Skills.Movement
{
    [CreateAssetMenu(fileName = "Roll", menuName = "ScriptableObjects/Skill/Roll", order = 1)]
    public class Roll : Skill
    {
        [SerializeField]
        private float dashSpeed;

        public override void Execute(PlayerController playerController)
        {
            Vector3 position = playerController.transform.position;
            Vector3 destinationDirection = playerController.PlayerMovement.WalkingMovement;
            Vector3 destinationPoint = destinationDirection.normalized * playerController.PlayerMovement.InitialMoveSpeed;
            playerController.PlayerMovement.TravelToPoint(destinationPoint + position, 1.0f / dashSpeed);
        }

        public override void Cleanup(PlayerController playerController) { }
    }
}