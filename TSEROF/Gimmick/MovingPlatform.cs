using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField]
    private WaypointPath _waypointPath;

    [SerializeField]
    private float _speed;

    private int _targetWaypointIndex;

    private Transform _previousWaypoint;
    private Transform _targetWaypoint;

    private float _timeToWaypoint;
    private float _elapsedTime;

    [SerializeField] private bool isCollisionMoving;
    [SerializeField] private bool canMove;

    public void Start()
    {
        TargetNextWaypoint();
    }

    public void FixedUpdate()
    {
        if (!isCollisionMoving || canMove)
        {
            Move();
        }
    }

    private void Move()
    {
        _elapsedTime += Time.deltaTime;

        float elapsedPercentage = _elapsedTime / _timeToWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);//한게에 스무딩이 추가된다
        transform.position = Vector3.Lerp(_previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(_previousWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage); //재미를 주기 위해서 일단 회전값도 한번 넣어봄
        if (elapsedPercentage >= 1)
        {
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        _previousWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);
        _targetWaypointIndex = _waypointPath.GetNextWaypointIndex(_targetWaypointIndex);
        _targetWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);

        _elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(_previousWaypoint.position, _targetWaypoint.position);
        _timeToWaypoint = distanceToWaypoint / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        other.transform.SetParent(transform);
        canMove = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Transform playerTransform = collision.gameObject.transform;
        if (playerTransform == null)
        {
            return;
        }
        playerTransform.SetParent(transform);
        canMove = true;
    }

    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
            canMove = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
            canMove = false;
        }
    }
}
//캐릭터를 이동플렛폼위에 올려놓고 이동시키면 움직여지지 않는다. 이것의 해결방법은 캐릭터를 플랫폼의 하위 즉 자식으로 만드는 것이다.
//콜라인터의 충돌로 인한 오류를 막고 싶다면 표준업데이트가 아닌 고정업데이트를 사용하도록 이동을 변경해야한다.