using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTeleporter : MonoBehaviour
{
    [SerializeField] Transform currentCheckpoint;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = currentCheckpoint.position;
        }
    }
}
