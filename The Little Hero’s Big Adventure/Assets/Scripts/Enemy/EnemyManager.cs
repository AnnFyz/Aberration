using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    public static EnemyManager Instance { get { return _instance; } }
    public Action OnAmountChange;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] int minAmountOfEnemies = 2;
    public int currentAmountOfEnemies = 0;

    public void setCurrentAmountOfEnemies(int enemiesAmount, bool isRespawn)
    {
        currentAmountOfEnemies += enemiesAmount;

    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        enemySpawner = GetComponent<EnemySpawner>();
        OnAmountChange += HandleAmountChange;
       
        
    }

    private void Start()
    {
        
    }

    void HandleAmountChange()
    {
        if (currentAmountOfEnemies <= minAmountOfEnemies)
        {
            enemySpawner.RespawnEnemies();

           
        }
    }
}
