using UnityEngine;

public class FireTrap : MonoBehaviour
{
    private void OnParticleTrigger()
    {
        GameManager.Instance.playerRespawn.Respawn();
    }
}
