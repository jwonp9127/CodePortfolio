using System;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private GameManager _gameManager;
    private Rigidbody _rigidbody;
    
    private Vector3[] _respawnPosition;
    private Quaternion[] _respawnRotation;
    private int _currentStage;
    
    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _rigidbody = GetComponent<Rigidbody>();

        _respawnPosition = new Vector3[3];
        _respawnRotation = new Quaternion[3];
    }

    private void Start()
    {
        RespawnPositionSetting();
    }

    private void Update()
    {        
        if (_currentStage < 1)
        {
            return;
        }
        
        _currentStage = _gameManager.CurrentStage - 1;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision != null && collision.CompareTag(Globals.DeadZoneTag))
        {
            Respawn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject.CompareTag(Globals.DeadZoneTag))
        {
            Respawn();
        }
    }
    
    public void Respawn()
    {
        if (_currentStage == 1)
        {
            _rigidbody.transform.position = Stage2Manager.Instance.CurrentRespawnPosition;
        }
        else
        {
            _rigidbody.transform.position = _respawnPosition[_currentStage];
        }
        _rigidbody.transform.rotation = _respawnRotation[_currentStage];
    }

    private void RespawnPositionSetting()
    {
        _respawnPosition[0] = new Vector3(-2, 2, 2);
        _respawnRotation[0] = Quaternion.Euler(new Vector3(0, 90, 0));

        _respawnPosition[1] = new Vector3();
        _respawnRotation[1] = Quaternion.Euler(new Vector3(0, -75f, 0));
        
        _respawnPosition[2] = new Vector3(0, 2, 2);
        _respawnRotation[2] = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
