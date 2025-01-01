using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeHandler : MonoBehaviour
{
    [SerializeField] GameObject particlesPrefab;
    [SerializeField] GameObject fallingGrenadeVisual;
    [SerializeField] float explosionDelay = 5f;

    private void Awake()
    {
        particlesPrefab.SetActive(false);
        fallingGrenadeVisual.SetActive(true);
        StartCoroutine(LiveTimeCountdown());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Damage")
        {
            StopCoroutine(LiveTimeCountdown());
            StartCoroutine(StartExplosion());
            particlesPrefab.SetActive(true);
            fallingGrenadeVisual.SetActive(false);
        }
    }

    IEnumerator LiveTimeCountdown()
    {
        yield return new WaitForSeconds(explosionDelay);
        Destroy(this.gameObject);
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
