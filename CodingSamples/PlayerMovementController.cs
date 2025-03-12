using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrocodileBrackets;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public static PlayerMovementController instantOfPlayerMovementControllerScript;

    [Header("Player References")]
    [HideInInspector] public PlayerActions inputActions;

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Rigidbody playerRigidbody;
    Vector3 moveDirection;
    [SerializeField] Camera playerCamera;


    [Header("Movement")]
    public float playerMovementSpeed;
    [SerializeField] float playerRotateSpeed;

    [Header("Type Of Movement")]
    public float defaultMovementSpeed;
    public float sprintMovementSpeed;
    public float inAirMovementSpeed;
    public float inWaterMovementSpeed;

    [Header("Jump Info")]
    [SerializeField] float playerJumpMultiplier;
    [SerializeField] float playerFlipMultiplier;
    [SerializeField] float gravityScale;
    [SerializeField] float gravityTiming;

    public int currentJumpCount;
    public int maxjumpCount;

    [Header("type of Jump")]
    public float defaultJumpHeight;
    public float waterJumpHeight;


    [Header("GroundCheck Info")]
    [SerializeField] Transform overlapPosition;
    [SerializeField] float overlapRadius;
    [SerializeField] GameObject raycastGroundCheck;
    [SerializeField] LayerMask groundLayer;
    [Header("Check Info")]
    [SerializeField] bool isGrounded;

    [SerializeField] float raycastDistendsIsGrounded;
    //[SerializeField] float raycastBufferDistendsIsGrounded = 0.3f;

    [SerializeField] bool isInAir;
    [SerializeField] bool isSprinting;
    public bool isInWater;

    [Header("slopeHit")]
    RaycastHit slopeHit;

    [Header("Animations")]
    public Animator playerAnimator;




    private void Awake()
    {
        instantOfPlayerMovementControllerScript = this;

        Application.targetFrameRate = 120;

        CursorSettings(false, CursorLockMode.Locked);
        playerCamera = Camera.main;
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;


        //InputAction SetUp
        inputActions = new PlayerActions();

        inputActions.Enable();
        //Move
        inputActions.playerInputActions.Move.performed += OnMovePerformed;
        inputActions.playerInputActions.Move.canceled += OnMoveCancelled;
        //Jump
        inputActions.playerInputActions.Jump.performed += OnJumpPreformed;
        //Sprint
        inputActions.playerInputActions.Sprint.performed += OnSprintPreformed;
        inputActions.playerInputActions.Sprint.canceled += OnSprintCanceled;
        //TAB UI
        //            inputActions.playerUI.UI.performed += PlayerUIScript.instantOfPlayerUIScript.OnPlayerUIperformed;
        //           inputActions.playerUI.UI.canceled += PlayerUIScript.instantOfPlayerUIScript.OnPlayerUIcanceled;
        //Jetpack
        //   inputActions.playerInputActions.JetpackHover.performed += Jetpack.instantOfJetpack.OnJetpackPreformed;
        //   inputActions.playerInputActions.JetpackHover.canceled += Jetpack.instantOfJetpack.OnJetpackCanceled;


    }

    private void Start()
    {

        OnGroundCheck();
        MovementSpeedChanges(defaultMovementSpeed);
        JumpMultiplierChanges(defaultJumpHeight);
        //raycastDistendsIsGrounded = (GetComponent<CapsuleCollider>().height / 2) + raycastBufferDistendsIsGrounded;

    }

    private void FixedUpdate()
    {

        OnGroundCheck();

        if ((playerRigidbody.velocity.y < gravityTiming && !Jetpack.instantOfJetpack.isHovering && !WallClimingScript.instanceOfWallClimbingScript.isClimbing)
        || (!isGrounded && isInAir && !Jetpack.instantOfJetpack.isHovering && !WallClimingScript.instanceOfWallClimbingScript.isClimbing)
        || (!Jetpack.instantOfJetpack.isHovering && !WallClimingScript.instanceOfWallClimbingScript.isClimbing)) // If Velocity is Negative player needs to fall
        {
            Gravity();
        }



        OnMove();

    }




    private void CursorSettings(bool cursorVisibility, CursorLockMode cursorLockState)
    {
        Cursor.visible = cursorVisibility;
        Cursor.lockState = cursorLockState;
    }

    private void Gravity()
    {

        playerRigidbody.velocity += Vector3.up * Physics.gravity.y * gravityScale * Time.fixedDeltaTime;

    }




    //Movement & Jump Changes
    public void MovementSpeedChanges(float movementChanges)
    {
        // Debug.Log("MovementSpeedChanges Called ");
        playerMovementSpeed = movementChanges;


    }
    public void JumpMultiplierChanges(float JumpMultiplierChanges)
    {
        playerJumpMultiplier = JumpMultiplierChanges;
    }



    //Move Functions
    public void OnMovePerformed(InputAction.CallbackContext incomingValue)
    {
        moveInput = incomingValue.ReadValue<Vector2>();
        if (isGrounded)
        {
            if (!isSprinting)
            {
                playerAnimator.SetFloat("Speed", 1f);
            }
            AudioScript.InstanceOfAudioScript.rockFootStepsSound.setParameterByName("Speed", PlayerMovementController.instantOfPlayerMovementControllerScript.playerMovementSpeed);
        }

        if (WallClimingScript.instanceOfWallClimbingScript.isClimbing)
        {

            playerAnimator.SetFloat("Speed", moveInput.y);
            playerAnimator.SetFloat("Direction", moveInput.x);
        }

    }
    public void OnMoveCancelled(InputAction.CallbackContext incomingValue)
    {
        moveInput = Vector2.zero;
        //playerRigidbody.velocity = Vector3.zero;
        playerAnimator.SetFloat("Speed", 0f);
        playerAnimator.SetFloat("Direction", 0f);
    }

    //Sprint Functions
    public void OnSprintPreformed(InputAction.CallbackContext incomingValue)
    {
        // Debug.Log("sprint Hold");
        if (isGrounded || !isInAir || !Jetpack.instantOfJetpack.isHovering)
        {
            isSprinting = true;

            MovementSpeedChanges(sprintMovementSpeed);
            playerAnimator.SetFloat("Speed", 2f);
        }
    }
    public void OnSprintCanceled(InputAction.CallbackContext incomingValue)
    {
        //Debug.Log("sprint Canceled");
        isSprinting = false;
        playerAnimator.SetFloat("Speed", 1f);
        MovementSpeedChanges(defaultMovementSpeed);
    }

    void MoveRotarian()
    {
        transform.forward = Vector3.Lerp(transform.forward, new Vector3(moveDirection.x, 0, moveDirection.z), playerRotateSpeed * Time.fixedDeltaTime);
    }

    public void OnMove()
    {

        if (!WallClimingScript.instanceOfWallClimbingScript.isClimbing)
        {
            moveDirection = Utilities.CameraBasedInput(playerCamera, moveInput);
            if (OnSlope() && isGrounded)
            {
                Vector3 slopeDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
                playerRigidbody.velocity = slopeDirection * playerMovementSpeed * Time.fixedDeltaTime;
                MoveRotarian();
                //  transform.forward = Vector3.Lerp(transform.forward, new Vector3(moveDirection.x, 0, moveDirection.z), playerRotateSpeed * Time.fixedDeltaTime);
            }
            else if (!OnSlope())
            {
                // moveDirection = Utilities.CameraBasedInput(playerCamera, moveInput);
                Vector3 updatedMoveDirection = new Vector3(moveDirection.x * playerMovementSpeed * Time.fixedDeltaTime, playerRigidbody.velocity.y, moveDirection.z * playerMovementSpeed * Time.fixedDeltaTime);
                // playerRigidbody.velocity = new Vector3(moveDirection.x * playerMovementSpeed * Time.fixedDeltaTime, playerRigidbody.velocity.y, moveDirection.z * playerMovementSpeed * Time.fixedDeltaTime);
                MoveRotarian();

                playerRigidbody.velocity = updatedMoveDirection;
                //moveDirection = new Vector3(moveDirection.x * playerMovementSpeed * Time.fixedDeltaTime, playerRigidbody.velocity.y, moveDirection.z * playerMovementSpeed * Time.fixedDeltaTime);
                playerRigidbody.useGravity = true;
            }
        }
        else if (WallClimingScript.instanceOfWallClimbingScript.isClimbing)
        {
            moveDirection = (transform.up * moveInput.y + transform.right * moveInput.x) * WallClimingScript.instanceOfWallClimbingScript.ClimbingMultiplier * Time.fixedDeltaTime;
            playerRigidbody.velocity = moveDirection;
            playerRigidbody.useGravity = false;
        }

        // if (!WallClimingScript.instanceOfWallClimbingScript.isClimbing)
        // {

        //     moveDirection = Utilities.CameraBasedInput(playerCamera, moveInput);
        //     moveDirection = new Vector3(moveDirection.x * playerMovementSpeed * Time.fixedDeltaTime, playerRigidbody.velocity.y, moveDirection.z * playerMovementSpeed * Time.fixedDeltaTime);
        //     transform.forward = Vector3.Lerp(transform.forward, new Vector3(moveDirection.x, 0, moveDirection.z), playerRotateSpeed * Time.fixedDeltaTime);
        //     playerRigidbody.velocity = moveDirection;
        //     playerRigidbody.useGravity = true;
        // }
        // if (WallClimingScript.instanceOfWallClimbingScript.isClimbing) // if can climb and in range
        // {
        //     moveDirection = (transform.up * moveInput.y + transform.right * moveInput.x) * WallClimingScript.instanceOfWallClimbingScript.ClimbingMultiplier * Time.fixedDeltaTime;
        //     playerRigidbody.velocity = moveDirection;
        //     playerRigidbody.useGravity = false;
        // }

        // if (!isGrounded)
        // {
        //     isInAir = true;
        // }

    }


    private bool OnSlope()
    {

        if (Physics.Raycast(raycastGroundCheck.transform.position, Vector3.down, out slopeHit, raycastDistendsIsGrounded, groundLayer))
        {
            if (slopeHit.normal != Vector3.up)
            {
                // Debug.Log("OnSlope True");
                return true;

            }
            else
            {
                // Debug.Log("OnSlope false");
                return false;
            }
        }
        //        Debug.Log("OnSlope True");
        return false;
    }

    public void OnJumpPreformed(InputAction.CallbackContext context)
    {
        //moveInput = Vector2.zero;
        if (isGrounded)
            jump();
        else if (!isGrounded && currentJumpCount < maxjumpCount)
        {
            currentJumpCount++;
            flip();
        }



    }
    private void jump()
    {
        // Debug.Log("OnJumping isJumping = " + true);

        playerRigidbody.AddForce(Vector3.up * playerJumpMultiplier, ForceMode.Impulse);
        isGrounded = false;
        isInAir = true;
        playerAnimator.SetBool("Jump", true);
        playerAnimator.SetBool("InAir", isInAir);
        playerAnimator.SetBool("Grounded", isGrounded);
        // Debug.Log(" At end of OnJump 1 currentJumpCount= " + currentJumpCount);



    }

    IEnumerator DubbleJumpSet()
    {

        yield return new WaitForSeconds(0.5f);
        playerAnimator.SetBool("DoubleJump", false);

    }

    private void flip()
    {
        //  Debug.Log($"Flipping");

        playerAnimator.SetBool("Jump", true);
        playerAnimator.SetBool("DoubleJump", true);
        playerAnimator.SetBool("Grounded", false);
        playerRigidbody.AddForce(Vector3.up * playerFlipMultiplier, ForceMode.Impulse);


        StartCoroutine(DubbleJumpSet());
        // playerRigidbody.AddForce(Vector3.up * playerFlipMultiplier, ForceMode.Impulse);
    }






    //Jump Functions
    private void OnGroundCheck()
    {
        isGrounded = Physics.CheckSphere(overlapPosition.position, overlapRadius, groundLayer);

        RaycastHit groundHit;
        if (Physics.Raycast(raycastGroundCheck.transform.position, -transform.up, out groundHit, raycastDistendsIsGrounded))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && isInAir)    //ie the player was in the air and has now landed
        {
            isInAir = false;
            playerAnimator.SetBool("InAir", isInAir);
            playerAnimator.SetBool("Jump", false);
            playerAnimator.SetBool("DoubleJump", false);
            playerAnimator.SetBool("Jetpack", false);
            playerAnimator.SetBool("Jetpack", false);
            currentJumpCount = 0;
            Jetpack.instantOfJetpack.isHovering = false;
            Jetpack.instantOfJetpack.jetpackPressCount = 0;
        }
        else if (!isGrounded && !isInAir)
        {
            isInAir = true;
            playerAnimator.SetBool("InAir", isInAir);
        }
        playerAnimator.SetBool("Grounded", isGrounded);



    }

    [SerializeField] ParticleSystem leftDustParticleSystem;
    [SerializeField] ParticleSystem rightDustParticleSystem;
    public void Right_FootCollide()
    {
        //Debug.Log("Dust");
        AudioScript.InstanceOfAudioScript.rockFootSoundPlay();
        rightDustParticleSystem.Play();

    }

    public void Left_FootCollide()
    {
        //Debug.Log("Dust");
        AudioScript.InstanceOfAudioScript.rockFootSoundPlay();
        leftDustParticleSystem.Play();
    }
    public void ClimbStep()
    {
        AudioScript.InstanceOfAudioScript.grassFootSoundPlay();
    }





}
