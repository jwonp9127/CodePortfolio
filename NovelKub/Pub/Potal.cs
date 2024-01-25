using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Potal : MonoBehaviour
{
    public Player player;
    public GameObject PubPotal;
    public GameObject InPotal;
    private Collider collider;
    int i = -1;

    void Start()
    {
        collider = InPotal.GetComponent<Collider>();
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPotal()
    {
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            player.Teleport(PubPotal.transform.position + new Vector3(0, 2, i));
            i = -i;


        }
    }
}