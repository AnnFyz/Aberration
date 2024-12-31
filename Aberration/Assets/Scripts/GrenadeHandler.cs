using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeHandler : MonoBehaviour
{
    [SerializeField] GameObject particlesPrefab;
    [SerializeField] float explosionDelay = 1f;

    private void Awake()
    {
        particlesPrefab.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            particlesPrefab.SetActive(true);
            GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(StartExplosion());
        }
    }
    IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        Destroy(this.gameObject);
    }

  
}
