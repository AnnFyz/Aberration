using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyHandler : MonoBehaviour
{
    [SerializeField] GameObject explosionParticlesPrefab;
    [SerializeField] float explosionDelay = 2f;
    [SerializeField] GameObject mesh;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth = 100;

    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public Action OnEnemyDeath;

    public GameObject star;
    public bool hasStar;
    private void Awake()
    {
        mesh.SetActive(true);
        explosionParticlesPrefab.SetActive(false);
        Movement = GetComponent<EnemyMovement>();
        Agent = GetComponent<NavMeshAgent>();
    }

    public void OnEnable()
    {
        mesh.SetActive(true);
        explosionParticlesPrefab.SetActive(false);
        Movement.State = EnemyState.Idle;
        currentHealth = maxHealth;
        //EnemyManager.Instance.CurrentAmountOfEnemies += 1;
    }


        private void Start()
    {
        currentHealth = maxHealth;
        star.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            ApplyDamage(100);
           
        }
    }


    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0 && Movement.State != EnemyState.Dead)
        {
            Debug.Log("Enemy is dead");
            mesh.SetActive(false);
            explosionParticlesPrefab.SetActive(true);
            StartCoroutine(StartExplosion());
            Movement.State = EnemyState.Dead;
            //if (hasStar)
            //{
            //    star.SetActive(true);
            //}
        }
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        EnemyManager.Instance.currentAmountOfEnemies -= 1;
        EnemyManager.Instance.OnAmountChange?.Invoke();
        gameObject.SetActive(false);
    }

}
