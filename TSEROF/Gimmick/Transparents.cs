using UnityEngine;

public class Transparents : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowNeedles();          
        }
    }

    private void ShowNeedles()
    {
        for(int i = 0; i < transform.childCount; i++) 
        {
            transform.GetChild(i).GetComponent<Transparent>()?.StartShowCoroutine();
        }
    }
}
