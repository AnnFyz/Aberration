using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //important

public class EnemyMovement : PoolableObject
{
    [Header("Movemet")]
    public Transform Player;
    public float UpdateRate = 0.1f;
    public UnityEngine.AI.NavMeshAgent Agent;
    public EnemyLineOfSightChecker LineOfSightChecker;
    private Coroutine FollowCoroutine;

    [Header("State settings")]
    public EnemyState DefaultState;
    [SerializeField] GameObject chasingSign;
    [SerializeField] GameObject attackingParticles;
    [SerializeField] float chasingSpeed;
    [SerializeField] float attackingDistance = 5f;

    [Header("Idle")]
    public float IdleLocationRadius = 4f; //radius of sphere
    public float idleSpeed;
    public Transform centerPoint; //centre of the area the agent wants to move around in


    [SerializeField] EnemyState _state;
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
        //instead of centrePoint it can be set as the transform of the agent if you don't care about a specific area
        centerPoint = transform;
        OnStateChange += HandleStateChange;
        Agent.avoidancePriority = Random.Range(1, 50);
        LineOfSightChecker.OnGainSight += HandleGainSight;
        LineOfSightChecker.OnLoseSight += HandleLoseSight;
        chasingSign.SetActive(false);
        attackingParticles.SetActive(false);
    }

    private void Start()
    {
        idleSpeed = Agent.speed * 1f;
        chasingSpeed = Agent.speed * 10f;
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

  
            switch (newState)
            {
                case EnemyState.Idle:
                    FollowCoroutine = StartCoroutine(DoIdleMotion());
                    break;
                case EnemyState.Chase:
                    FollowCoroutine = StartCoroutine(FollowTarget());
                    break;
                case EnemyState.Dead:
                    attackingParticles.SetActive(false);
                    chasingSign.SetActive(false);
                    break;
            }
        }
    }
 
    private IEnumerator DoIdleMotion() 
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        Agent.speed  = idleSpeed;

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
        Agent.speed = chasingSpeed;
        if(Vector3.Distance(transform.position, Player.position) < 5f)
        {
            attackingParticles.SetActive(true);
        }
        else
        {
            attackingParticles.SetActive(false);
        }
            while (true)
            {
                if (Agent.enabled)
                {
                    Agent.SetDestination(Player.transform.position);

                }
                yield return Wait;
            }
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }
    private void HandleGainSight(CompanionCharacterController player)
    {
        chasingSign.SetActive(true);
        State = EnemyState.Chase;
    }

    private void HandleLoseSight(CompanionCharacterController player)
    {
        attackingParticles.SetActive(false);
        chasingSign.SetActive(false);
        State = DefaultState;
       
    }


    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
        _state = DefaultState;
    }
}
