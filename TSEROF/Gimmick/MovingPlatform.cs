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
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);//�ѰԿ� �������� �߰��ȴ�
        transform.position = Vector3.Lerp(_previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(_previousWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage); //��̸� �ֱ� ���ؼ� �ϴ� ȸ������ �ѹ� �־
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
//ĳ���͸� �̵��÷������� �÷����� �̵���Ű�� ���������� �ʴ´�. �̰��� �ذ����� ĳ���͸� �÷����� ���� �� �ڽ����� ����� ���̴�.
//�ݶ������� �浹�� ���� ������ ���� �ʹٸ� ǥ�ؾ�����Ʈ�� �ƴ� ����������Ʈ�� ����ϵ��� �̵��� �����ؾ��Ѵ�.