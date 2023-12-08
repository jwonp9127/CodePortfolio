using UnityEngine;

public class TopViewPlayer : Player
{
    protected override Vector3 GetMovementDirection()
    {
        Vector3 forward = CameraTransform.up;
        Vector3 right = CameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();
        
        return forward * MovementInput.y + right * MovementInput.x;
    }
    
    protected override void ReadMovementInput()
    {
        MovementInput = Input.PlayerActions.Movement.ReadValue<Vector2>();
    }
}
