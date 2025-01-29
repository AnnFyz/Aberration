using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class QuestManager : MonoBehaviour
{
    public int StarsToComplete = 3;
    private static QuestManager _instance;
    public static QuestManager Instance { get { return _instance; } }
    public Action OnAmountChange;
    int _amountOfCollectedStars = 0;
    [SerializeField] GameObject PortalTrigger;
    PlayableDirector playableDirector;
    [SerializeField] GameObject[] bloomWindows;
    public int AmountOfCollectedStars
    {
        get
        {
            return _amountOfCollectedStars;
        }
        set
        {
            if(AmountOfCollectedStars <= StarsToComplete)
            {
                OnAmountChange?.Invoke();
                _amountOfCollectedStars = value;
            }
            
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
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        foreach (var window in bloomWindows)
        {
            window.SetActive(false);
        }
        PortalTrigger.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Quest is completed");
            playableDirector.Play();
            PortalTrigger.SetActive(true);
        }
    }

    void HandleAmountChange()
    {
        Debug.Log("AmountOfCollectedStars" + AmountOfCollectedStars);
        bloomWindows[AmountOfCollectedStars].SetActive(true);
        if (AmountOfCollectedStars >= StarsToComplete - 1)
        {
            Debug.Log("Quest is completed");
            PortalTrigger.SetActive(true);
            playableDirector.Play();
        }
    }

    public void StartGoodEndingScene()
    {
        SceneManager.LoadScene(4);
    }

    public void StartBadEndingScene()
    {
        SceneManager.LoadScene(3);
    }
}
