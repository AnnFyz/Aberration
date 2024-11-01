using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public enum PlayerState
{
    Idle,
    Move,
    Jump,
    InJump,
    Land
}

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class NavMeshCharacterController : MonoBehaviour
{
    public float gravityScale = 1f; //The gravity scale
    [SerializeField] PlayerState currentPlayerState;
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
    public bool wasJumpEnded = false;
    [SerializeField] GroundChecker groundChecker;

    public float jumpForce = 7f;
    [SerializeField] bool isGrounded;
    public float raycastDistance = 0.6f;
    [SerializeField] LayerMask groundLayers;
    [SerializeField] float sekBeforeCheck = 2f;
    [SerializeField] bool isLanded = false;
    void Start()
    {
        m_Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_RB = GetComponent<Rigidbody>();
        isGrounded = true;
        m_RB.isKinematic = true;
        groundChecker = GetComponent<GroundChecker>();
    }


    private void FixedUpdate()
    {
        StepWithRB();
        m_RB.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }


    void StepWithRB()
    {
        if (currentPlayerState == PlayerState.Jump || currentPlayerState == PlayerState.InJump) return;
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
        if (groundChecker.IsGrounded && currentPlayerState != PlayerState.Jump)
        {
            currentPlayerState = PlayerState.Jump;

            if (m_Agent.enabled)
            {
                // set the agents target to where you are before the jump
                // this stops her before she jumps. Alternatively, you could
                // cache this value, and set it again once the jump is complete
                // to continue the original move
                //m_Agent.velocity = new Vector3(0, m_RB.velocity.y, 0);
                NavMeshHit hit;
                //NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas);
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
           // m_RB.useGravity = true;
            //m_RB.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            NavMeshJump();
           // if (!wasJumpCheckStarted) { }
            StartCoroutine(Check());
            Debug.Log("StartCoroutine");
        }
    }

    void CheckIfLanding()
    {
       
        m_RB.velocity = Vector3.zero;

        m_RB.isKinematic = true;
        //m_RB.useGravity = false;


        if (m_Agent.enabled)
        {

            // m_Agent.velocity = new Vector3(0, 0, 0);
            //Vector3 targetPos = new Vector3(transform.position.x, posY, transform.position.z);
            //m_Agent.SetDestination(targetPos);
            //m_Agent.ResetPath();
           
            Debug.Log("Warp:" + m_Agent.Warp(new Vector3(transform.position.x, posY, transform.position.z)));
            if (m_Agent.Warp(new Vector3(transform.position.x, posY, transform.position.z)))
            {
                m_Agent.Warp(new Vector3(transform.position.x, posY, transform.position.z));
            }
            else
            {
                NavMeshHit hitNavMesh;
                NavMesh.SamplePosition(new Vector3(transform.position.x, posY, transform.position.z), out hitNavMesh, 100f, NavMesh.AllAreas);
                m_Agent.Warp(hitNavMesh.position);
            }

            m_Agent.updatePosition = true;
            m_Agent.updateRotation = true;
            m_Agent.isStopped = false;
        }
        currentPlayerState = PlayerState.Land;

    }

    IEnumerator Check()
    {
        yield return new WaitForSeconds(sekBeforeCheck);
        currentPlayerState = PlayerState.InJump;
        while (!groundChecker.IsGrounded)
        {
            yield return new WaitForSeconds(0.25f);
           
        }
        Debug.Log("CheckIfLanding");
        CheckIfLanding();
    }

    public void NavMeshJump()
    {
        m_RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    }
}

