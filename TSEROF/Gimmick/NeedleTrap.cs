using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class NeedleTrap : MonoBehaviour
{
    [Header("Option")]
    [SerializeField] private bool _isMove;
    [SerializeField] private float _speedMultiplier_f;
    [SerializeField] private Vector2 _idleTimeRange;
    [SerializeField] private Vector3 _needlePower;

    private const int IdleState = 0;
    private const int EmergeState = 1;
    private const int RetractState = 2;

    private Transform _needleTransform;
    private Vector3 _idlePosition;
    private Vector3 _outPosition;
    private float _emergeSpeed;
    private float _retractSpeed;
    private float _idleDuration;

    // wait for idleDuration 캐싱
    private WaitForSeconds _idleWait;

    private void Start()
    {
        _needleTransform = transform.GetChild(0);
        InitializeDefaults();
        SetInitialState();

        _idleWait = new WaitForSeconds(_idleDuration);
    }

    private void InitializeDefaults()
    {
        _idlePosition = new Vector3(0, -1f, 0);
        _outPosition = Vector3.zero;
        _emergeSpeed = 8 * _speedMultiplier_f;
        _retractSpeed = 1 * _speedMultiplier_f;
        _idleTimeRange = new Vector2(2f, 3f);
    }

    private void SetInitialState()
    {
        _idleDuration = Random.Range(_idleTimeRange.x, _idleTimeRange.y);
        _needleTransform.localPosition = _isMove ? _idlePosition : _outPosition;
    }

    private void Update()
    {
        if (_isMove)
        {
            Move();
        }
    }

    private void Move()
    {
        if (IsInState(IdleState))
        {
            StartCoroutine(IdleCoroutine());
            SetState(EmergeState);
        }
        else if (IsInState(EmergeState))
        {
            EmergeNeedle();
        }
        else if (IsInState(RetractState))
        {
            RetractNeedle();
        }
    }

    // State 확인
    private bool IsInState(int state)
    {
        if (state == IdleState)
        {
            return _needleTransform.localPosition.y <= _idlePosition.y;
        }
        else
        {
            return _needleTransform.localPosition.y >= _outPosition.y;
        }
    }
    
    private void EmergeNeedle()
    {
        _needleTransform.Translate(Vector3.up * (Time.deltaTime * _emergeSpeed));

        if (IsInState(RetractState))
        {
            SetState(RetractState);
        }
    }
    
    private void RetractNeedle()
    {
        _needleTransform.Translate(Vector3.down * (Time.deltaTime * _retractSpeed));

        if (IsInState(IdleState))
        {
            SetState(IdleState);
        }
    }

    // Idle State에서 대기
    private IEnumerator IdleCoroutine()
    {
        yield return _idleWait;
        SetState(EmergeState);
    }

    // State 변경
    private void SetState(int state)
    {
        Vector3 targetPosition = state == EmergeState ? _outPosition : _idlePosition;
        _needleTransform.localPosition = targetPosition;
    }

    // Player에게 힘을 가함
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            return;
        }
        
        rb.AddForce(_needlePower, ForceMode.Impulse);
    }
}
