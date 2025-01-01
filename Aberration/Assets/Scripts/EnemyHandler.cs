using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] GameObject explosionParticlesPrefab;
    [SerializeField] float explosionDelay = 5f;
    [SerializeField] GameObject mesh;
    private void Awake()
    {
        mesh.SetActive(true);
        explosionParticlesPrefab.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Damage")
        {
            mesh.SetActive(false);
            explosionParticlesPrefab.SetActive(true);
            StartCoroutine(StartExplosion());
            Debug.Log("Exploded!!!");
        }
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        Destroy(this.gameObject);
    }
}
