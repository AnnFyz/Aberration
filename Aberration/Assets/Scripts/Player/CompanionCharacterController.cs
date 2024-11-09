using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CompanionCharacterController : MonoBehaviour
{
    CompanionControls companionControls;
    CharacterController characterController;
    Animator animator;

    //movement
    [Header("Movemet")]
    [SerializeField] float movementSpeed = 10.0f;
    [SerializeField] float rotationFactorPerFrame = 15.0f;

    int isMovingHash;  
    Vector2 currentInputMovement;
    Vector3 currentMovement;
    bool isMovePressed;

    //gravity
    float gravity = -9.5f;
    float groundedGravity = -0.05f;

    //jumping
    [Header("Jumping")]
    [SerializeField] bool isJumpPressed;
    [SerializeField] float initialJumpVelocity, maxJumpHeight = 1.0f, maxJumpTime = 0.5f;
    [SerializeField] bool isJumping = false;
    [SerializeField] bool isJumpAnimating = false;
    [SerializeField] int jumpCount = 0;
    int isJumpingHash;
    int jumpCountHash;
    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    Coroutine currentJumpResetCoroutine = null; 

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        companionControls = new CompanionControls();
        isMovingHash = Animator.StringToHash("IsMoving");
        isJumpingHash = Animator.StringToHash("IsJumping");
        jumpCountHash = Animator.StringToHash("JumpCount");

        //companionControls.CompanionCharacterControls.MoveCompanion.started += context => { Debug.Log(context.ReadValue<Vector2>()); };
        companionControls.CompanionCharacterControls.MoveCompanion.started += OnMovementInput;
        companionControls.CompanionCharacterControls.MoveCompanion.performed += OnMovementInput;
        companionControls.CompanionCharacterControls.MoveCompanion.canceled += OnMovementInput;
        companionControls.CompanionCharacterControls.MakeCompanionJump.started += OnJumpInput;
        companionControls.CompanionCharacterControls.MakeCompanionJump.canceled += OnJumpInput;

        SetupJumpVariables();
    }
    
    private void Update()
    {
        HandleRotation();
        HandleAnimation();
        characterController.Move(currentMovement * Time.deltaTime * movementSpeed);

        HandleGravity();
        HandleJump();

    }

    private void OnEnable()
    {
        companionControls.CompanionCharacterControls.Enable();
    }

    private void OnDisable()
    {
        companionControls.CompanionCharacterControls.Disable();
    }

    void OnMovementInput(InputAction.CallbackContext context) 
    {
        currentInputMovement = context.ReadValue<Vector2>();
        currentMovement.x = currentInputMovement.x;
        currentMovement.z = currentInputMovement.y;
        isMovePressed = currentInputMovement.x != 0 || currentInputMovement.y != 0;
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void HandleAnimation()
    {
        bool isWalking = animator.GetBool(isMovingHash);

        if(isMovePressed && !isWalking)
        {
            animator.SetBool(isMovingHash, true);
        }
        else if (!isMovePressed && isWalking)
        {
            animator.SetBool(isMovingHash, false);
        }
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation= transform.rotation;

        if (isMovePressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }

    }

    void HandleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2f;
        if (characterController.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
                currentJumpResetCoroutine = StartCoroutine(JumpResetCoroutine());
                if(jumpCount == 3)
                {
                    jumpCount = 0;
                    animator.SetInteger(jumpCountHash, jumpCount);
                }
            }
            currentMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y; 
            float newYVelocity = currentMovement.y + (jumpGravities[jumpCount] * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -20f); //Mathf.Max for velocity clamp
            currentMovement.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (jumpGravities[jumpCount] * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) *0.5f;
            currentMovement.y = nextYVelocity;
        }
    }

    void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;

        float secondJumpGravity = (-2 * (maxJumpHeight + 2)) / Mathf.Pow((timeToApex * 1.25f), 2);
        float secondJumpInitialGravity = (2 * (maxJumpHeight + 2)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (maxJumpHeight + 4)) / Mathf.Pow((timeToApex * 1.5f), 2);
        float thirdJumpInitialGravity = (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialGravity);
        initialJumpVelocities.Add(3, thirdJumpInitialGravity);

        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);
    }

    void HandleJump()
    {
        if(!isJumping && characterController.isGrounded && isJumpPressed)
        {
            if(jumpCount >= 2) { Debug.Log("JumpCount: " + jumpCount); }
            if(jumpCount < 3 && currentJumpResetCoroutine != null)
            {
                StopCoroutine(JumpResetCoroutine());
            }
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            if(jumpCount < 3)
            {
                jumpCount += 1;
            }
            animator.SetInteger(jumpCountHash, jumpCount);
            currentMovement.y = initialJumpVelocities[jumpCount] * .5f; 
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false; 
        }
    }

    IEnumerator JumpResetCoroutine()
    {
        yield return new WaitForSeconds(1f);
        jumpCount = 0;
    }
}
