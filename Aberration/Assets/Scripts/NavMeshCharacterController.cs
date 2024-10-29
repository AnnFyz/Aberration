using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class NavMeshCharacterController : MonoBehaviour
{

    [SerializeField] float speed = 10f;
    Vector3 inputValue = Vector3.zero;
    float inputSqrMagnitude;
    Vector3 moveDir;
    Vector3 lastDir;
    Rigidbody m_RB;
    UnityEngine.AI.NavMeshAgent m_Agent;
    RaycastHit m_HitInfo = new RaycastHit();
    float posY;
    [SerializeField] bool onNavMeshLink = false;


    public float jumpForce = 7f;
    [SerializeField] bool isGrounded;
    public float raycastDistance = 0.6f;
    void Start()
    {
        m_Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_RB = GetComponent<Rigidbody>();
        isGrounded = true;
        m_RB.isKinematic = true;
    }


    private void FixedUpdate()
    {
        StepWithRB();

    }



    void StepWithRB()
    {
        if (!isGrounded) return;
        inputSqrMagnitude = inputValue.sqrMagnitude;
        if (inputSqrMagnitude <= 0.1f) { m_Agent.velocity = new Vector3(0, m_RB.velocity.y, 0); return; }

        Vector3 velocity = moveDir * speed * Time.fixedDeltaTime;
        m_Agent.velocity = new Vector3(velocity.x, m_RB.velocity.y, velocity.z);
        if (velocity == Vector3.zero)
        { moveDir = lastDir; }
        else
        { lastDir = moveDir; }
        transform.rotation = Quaternion.LookRotation(moveDir);
    }

    void Step()
    {
        // Debug.Log("Step");
        inputSqrMagnitude = inputValue.sqrMagnitude;
        if (inputSqrMagnitude >= 0.05f)
        {
            Vector3 newPos = transform.position + inputValue * Time.deltaTime * speed;
            NavMeshHit hit;
            bool isValid = NavMesh.SamplePosition(newPos, out hit, .3f, NavMesh.AllAreas);
          
            if (isValid)
            {
                m_Agent.destination = newPos;
                //Debug.Log("isValid");
                //if ((transform.position - hit.position).magnitude >= .02f)
                //{
                //    m_Agent.destination = hit.position;
                //}
            }
        }
    }
    public void Move(InputAction.CallbackContext ctx)
    {
       
        Vector2 input2D = ctx.ReadValue<Vector2>();
        inputValue.x = input2D.x;
        inputValue.z = input2D.y;
        if (!isGrounded) return;
        moveDir = inputValue.normalized;
    }

    public void Jump()
    {
        m_RB.isKinematic = false;
        if (isGrounded)
        {
            isGrounded = false;

            if (m_Agent.enabled)
            {
                // set the agents target to where you are before the jump
                // this stops her before she jumps. Alternatively, you could
                // cache this value, and set it again once the jump is complete
                // to continue the original move
                //m_Agent.velocity = new Vector3(0, m_RB.velocity.y, 0);
                //NavMeshHit hit;
                // NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas);
                //m_Agent.SetDestination(transform.position);
                posY = transform.position.y;
                // disable the agent
                //GetComponent<NavMeshAgent>().SetDestination(transform.position);
                m_Agent.updatePosition = false;
                m_Agent.updateRotation = false;
                m_Agent.isStopped = true;
            }
            // make the jump
            m_RB.isKinematic = false;
            m_RB.useGravity = true;
            //m_RB.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            NavMeshJump();
        }



    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject.tag == "Ground")
        {
            if (!isGrounded)
            {
                m_RB.velocity = Vector3.zero;
                if (m_Agent.enabled)
                {

                    // m_Agent.velocity = new Vector3(0, 0, 0);
                    //Vector3 targetPos = new Vector3(transform.position.x, posY, transform.position.z);
                    //m_Agent.SetDestination(targetPos);
                    m_Agent.ResetPath();
                    m_Agent.updatePosition = true;
                    m_Agent.updateRotation = true;
                    m_Agent.isStopped = false;
                }

                m_RB.isKinematic = true;
                m_RB.useGravity = false;
                isGrounded = true;
            }
        }
    }

    public void NavMeshJump()
    {
        m_RB.AddForce(new Vector3(inputValue.x * 5, jumpForce, inputValue.z * 5), ForceMode.Impulse);

    }
}

