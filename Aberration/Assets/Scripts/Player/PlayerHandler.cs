using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth = 100;
    [SerializeField] Image uiHealth;
    [SerializeField] GameObject heart;
    public int index = 0;
    [SerializeField] bool isSafe = false;
    [SerializeField] GameObject inDangerVFX;
    [SerializeField] GameObject teleportationVFX;
    private void Start()
    {
        currentHealth = maxHealth;
        heart.SetActive(false);
        SetVFXInDanger(false);
    }

    public void ApplyDamage(float damage)
    { 
        currentHealth -= damage;
        heart.SetActive(true);
        uiHealth.fillAmount = currentHealth / maxHealth;
        isSafe = false;
        SetVFXInDanger(true);
        StartCoroutine(CheckIfItSafe());
        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead");
            QuestManager.Instance.StartBadEndingScene();
        }
    }

    IEnumerator CheckIfItSafe()
    {
        yield return new WaitForSeconds(5);
        isSafe = true;
        StartCoroutine(StartHealing());
    }

    IEnumerator StartHealing()
    {
        SetVFXInDanger(false);
        while (currentHealth < maxHealth)
        {
            yield return new WaitForSeconds(1);
            if (isSafe) currentHealth += 1;
            uiHealth.fillAmount = currentHealth / maxHealth;
        }
        currentHealth = maxHealth;
        uiHealth.fillAmount = currentHealth / maxHealth;
        heart.SetActive(false);
        yield return new WaitForSeconds(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            var enemy = GameObject.FindGameObjectWithTag("Enemy");
            enemy.GetComponent<EnemyHandler>().ApplyDamage(100);
        }
    }

   void SetVFXInDanger(bool isInDanger)
    {
        if (isInDanger)
        {
            inDangerVFX.SetActive(true);
        }
        else
        {
            inDangerVFX.SetActive(false);
        }
    }

    public IEnumerator StartTeleportationVFX()
    {
        teleportationVFX.SetActive(true);
        yield return new WaitForSeconds(1.75f);
        teleportationVFX.SetActive(false);
    }
}


