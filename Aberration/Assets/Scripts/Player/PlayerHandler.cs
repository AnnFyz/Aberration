using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHandler : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth = 100;
    public int index = 0;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead");
            QuestManager.Instance.StartBadEndingScene();

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            var enemy = GameObject.FindGameObjectWithTag("Enemy");
            enemy.GetComponent<EnemyHandler>().ApplyDamage(100);
        }
    }

   
}


