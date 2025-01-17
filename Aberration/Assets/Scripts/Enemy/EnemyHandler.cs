using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyHandler : MonoBehaviour
{
    [SerializeField] GameObject explosionParticlesPrefab;
    [SerializeField] float explosionDelay = 5f;
    [SerializeField] GameObject mesh;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth = 100;

    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public Action OnEnemyDeath;
    private void Awake()
    {
        mesh.SetActive(true);
        explosionParticlesPrefab.SetActive(false);
        Movement = GetComponent<EnemyMovement>();
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            ApplyDamage(50);
           
        }
    }

    void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            EnemyManager.Instance.CurrentAmountOfEnemies -= 1;
            Debug.Log("Enemy is dead");
            mesh.SetActive(false);
            explosionParticlesPrefab.SetActive(true);
            StartCoroutine(StartExplosion());
            Movement.State = EnemyState.Dead;
        }
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        Destroy(this.gameObject);
    }

}
