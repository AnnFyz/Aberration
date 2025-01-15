using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyHandler : MonoBehaviour
{
    [SerializeField] GameObject explosionParticlesPrefab;
    [SerializeField] float explosionDelay = 5f;
    [SerializeField] GameObject mesh;
    [SerializeField] int health = 100;
    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    private void Awake()
    {
        mesh.SetActive(true);
        explosionParticlesPrefab.SetActive(false);
        Movement = GetComponent<EnemyMovement>();
        Agent = GetComponent<NavMeshAgent>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Damage")
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
