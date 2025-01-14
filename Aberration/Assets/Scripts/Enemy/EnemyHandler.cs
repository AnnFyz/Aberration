using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyHandler : AutoDestroyPoolableObject
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
        setAutoDestroyTime(1);
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.tag == "Damage")
    //    {
    //        mesh.SetActive(false);
    //        explosionParticlesPrefab.SetActive(true);
    //        StartCoroutine(StartExplosion());
    //        Debug.Log("Exploded!!!");
    //    }
    //}

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

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }

    public override void setAutoDestroyTime(float newTime)
    {
        base.setAutoDestroyTime(newTime);
    }
}
