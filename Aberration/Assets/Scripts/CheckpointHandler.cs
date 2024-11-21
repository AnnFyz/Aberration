using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    [SerializeField] Transform checkpoint;

    private void Awake()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if(child.gameObject.tag == "Checkpoint")
            {
                checkpoint = child;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("SetCurrentCheckpoint Trigger");
        if (other.gameObject.tag == "Player")
            CheckpointTeleporter.Instance.SetCurrentCheckpoint(checkpoint);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("SetCurrentCheckpoint Col");
        if (collision.gameObject.tag == "Player")
        CheckpointTeleporter.Instance.SetCurrentCheckpoint(checkpoint);
    }
}
