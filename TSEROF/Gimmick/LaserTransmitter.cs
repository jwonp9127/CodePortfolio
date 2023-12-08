using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

public class LaserTransmitter : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    [SerializeField] private float rotationDamping;
    [SerializeField] private int rotationAngleY;
    [SerializeField] private int maxRotationAngleY;
    [SerializeField] private Quaternion rotationDirection;
    
    [SerializeField] private TopViewPlayer _player;
    [SerializeField] private bool isRotate;

    [SerializeField] private Quaternion _baseRotation;
    
    [SerializeField] private LaserReceiver _beforeLaserReceiver;
    [SerializeField] private LaserReceiver _currentLaserReceiver;
    [SerializeField] private Quaternion additionalRotation;
    [SerializeField] private bool isSuccess;
    public float _startTime = -1;
    private float successTime = 3;

    [SerializeField] private GameObject interactionIcon;

    private LaserPatternManager _manager;

    private void Awake()
    {
        _manager = GetComponentInParent<LaserPatternManager>();
        interactionIcon = transform.GetChild(2).gameObject;
    }

    private void Start()
    {
        _baseRotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -maxRotationAngleY / 2, 0));
        additionalRotation = Quaternion.Euler(new Vector3(0,
            new Random().Next(maxRotationAngleY / rotationAngleY + 1) * rotationAngleY, 0));
        transform.rotation =
            Quaternion.Euler(_baseRotation.eulerAngles + additionalRotation.eulerAngles);
        
        rotationDirection = _baseRotation;
        interactionIcon.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isSuccess)
        {
            return;
        }

        if (isRotate)
        {
            Rotate();
        }
        ShootLaser();
    }

    private void ShootLaser()
    {
        Ray ray = new Ray(transform.position + transform.right, transform.right);
        RaycastHit[] hits = new RaycastHit[1];
        if (Physics.RaycastNonAlloc(ray, hits, maxDistance, LayerMask.GetMask("ReceiverSensor")) >= 1)
        {
            _beforeLaserReceiver = _currentLaserReceiver;
            _currentLaserReceiver = hits[0].transform.gameObject.GetComponentInParent<LaserReceiver>();
        }
        else
        {
            _currentLaserReceiver = null;
        }
        
        if (_beforeLaserReceiver == _currentLaserReceiver)
        {
            if (_beforeLaserReceiver == null)
            {
                return;
            }

            GetSuccess();
                
            if (!isSuccess)
            {
                return;
            }

            _manager.successCount++;
            interactionIcon.SetActive(false);
            return;
        }

        if (_beforeLaserReceiver != null)
        {
            _beforeLaserReceiver.Deactivate(out _startTime);
        }

        if (_currentLaserReceiver == null)
        {
            return;
        }
        _currentLaserReceiver.Activate(transform, out _startTime);
    }
    
    private void GetSuccess()
    {
        isSuccess = _startTime >= 0 && Time.time - _startTime > successTime;
        _startTime = isSuccess ? 0 : _startTime;
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationDirection, rotationDamping * Time.deltaTime);
        if (transform.rotation == rotationDirection)
        {
            isRotate = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<TopViewPlayer>();

        if (_player == null)
        {
            return;
        }

        _player.Input.PlayerActions.Interaction.started += ChangeDegreeStarted;

        if (isSuccess)
        {
            return;
        }
        
        interactionIcon.SetActive(true);
    }
    
    private void OnTriggerExit(Collider other)
    {
        _player = other.GetComponent<TopViewPlayer>();

        if (_player == null)
        {
            return;
        }

        _player.Input.PlayerActions.Interaction.started -= ChangeDegreeStarted;
        
        if (isSuccess)
        {
            return;
        }
        
        interactionIcon.SetActive(false);
    }

    private void ChangeDegreeStarted(InputAction.CallbackContext obj)
    {
        isRotate = true;
        
        if (additionalRotation.eulerAngles.y + rotationAngleY > maxRotationAngleY || additionalRotation.eulerAngles.y + rotationAngleY < 0)
        {
            rotationAngleY = -rotationAngleY;
        }

        additionalRotation = Quaternion.Euler(additionalRotation.eulerAngles + new Vector3(0, rotationAngleY, 0));
        rotationDirection = Quaternion.Euler(_baseRotation.eulerAngles + additionalRotation.eulerAngles);
    }
}
