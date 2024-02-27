using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AnimationandMovementController : MonoBehaviour
{

    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;
    Rigidbody rb;
    bool Variable;
    public AudioSource footstepsSound, sprintSound;

    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    bool isJumpAnimating = false;

    Vector2 currentMovementInput;
    
    Vector2 look;
    internal Vector3 velocity;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    public float rotationFactorPerFrame = 15.0f;
    private float runmultiplier =15f;
    private float horizontalMove;

    // gravity varibles
    private float gravity = -9.8f;
    private float groundedGravity = -.05f;
    bool _Gravity;

    // jumping varibles
    bool isJumpPressed = false;
    float initiaJumpVelocity;
    float maxJumpHeight = 3f;
    float maxJumpTime = 0.5f;
    bool isJumping = false;


    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        SetupJumpVariables();
    }

    

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        Physics.SyncTransforms();
        look.x = rotation.eulerAngles.y;
        look.y = rotation.eulerAngles.z;
        velocity = Vector3.zero;
    }

    void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initiaJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = initiaJumpVelocity;
            currentRunMovement.y = initiaJumpVelocity;
            JumpStep.instance.PlayJump();
        }
        else if(!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }


    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.z = currentMovement.z;
        positionToLookAt.y = 0.0f;
        positionToLookAt.x = currentMovement.x;
        Quaternion currentRotation = transform.rotation;

        if(isMovementPressed)
        {
        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }

    }

   void onMovementInput(InputAction.CallbackContext context)
    {
        Debug.Log("run");
            currentMovementInput = context.ReadValue<Vector2>();
            currentMovement.x = currentMovementInput.x * 7f;
            currentMovement.z = currentMovementInput.y * 7f;
            currentRunMovement.x = currentMovementInput.x * runmultiplier;
            currentRunMovement.z = currentMovementInput.y * runmultiplier;
            isMovementPressed = currentMovementInput.x != 0 || currentMovement.z != 0;
    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if ((!isMovementPressed || !isRunPressed) || !isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f;
        float fallMultiplier = 2.0f;
        if (characterController.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
            }
            
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentMovement.y += gravity * Time.deltaTime;
            currentRunMovement.y += Time.deltaTime;
        }
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)
           || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow)
           || Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                footstepsSound.enabled = false;
                sprintSound.enabled = true;
            }
            else
            {
                footstepsSound.enabled = true;
                sprintSound.enabled = false;
            }
        }
        else
        {
            footstepsSound.enabled = false;
            sprintSound.enabled = false;
        }

        //GravityHandler();
        handleRotation();
        handleAnimation();

        if(isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else 
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }
        handleGravity();
        handleJump();

    }

    void  OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
    //Gravity Handler
    private void GravityHandler()
    {
        
        bool isFalling = currentMovement.y <= 0.0f;
        float fallMultiplier = 2.0f;
        if (characterController.isGrounded)
        {

            float groundedGravity = -0.05f;
            currentMovement.y = groundedGravity;

        }
        else if (isFalling)
        {

            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -20.0f);
            currentMovement.y = nextYVelocity;

        }
        else
        {

            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity;

        }
} 
}
