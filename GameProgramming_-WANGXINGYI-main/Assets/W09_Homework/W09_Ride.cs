using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W09_Ride : MonoBehaviour
{
    public GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        if (Player.tag == other.gameObject.tag)
        {
            Player.transform.position = transform.position + Vector3.up;
            Player.transform.rotation = transform.rotation;
            Player.transform.SetParent(transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (Player.tag == other.gameObject.tag)
        {
            Player.transform.SetParent(null);
        }
    }
}