using System.Collections;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _startPosition;
    private Vector3 _startRotation;
    private Collider _collider;

    [Header("임시입니다")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _respwanPos;
    void Start()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;
        _startRotation = new Vector3(0,0,0);
        StartCoroutine(MoveStart());
    }

    public IEnumerator MoveStart() //한번만 실행 시킬 수 있는 코루틴 만들어 보기
    {
        while (true)
        {
            int fallingsecond = Random.Range(1,8);
            yield return new WaitForSeconds(fallingsecond);
            _rigidbody.isKinematic = false;
            yield return new WaitForSeconds(2.5f);
            _rigidbody.isKinematic = true;
            _collider.isTrigger = true;
            transform.SetPositionAndRotation(_startPosition, Quaternion.Euler(_startRotation));
        }
      
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int faintingtime = Random.Range(4, 8);
            SwichPos();
        }
        if (other.CompareTag("Ice"))
        {
            _collider.isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    int faintingtime = Random.Range(4, 8);
        //    SwichPos();
        //}
    }

    private void SwichPos()  //임시입니다.
    {
        _player.transform.position = _respwanPos.transform.position;
    }

   // private void 
}//상속받아서 
