using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlatformInteractable : XRBaseInteractable
{

    [Header("Platform Handle Data")]
    public Transform draggedTransform;
    public Transform endPosition;

    public Vector3 localDragDirection;
    public float dragDistance;

    //arbitrary unit, not matching any "physic" reality, just a factor of how "heavy" the door is to pull
    public int platformWeight = 20;

    [Header("Auto Movement")]
    [SerializeField] bool isAutoMoving = false;
    [SerializeField] int rotationDir = 1;
    [SerializeField] Destination currentDestination = Destination.endPosition;
    [SerializeField] public float amplitude = 1;
    [SerializeField] public float speed = .5f;

    enum Destination
    {
        startPosition,
        endPosition
    }

    // ================== EXTENSION FOR THE VISUAL LINE ==========================
    [Header("Visual References")]
    public LineRenderer handleToHandLine;
    public LineRenderer dragVectorLine;
    //============================================================================

    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;

    private Vector3 m_WorldDragDirection;

    private void Start()
    {
        //we cache that, meaning we don't expect the draggable to rotate during its lifetime. If we wanted to support that,
        //we would need to transform from local to world in the ProcessInteractable every frame.
        m_WorldDragDirection = transform.TransformDirection(localDragDirection).normalized;


        //we store the start and end position of the drag, as the object will move as it is dragged
        m_StartPosition = draggedTransform.position;
        //m_EndPosition = m_StartPosition + m_WorldDragDirection * dragDistance;
        m_EndPosition = endPosition.position;

        //auto move
        currentDestination = Destination.endPosition;
        amplitude += UnityEngine.Random.Range(-0.25f, 0.25f);
        speed += UnityEngine.Random.Range(-0.25f, 0.25f);


        // ================== EXTENSION FOR THE VISUAL LINE ==========================
        handleToHandLine.gameObject.SetActive(false);
        dragVectorLine.gameObject.SetActive(false);
        //============================================================================
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed && isSelected)
        {
            var interactorTransform = firstInteractorSelecting.GetAttachTransform(this);
            Debug.Log("GetAttachTransform");

            //we get the vector that goes from this to the interactor
            Vector3 selfToInteractor = interactorTransform.position - transform.position;

            //project onto the movement vector
            float forceInDirectionOfDrag = Vector3.Dot(selfToInteractor, m_WorldDragDirection);

            //we then need to check in which direction are we dragging : toward the end (positive direction) or toward
            //the start (megative direction)
            bool dragToEnd = forceInDirectionOfDrag > 0.0f;

            //we take the absolute of that value now, as we need a speed, not a direction anymore
            float absoluteForce = Mathf.Abs(forceInDirectionOfDrag);

            //we transform our force into a speed (by dividing it by delta Time). Then we "scale" that speed by the door
            //weight. The "heavier" the door, the lower the speed will be.
            float speed = absoluteForce / Time.deltaTime / platformWeight;

            //finally we move the target either toward end or start based on the speed.
            draggedTransform.position = Vector3.MoveTowards(draggedTransform.position,
                //the target depend on the direction of drag we recovered earlier
                dragToEnd ? m_EndPosition : m_StartPosition,
                speed * Time.deltaTime);

            // ================== EXTENSION FOR THE VISUAL LINE ==========================

            //handleToHandLine.SetPosition(0, transform.position);
            //handleToHandLine.SetPosition(1, interactorTransform.position);

            ////to be sure to see it we offset it a bit back on x so it is not IN the door 
            //dragVectorLine.SetPosition(0, transform.position);
            //dragVectorLine.SetPosition(1, transform.position + forceInDirectionOfDrag * m_WorldDragDirection);

            // ===========================================================================
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 worldDirection = transform.TransformDirection(localDragDirection);
        //make sure this is a unit vector, so when we multiply by the drag distance we get the right distance.
        worldDirection.Normalize();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + worldDirection * dragDistance);
    }


    // ================== EXTENSION FOR THE VISUAL LINE ==========================
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        handleToHandLine.gameObject.SetActive(false);
        dragVectorLine.gameObject.SetActive(false);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        handleToHandLine.gameObject.SetActive(false);
        dragVectorLine.gameObject.SetActive(false);
    }
    //============================================================================

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }

    private void Update()
    {
        if (!isSelected)
        {
            Debug.Log("AutoMove");
            AutoMove();
        }
    }

    void AutoMove()
    {
        Vector3 p = transform.position;
        p.y = amplitude * Mathf.Cos(Time.time * speed);
        transform.position = p;
        if (!isAutoMoving)
        {
            isAutoMoving = true;
            //StartCoroutine(AutoMovingCoroutine());

        }
    }

    IEnumerator AutoMovingCoroutine()
    {
        yield return new WaitForSeconds(0.025f);
        if (Vector3.Distance(draggedTransform.position, m_EndPosition) > 7f && currentDestination != Destination.startPosition) //to overrride this condition
        {
            Vector3 destination = m_EndPosition;
            draggedTransform.position = Vector3.MoveTowards(draggedTransform.position, destination, 5 * Time.deltaTime);
            Debug.Log("MoveTowards: m_EndPosition");
        }
        else
        {
            if(Vector3.Distance(draggedTransform.position, m_StartPosition) > .5f)
            {
                currentDestination = Destination.startPosition;
                Vector3 destination = m_StartPosition;
                draggedTransform.position = Vector3.MoveTowards(draggedTransform.position, destination, 5 * Time.deltaTime);
                Debug.Log("MoveTowards: m_StartPosition");
            }
            else
            {
                currentDestination = Destination.endPosition;
            }
           
        }

       
        isAutoMoving = false;

    }
}
