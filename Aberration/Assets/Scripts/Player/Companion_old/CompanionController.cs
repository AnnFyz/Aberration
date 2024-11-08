using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.AI;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class CompanionController : MonoBehaviour
{
    [SerializeField] XRRayInteractor rayInteractor;
    [SerializeField] NearFarInteractor rayInteractorNearFar;
    NavMeshAgent m_Agent;
    Vector3 endPoint;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }


    public void Move()
    {
       
        if (rayInteractor.TryGetCurrent3DRaycastHit(out var raycastHit))
            m_Agent.destination = raycastHit.point;

        
    }

    public void MoveWithNearFarInteractor()
    {
        rayInteractorNearFar.TryGetCurveEndPoint(out Vector3 endPoint);
        m_Agent.destination = endPoint;
    }
}
