using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] [Range(0, 10f)] private float fireForce;
    [SerializeField] [Range(0, 10f)] private float fireUpForce;
    private void OnParticleTrigger()
    {
        player.transform.position = Stage2Manager.instance.curRespawnPosition;
      //  Transform playerTransform = player.transform;
       // player.Rigidbody.AddForce(playerTransform.forward * -1 * fireForce + playerTransform.up * fireUpForce, ForceMode.Impulse);
    }
}
