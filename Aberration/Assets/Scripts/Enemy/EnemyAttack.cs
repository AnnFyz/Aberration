using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Coroutine AttackCoroutine;
    private void OnTriggerEnter(Collider other)
    {
        PlayerHandler player;
        if (other.TryGetComponent<PlayerHandler>(out player))
        {
            AttackCoroutine = StartCoroutine(StartAttack(player));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerHandler player;
        if (other.TryGetComponent<PlayerHandler>(out player))
        {
            Debug.Log("StopAttack");
            StopCoroutine(AttackCoroutine);
        }
    }

    IEnumerator StartAttack(PlayerHandler player)
    {
        Debug.Log("StartAttack");
        while (true)
        {
            player.ApplyDamage(10);
            Debug.Log("ApplyDamage");
            yield return new WaitForSeconds(1);
        }
    }
}
