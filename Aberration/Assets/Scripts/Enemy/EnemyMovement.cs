using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //important

public class EnemyMovement : PoolableObject
{
    public Transform Player;
    public float UpdateRate = 0.1f;
    public UnityEngine.AI.NavMeshAgent Agent;

    private Coroutine FollowCoroutine;

    public float IdleLocationRadius = 4f; //radius of sphere
    public float IdleMovespeedMultiplier = 0.5f;
    public Transform centerPoint; //centre of the area the agent wants to move around in
    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

    public EnemyState DefaultState;
    private EnemyState _state;
    public EnemyState State
    {
        get
        {
            return _state;
        }
        set
        {
            OnStateChange?.Invoke(_state, value);
            _state = value;
        }
    }

    public delegate void StateChangeEvent(EnemyState oldState, EnemyState newState);
    public StateChangeEvent OnStateChange;

    private void Awake()
    {
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        centerPoint = transform;
        OnStateChange += HandleStateChange;
        Agent.avoidancePriority = Random.Range(1, 50);
    }


    public void StartMovement()
    {
        OnStateChange?.Invoke(EnemyState.Spawn, DefaultState);
    }


    private void HandleStateChange(EnemyState oldState, EnemyState newState)
    {
        if (oldState != newState)
        {
            if (FollowCoroutine != null)
            {
                StopCoroutine(FollowCoroutine);
            }

            if (oldState == EnemyState.Idle)
            {
                Agent.speed /= IdleMovespeedMultiplier;
            }

            switch (newState)
            {
                case EnemyState.Idle:
                    FollowCoroutine = StartCoroutine(DoIdleMotion());
                    break;
                //case EnemyState.Patrol:
                //    FollowCoroutine = StartCoroutine(DoPatrolMotion());
                //    break;
                case EnemyState.Chase:
                    FollowCoroutine = StartCoroutine(FollowTarget());
                    break;
            }
        }
    }
 
    private IEnumerator DoIdleMotion() 
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        Agent.speed *= IdleMovespeedMultiplier;

       

        while (true)
        {
            if (!Agent.enabled || !Agent.isOnNavMesh)
            {
                yield return Wait;
            }
            else if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                Vector2 point = Random.insideUnitCircle * IdleLocationRadius;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(Agent.transform.position + new Vector3(point.x, 0, point.y), out hit, 2f, Agent.areaMask))
                {
                    Agent.SetDestination(hit.position);
                }
                else
                {
                    point = Random.insideUnitCircle * IdleLocationRadius;
                }
            }

            yield return Wait;
        }
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        while (true)
        {
            if (Agent.enabled)
            {
                Agent.SetDestination(Player.transform.position);
            }
            yield return Wait;
        }
    }


    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
        _state = DefaultState;
    }
}
