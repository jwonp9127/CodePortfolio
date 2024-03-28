using UnityEngine;
using UnityEngine.InputSystem;

public class Leaf : MonoBehaviour
{
    [SerializeField] private GameObject icon;
    
    private Player _player;
    private bool _isOnRide;
    private float _speedMultiplier_f = 1.7f;

    private void OnTriggerEnter(Collider other)
    {
        icon.SetActive(true);
        _player = other.GetComponent<Player>();
        
        if (_isOnRide || _player == null)
        {
            return;
        }
        
        _player.Input.PlayerActions.Interaction.started += CheckOnRideStarted;
    }

    private void OnTriggerExit(Collider other)
    {
        icon.SetActive(false);
        _player = other.GetComponent<Player>();
        
        if (_isOnRide || _player == null)
        {
            return;
        }
        
        _player.Input.PlayerActions.Interaction.started -= CheckOnRideStarted;
    }
    
    private void CheckOnRideStarted(InputAction.CallbackContext obj)
    {
        if (!_isOnRide)
        {
            _isOnRide = true;
            _player.transform.position = transform.position;
            _player.transform.rotation = transform.rotation;
            _player.movementSpeed *= _speedMultiplier_f;
            transform.SetParent(_player.transform);
        
            // Section Change
            Stage2Manager.Instance.SetSection(2);
        }
    }
}
