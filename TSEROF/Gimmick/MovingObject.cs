using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Header("Waypoint Path")]
    [SerializeField] private WaypointPath _waypointPath;

    [Header("Option")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private bool _isCollisionMoving;
    [SerializeField] private bool _isReverse;
    
    private int _currentIndex;
    private Transform _currentWaypoint;
    private Transform _targetWaypoint;

    private float _timeToWaypoint;
    private float _elapsedTime;
    
    private bool _canMove;

    private void Start()
    {
        SetTargetWaypoint();
    }

    private void Update()
    {
        if (!_isCollisionMoving || _canMove)
        {
            Move();
        }
    }

    private void Move()
    {
        _elapsedTime += Time.deltaTime;

        float elapsedPercentage = _elapsedTime / _timeToWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        
        // waypoint 사이을 보간하여 이동
        transform.position = Vector3.Lerp(_currentWaypoint.position, _targetWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Slerp(_currentWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage);
        
        if (elapsedPercentage >= 1)
        {
            SetTargetWaypoint();
        }
    }

    private void SetTargetWaypoint()
    {
        _currentWaypoint = _waypointPath.GetWaypoint(_currentIndex);

        if (!_isReverse)
        {
            _targetWaypoint = _waypointPath.GetNextWaypoint(ref _currentIndex);
        }
        else
        {
            _targetWaypoint = _waypointPath.GetPreviousWaypoint(ref _currentIndex);
        }

        _elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(_currentWaypoint.position, _targetWaypoint.position);
        _timeToWaypoint = distanceToWaypoint / _movementSpeed;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Globals.PlayerTag))
        {
            collision.transform.SetParent(transform);
            _canMove = true;
        }
    }

    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(Globals.PlayerTag))
        {
            collision.transform.SetParent(null);
            _canMove = false;
        }
    }
}
