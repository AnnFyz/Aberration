using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTeleporter : MonoBehaviour
{
    [SerializeField] Transform currentCheckpoint;
    public static CheckpointTeleporter Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = currentCheckpoint.position;
        }
    }

    public void SetCurrentCheckpoint(Transform checkpoint)
    {
        Debug.Log("SetCurrentCheckpoint");
        currentCheckpoint = checkpoint;
    }
}
