using System.Collections;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _tornadoDirection;
    private float _tornadoPower;
    private float _tornadoDuration;
    private Vector3 _tornadoTorque;

    private WaitForSeconds _waitTornadoDuration;
    
    private void Start()
    {
        _waitTornadoDuration = new  WaitForSeconds(_tornadoDuration);
        InitializeDefaults();
    }

    private void InitializeDefaults()
    {
        _tornadoDirection = new Vector3(1, 2, 1).normalized;
        _tornadoPower = 4f;
        _tornadoDuration = 0.6f;
        _tornadoTorque = new Vector3(10, 40, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        _rigidbody = other.GetComponent<Rigidbody>();
        
        if (_rigidbody != null)
        {
            StartCoroutine(TornadoEffect());
        }
    }

    // 토네이도 회전
    private IEnumerator TornadoEffect()
    {
        RotateObject(true);
        
        yield return _waitTornadoDuration;
        
        RotateObject(false);
    }

    // 회전 여부 설정
    private void RotateObject(bool startRotation)
    {
        _rigidbody.freezeRotation = !startRotation;
        
        if (startRotation)
        {
            _rigidbody.AddForce(_tornadoDirection * _tornadoPower, ForceMode.Impulse);
            _rigidbody.AddTorque(_tornadoTorque);
        }
        else
        {
            _rigidbody.rotation = Quaternion.identity;
        }
    }
}