using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimingScript : MonoBehaviour
{
    public static WallClimingScript instanceOfWallClimbingScript;
    [Header("Player References")]
    Rigidbody playerRigidbody;

    [Header("Climbing")]
    [SerializeField] LayerMask climbLayer; // in scene layer number check for build
    [SerializeField] public bool isClimbing;
    [SerializeField] private bool sideWall;
    [SerializeField] public float ClimbingMultiplier;


    [Header("Player Raycast")]
    private GameObject playerRay;
    private RaycastHit climbHit;
    private RaycastHit topHit;
    private RaycastHit sidesHit;


    [SerializeField] Transform rayStartPos;
    [SerializeField] Transform ClimbRayStartPos;

    [SerializeField] Transform topRayStart;
    [SerializeField] Transform leftRay;
    [SerializeField] Transform rightRay;
    [SerializeField] float rayDistance;
    [SerializeField] float sideWallDistance;
    [SerializeField] float sideRayPlayer;
    [SerializeField] LayerMask hitLayers;


    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        instanceOfWallClimbingScript = this;

    }

    private void FixedUpdate()
    {
        OnClimbDetect();
        OnCornerSwitch();
    }

    private void OnClimbDetect()
    {
        if (!isClimbing)
        {
            if (Physics.Raycast(rayStartPos.position, transform.forward, out climbHit, rayDistance, climbLayer)) // Forward
            {
                isClimbing = true;
                PlayerMovementController.instantOfPlayerMovementControllerScript.playerAnimator.SetBool("DoubleJump", false);
                OnClimbing();
            }
            Debug.DrawRay(rayStartPos.position, transform.forward * rayDistance, Color.blue);

        }
        else
        {
            ClimbRayStartPos.localPosition = Vector3.right * PlayerMovementController.instantOfPlayerMovementControllerScript.moveInput.x * sideRayPlayer;

            if (Physics.Raycast(rayStartPos.position, transform.forward, out climbHit, climbLayer))  // Side to Side values
            {
                OnClimbing();
                Debug.DrawRay(rightRay.position, rightRay.TransformDirection(Vector3.forward) * rayDistance, Color.blue);
            }
            else
            {
                // forward
                Physics.Raycast(ClimbRayStartPos.position, transform.forward, out climbHit, rayDistance, climbLayer);
                OnClimbing();
            }


            if (Physics.Raycast(topRayStart.position, transform.forward, out topHit, rayDistance, climbLayer))
            {
            }

            if (sideWall)
            {
                // transform.forward = -sidesHit.normal;
                transform.forward = -sidesHit.normal;
            }
            else
            {
                isClimbing = false;
                PlayerMovementController.instantOfPlayerMovementControllerScript.playerAnimator.SetBool("Climbing", false);
            }
        }
    }
    private void OnClimbing()
    {
        if (isClimbing)
        {
            isClimbing = true;
            PlayerMovementController.instantOfPlayerMovementControllerScript.playerAnimator.SetBool("DoubleJump", false);
            playerRigidbody.useGravity = false;
            PlayerMovementController.instantOfPlayerMovementControllerScript.playerAnimator.SetBool("Climbing", true);
            PlayerMovementController.instantOfPlayerMovementControllerScript.playerAnimator.SetBool("Grounded", false);
            // Debug.Log($"isClimbing" + isClimbing);
            // Debug.Log($"isGrounded" + isGrounded);

            // transform.forward = -sidesHit.normal; // if changes walls it will transform else stay same value 
            transform.forward = -climbHit.normal;
        }

        if (PlayerMovementController.instantOfPlayerMovementControllerScript.currentJumpCount > 0) // if player jumps while on wall he disconnects
        {
            isClimbing = false;
            PlayerMovementController.instantOfPlayerMovementControllerScript.playerAnimator.SetBool("Climbing", false);
            // canClimb = false;
            playerRigidbody.useGravity = true;
            transform.forward = Vector3.forward;
        }

    }
    private void OnCornerSwitch()
    {
        if (Physics.Raycast(rightRay.position, rightRay.TransformDirection(Vector3.forward), out sidesHit, sideWallDistance, climbLayer) || Physics.Raycast(leftRay.position, leftRay.TransformDirection(Vector3.forward), out sidesHit, sideWallDistance, climbLayer)) //right Corner Walls Ray
        {
            sideWall = true;
        }
        else
        {
            sideWall = false;
            // Debug.Log("sideWall= false");
        }
        Debug.DrawRay(rightRay.position, rightRay.TransformDirection(Vector3.forward), Color.yellow);
        Debug.DrawRay(leftRay.position, leftRay.TransformDirection(Vector3.forward), Color.yellow);
    }
}
