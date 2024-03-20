using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Transparents : MonoBehaviour
{
    [SerializeField] private GameObject iconObject;
    [SerializeField] private Player player;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        ShowNeedles();          
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        iconObject.SetActive(true);
        player = other.GetComponent<Player>();

        if (player == null)
        {
            return;
        }

        player.Input.PlayerActions.Interaction.started += CheckShowNeedleStarted;
    }

    private void OnTriggerExit(Collider other)
    {
        iconObject.SetActive(false);

        player = other.GetComponent<Player>();

        if (player == null)
        {
            return;
        }

        player.Input.PlayerActions.Interaction.started -= CheckShowNeedleStarted;
    }

    private void CheckShowNeedleStarted(InputAction.CallbackContext obj)
    {
        ShowNeedles();
    }

    private void ShowNeedles()
    {
        for(int i = 0; i < transform.childCount; i++) 
        {
            transform.GetChild(i).GetComponent<Transparent>()?.StartShowCoroutine();
        }
    }
}
