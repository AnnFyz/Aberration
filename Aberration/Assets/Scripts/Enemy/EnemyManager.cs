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
    [SerializeField] int _currentAmountOfEnemies = 0;
    public int CurrentAmountOfEnemies
    {
        get
        {
            return _currentAmountOfEnemies;
        }
        set
        {
            OnAmountChange?.Invoke();
            _currentAmountOfEnemies = value;
        }
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
        _currentAmountOfEnemies = enemySpawner.NumberOfEnemiesToSpawn;
        OnAmountChange += HandleAmountChange;
       
        
    }

    private void Start()
    {
        
    }

    void HandleAmountChange()
    {
        if (CurrentAmountOfEnemies <= minAmountOfEnemies)
        {
            
            Debug.Log("Respawn");
        }
    }
}
