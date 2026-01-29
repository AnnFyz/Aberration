using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSoundHandler : MonoBehaviour
{
    [SerializeField] string soundToPlay;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //Debug.Log("StopSound Background");
            AudioManager.Instance.StopSound("Background");
        }
    }


    private void Start()
    {
        AudioManager.Instance.StopSound("Background");
        //AudioManager.Instance.PlaySound(soundToPlay);
    }
}
