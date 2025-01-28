using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int StarsToComplete = 3;
    private static QuestManager _instance;
    public static QuestManager Instance { get { return _instance; } }
    public Action OnAmountChange;
    int _amountOfCollectedStars = 0;
    public int AmountOfCollectedStars
    {
        get
        {
            return _amountOfCollectedStars;
        }
        set
        {
            OnAmountChange?.Invoke();
            _amountOfCollectedStars = value;
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

        OnAmountChange += HandleAmountChange;

    }

    void HandleAmountChange() {
        Debug.Log("AmountOfCollectedStars" + AmountOfCollectedStars);
    if(AmountOfCollectedStars >= StarsToComplete-1)
        {
            Debug.Log("Quest is completed");
        }
    }
}
