using UnityEngine;
using UnityEngine.InputSystem;

public class Leaf : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private bool isOnRide;
    [SerializeField] private GameObject iconObject;
    private void OnTriggerEnter(Collider other)
    {
        iconObject.SetActive(true);
        if (isOnRide)
        {
            return;
        }
        
        player = other.GetComponent<Player>();
        
        if (player == null)
        {
            return;
        }
        
        player.Input.PlayerActions.Interaction.started += CheckOnRideStarted;
    }

    private void OnTriggerExit(Collider other)
    {
        iconObject.SetActive(false);
        if (isOnRide)
        {
            return;
        }
        
        player = other.GetComponent<Player>();
        
        if (player == null)
        {
            return;
        }
        
        player.Input.PlayerActions.Interaction.started -= CheckOnRideStarted;
    }
    
    private void CheckOnRideStarted(InputAction.CallbackContext obj)
    {
        if (isOnRide)
        {
            return;
        }

        isOnRide = true;
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
        player.movementSpeed *= 1.7f;
        Stage2Manager.instance.isStage2Clear = true;
        transform.SetParent(player.transform);
    }
}
