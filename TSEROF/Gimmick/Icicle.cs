using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Icicle : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    
    // 대기 시간 캐싱
    private WaitForSeconds _fallDelay;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _fallDelay = new WaitForSeconds(2.5f);
        
        ResetObject();
        StartCoroutine(StartFalling());
    }

    private IEnumerator StartFalling()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 8));
            _rigidbody.isKinematic = false;
            yield return _fallDelay; 
            _rigidbody.isKinematic = true;
            
            // 충돌 감지 시작
            _collider.isTrigger = true;
            
            ResetObject();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Globals.PlayerTag))
        {
            RespawnPlayer(other.gameObject);
        }
        else if (other.CompareTag(Globals.IceTag))
        {
            ResetObject();
        }
    }
    
    private void RespawnPlayer(GameObject player)
    {
        PlayerRespawn playerRespawn = player.GetComponent<PlayerRespawn>();
        if (playerRespawn != null)
        {
            playerRespawn.Respawn();
        }
    }
    
    private void ResetObject()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
    }
}
