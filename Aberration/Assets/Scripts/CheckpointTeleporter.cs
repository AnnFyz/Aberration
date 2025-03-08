using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTeleporter : MonoBehaviour
{
    [SerializeField] Transform currentCheckpoint;
    [SerializeField] GameObject player;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = currentCheckpoint.position;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            player.GetComponent<CharacterController>().enabled = false;
            player.gameObject.transform.position = currentCheckpoint.position;
            StartCoroutine(player.GetComponent<PlayerHandler>().StartTeleportationVFX());
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<CompanionCharacterController>().ToggleControls(false);
            player.transform.GetChild(0).gameObject.SetActive(false);
            StartCoroutine(StartTeleportationPLayer());
        }
    }

    IEnumerator StartTeleportationPLayer()
    {
        yield return new WaitForSeconds(1.15f);
        AudioManager.Instance.PlaySound("PlayerTeleportation");
        player.GetComponent<CompanionCharacterController>().ToggleControls(true);
        player.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void SetCurrentCheckpoint(Transform checkpoint)
    {
       
        currentCheckpoint = checkpoint;
    }
}
