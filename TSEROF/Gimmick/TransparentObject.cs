using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    private const string PlayerTag = "Player";

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag(PlayerTag))
        {
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag(PlayerTag))
        {
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}