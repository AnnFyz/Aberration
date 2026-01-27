using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyHandler : MonoBehaviour
{
    [Header("VFX Effects")]
    [SerializeField] GameObject explosionParticlesPrefab;
    [SerializeField] float explosionDelay = 2f;
    [SerializeField] GameObject mesh;
    [SerializeField] GameObject teleportationVFX;

    [Header("Health")]
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth = 100;

    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public Action OnEnemyDeath;

    public GameObject starPrefab;
    public bool hasStar;
    private void Awake()
    {
        mesh.SetActive(true);
        explosionParticlesPrefab.SetActive(false);
        Movement = GetComponent<EnemyMovement>();
        Agent = GetComponent<NavMeshAgent>();
        hasStar = false;
    }

    public void OnEnable()
    {
        mesh.SetActive(true);
        explosionParticlesPrefab.SetActive(false);
        Movement.State = EnemyState.Idle;
        currentHealth = maxHealth;
        StartCoroutine(StartTeleportation());
        //EnemyManager.Instance.CurrentAmountOfEnemies += 1;
    }


        private void Start()
    {
        currentHealth = maxHealth;
        starPrefab.SetActive(false);
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            ApplyDamage(maxHealth);

        }
    }


    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0 && Movement.State != EnemyState.Dead)
        {
            Debug.Log("Enemy is dead");
            mesh.SetActive(false);
            Movement.State = EnemyState.Dead;
            explosionParticlesPrefab.SetActive(true);
            Movement.chasingSign.SetActive(false);
            AudioManager.Instance.PlaySound("EnemyExplosion");
            if (gameObject.activeSelf)
            {
                StartCoroutine(StartExplosion());
            }
            if (hasStar)
            {
               Vector3 enemyPos = transform.position;
               var star = Instantiate(starPrefab, enemyPos + new Vector3(0,2,0), Quaternion.identity);
               star.transform.Rotate(new Vector3(-90, 0, 0), Space.Self);
               star.SetActive(true);
            }
        }
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        EnemyManager.Instance.currentAmountOfEnemies -= 1;
        EnemyManager.Instance.OnAmountChange?.Invoke();
        gameObject.SetActive(false);
    }

    IEnumerator StartTeleportation()
    {
        if(AudioManager.Instance != null) { AudioManager.Instance.PlaySound("EnemyTeleportation"); }
        teleportationVFX.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        teleportationVFX.SetActive(false);
    }
}
