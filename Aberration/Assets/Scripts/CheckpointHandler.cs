using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    [SerializeField] Transform checkpoint;

    private void Awake()
    {
        //Transform[] children = GetComponentsInChildren<Transform>();
        //foreach (Transform child in children)
        //{
        //    if(child.gameObject.tag == "Checkpoint")
        //    {
        //        checkpoint = child;
        //    }
        //}
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log("collision.gameObject.tag == Player");
    //        CheckpointTeleporter.Instance.SetCurrentCheckpoint(checkpoint);
    //    }
        
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("other.gameObject.tag == Player");
            CheckpointTeleporter.Instance.SetCurrentCheckpoint(checkpoint);
        }
    }
}
