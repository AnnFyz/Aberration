using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyLineOfSightChecker : MonoBehaviour
{
    public SphereCollider Collider;
    public float FieldOfView = 90f;
    public LayerMask LineOfSightLayers;
    [SerializeField] Transform eyes;

    public delegate void GainSightEvent(CompanionCharacterController player);
    public GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(CompanionCharacterController player);
    public LoseSightEvent OnLoseSight;

    private Coroutine CheckForLineOfSightCoroutine;

    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CompanionCharacterController player;
        if (other.TryGetComponent<CompanionCharacterController>(out player))
        {
            if (!CheckLineOfSight(player))
            {
                CheckForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(player));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CompanionCharacterController player;
        if (other.TryGetComponent<CompanionCharacterController>(out player))
        {
            if (!CheckLineOfSight(player))
            {
                CheckForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(player));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CompanionCharacterController player;
        if (other.TryGetComponent<CompanionCharacterController>(out player))
        {
            Debug.Log("OnLoseSight");
            OnLoseSight?.Invoke(player);
            if (CheckForLineOfSightCoroutine != null)
            {
                StopCoroutine(CheckForLineOfSightCoroutine);
            }
        }
    }

    private bool CheckLineOfSight(CompanionCharacterController player)
    {
        Vector3 Direction = (player.transform.position - transform.position).normalized;
        float DotProduct = Vector3.Dot(eyes.forward, Direction);
        if (DotProduct >= Mathf.Cos(FieldOfView))
        {
            RaycastHit Hit;

            if (Physics.Raycast(eyes.position, Direction, out Hit, Collider.radius*5, LineOfSightLayers))
            {
                if (Hit.transform.GetComponent<CompanionCharacterController>() != null)
                {
                    Debug.Log("OnGainSight");
                    OnGainSight?.Invoke(player);
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerator CheckForLineOfSight(CompanionCharacterController player)
    {
        WaitForSeconds Wait = new WaitForSeconds(0.1f);

        while (!CheckLineOfSight(player))
        {
            yield return Wait;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(eyes.position, eyes.forward);
    }
}
