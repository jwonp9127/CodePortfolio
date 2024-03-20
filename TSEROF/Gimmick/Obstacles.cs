using System.Collections;
using UnityEngine;

public enum ObstacleType
{
    Laser,
    Tornado,
    b,
    c,
    d,
    e,
    f
}
public class Obstacles : MonoBehaviour
{
    public Player player;
    public ObstacleType gimic;
    
    [SerializeField] private Vector3 tornadoDirection = new Vector3(1, 2, 1);
    [SerializeField] private float tornadoPower = 4;
    [SerializeField] private float tornadoTime = 0.6f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (gimic)
            {
                case ObstacleType.Tornado:
                    TorCoroutine();
                    break;
            }
        }
    }

    private void RotateStart()
    {
        player.Rigidbody.freezeRotation = false;
        player.ForceReceiver.AddVelocity(tornadoDirection * tornadoPower);
        player.Rigidbody.AddTorque(new Vector3(10, 40, 10));
    }

    private void RotateStop()
    {
        player.Rigidbody.rotation = Quaternion.identity;
        player.Rigidbody.freezeRotation = true;
    }
    
    private IEnumerator Tor()
    {
        RotateStart();
        yield return new WaitForSeconds(tornadoTime);
        RotateStop();
    }
    
    private void TorCoroutine()
    {
        StopCoroutine(Tor());
        StartCoroutine(Tor());
    }
}