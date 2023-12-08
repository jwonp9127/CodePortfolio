using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputAction PlayerInputAction { get; private set; }
    public PlayerInputAction.PlayerActions PlayerActions { get; private set; }

    private void Awake()
    {
        PlayerInputAction = new PlayerInputAction();
        PlayerActions = PlayerInputAction.Player;
    }

    private void OnEnable()
    {
        PlayerActions.Enable();
    }

    private void OnDisable()
    {
        PlayerActions.Disable();
    }
}
