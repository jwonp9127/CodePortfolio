using UnityEngine;
using UnityEngine.InputSystem;

public class Leaf : MonoBehaviour
{
    private Player _player;
    private bool _playerIsOnRide;
    private GameObject _interactionIcon;
    private Transform _playerTransform;
    
    private void Awake()
    {
        _interactionIcon = transform.GetChild(2).gameObject;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        _interactionIcon.SetActive(true);
        if (_playerIsOnRide)
        {
            return;
        }
        
        _player = other.GetComponent<Player>();
        
        if (_player == null)
        {
            return;
        }
        
        _player.Input.PlayerActions.Interaction.started += CheckOnRideStarted;
    }

    private void OnTriggerExit(Collider other)
    {
        _interactionIcon.SetActive(false);
        if (_playerIsOnRide)
        {
            return;
        }
        
        _player = other.GetComponent<Player>();
        
        if (_player == null)
        {
            return;
        }
        
        _player.Input.PlayerActions.Interaction.started -= CheckOnRideStarted;
    }
    
    private void CheckOnRideStarted(InputAction.CallbackContext obj)
    {
        if (_playerIsOnRide)
        {
            return;
        }
        SetPlayerOnRide();
    }
    
    private void SetPlayerOnRide()
    {
        if (_playerTransform == null)
        {
            return;
        }
        
        _playerIsOnRide = true;
        _playerTransform = _player.transform;
        _playerTransform.position = transform.position;
        _playerTransform.rotation = transform.rotation;
        _player.movementSpeed *= 1.7f;
        Stage2Manager.instance.isStage2Clear = true;
        transform.SetParent(_playerTransform);
    }
}