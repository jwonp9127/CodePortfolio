using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class NeedleTrap : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] private Player player;
    
    [Header("Position Setting")]
    [SerializeField] private bool isMove = false;
    [SerializeField] private Vector3 idlePosition = new Vector3(0, -1f, 0);
    [SerializeField] private Vector3 outPosition = Vector3.zero;
    [SerializeField] private float emergeSpeed = 8;
    [SerializeField] private float retractSpeed = 1;
    [SerializeField] private Vector2 idleTimeRange = new Vector2(2f, 3f);

    [Header("Power Setting")]
    [SerializeField] private Vector3 needlePower = new Vector3(-3, 10, 0);
    [SerializeField] [Range(0, 20f)] private float needleDrag = 0.5f;
    
    private Vector3 _needlePosition;
    private int _state;
    private float _idleTime;
    
    private void Start()
    {
        idlePosition = idlePosition == Vector3.zero ? new Vector3(0, -1f, 0) : idlePosition;
        outPosition = outPosition == Vector3.zero ? Vector3.zero : outPosition;
        emergeSpeed = emergeSpeed == 0 ? 8 : emergeSpeed;
        retractSpeed = retractSpeed == 0 ? 1 : retractSpeed;
        idleTimeRange = idleTimeRange == Vector2.zero ? new Vector2(2f, 3f) : idleTimeRange;
        
        _state = 0;
        _idleTime = Random.Range(idleTimeRange.x, idleTimeRange.y);
        transform.GetChild(0).localPosition = isMove ? idlePosition : outPosition;
    }

    private void Update()
    {
        if (isMove)
        {
            Move();
        }
    }

    private void Move()
    {
        if (_state == 0)
        {
            StartCoroutine(IdleCoroutine());
            _state = 1;
        }
        
        else if (_state == 2)
        {
            transform.GetChild(0).Translate(Vector3.up * (Time.deltaTime * emergeSpeed));
            _needlePosition = transform.GetChild(0).localPosition;
            
            if (_needlePosition.y >= outPosition.y)
            {
                _state++;
            }
        }
        
        else if (_state == 3)
        {
            transform.GetChild(0).Translate(Vector3.down * (Time.deltaTime * retractSpeed));
            _needlePosition = transform.GetChild(0).localPosition;
            
            if (_needlePosition.y <= idlePosition.y)
            {
                _state = 0;
            }
        }
    }

    private IEnumerator IdleCoroutine()
    {
        yield return new WaitForSeconds(_idleTime);
        _state++;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.ForceReceiver.AddVelocity(needlePower, needleDrag);
        }
    }
}
