using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    [SerializeField] GameObject startParticlesPrefab;
    [SerializeField] GameObject explosionParticlesPrefab;
    [SerializeField] float explosionDelay = 5f;
    private void Awake()
    {
        explosionParticlesPrefab.SetActive(false);
        startParticlesPrefab.SetActive(true);
        StartCoroutine(LiveTimeCountdown());
    }
    private void OnCollisionEnter(Collision collision)
    {
        StopCoroutine(LiveTimeCountdown());
        StartCoroutine(StartExplosion());
        GetComponent<Rigidbody>().isKinematic = true;
        explosionParticlesPrefab.SetActive(true);
        startParticlesPrefab.SetActive(false);

    }

    IEnumerator LiveTimeCountdown()
    {
        yield return new WaitForSeconds(explosionDelay);
        Destroy(this.gameObject);
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
