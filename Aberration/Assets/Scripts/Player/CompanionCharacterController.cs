using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CompanionCharacterController : MonoBehaviour
{
    CompanionControls companionControls;
    CharacterController characterController;
    Animator animator;

    // variables to store optimized getter/setter IDs
    int isMovingHash;

    //movement
    [Header("Movemet")]
    [SerializeField] float movementSpeed = 10.0f;
    [SerializeField] float rotationFactorPerFrame = 15.0f;

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

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        companionControls = new CompanionControls();
        isMovingHash = Animator.StringToHash("IsMoving");

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
        if (characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;
        }
        else
        {
            currentMovement.y += gravity;
        }
    }

    void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void HandleJump()
    {
        if(!isJumping && characterController.isGrounded && isJumpPressed)
        {
            isJumping = true;
            currentMovement.y = initialJumpVelocity; 
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false; 
        }
    }
}
