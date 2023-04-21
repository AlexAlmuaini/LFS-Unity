using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    GameObject jumpParticles;
    GameObject speedParticles;
    DoubleJumpManager doubleJumpManager;
    SpeedBoostManager speedBoostManager;
    public Vector3 moveDirection;
    Transform cameraObject, enemy;
    Rigidbody playerRigidbody;

    public float movementSpeed, jumpHeight, inAirTimer = 0;
    public float rotationSpeed = 15, attackDelay = 0.0f, jumps, speedBoostMultiplier = 1;
    public bool isJumping, isGrounded, canJump, followCam, attacking;
    private float speed;

    private void Awake()
    {
        // THIS IS HOW TO FIX NULL OBJECT ERROR ~ ALEX PLEASE REMEMBER THIS
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraObject = Camera.main.transform;
        followCam = true;
        speed = movementSpeed;
    }

   private void HandleMovement()
   {
    if(!attacking){
        movementSpeed = speed;
        if(inputManager.verticalMovementInput != 0 || inputManager.horizontalMovementInput != 0)
            {
                animator.SetBool("isWalking",true);
            }

        else
            {
                animator.SetBool("isWalking",false);
            }

        if(inputManager.sprintFloat != 0)
            {
                animator.SetBool("isSprinting",true);
                movementSpeed = 10f * speedBoostMultiplier;
            }

        else
            {
                animator.SetBool("isSprinting",false);
                movementSpeed = 5f;
            }

        moveDirection = cameraObject.forward * inputManager.verticalMovementInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalMovementInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = new Vector3 (moveDirection.x, playerRigidbody.velocity.y, moveDirection.z);
        playerRigidbody.velocity = movementVelocity;
    }
    if(attacking)
        {
            movementSpeed = 0;
            attackDelay += Time.deltaTime;
            if(attackDelay >= .33f)
            {
                attackDelay = 0f;
                attacking = false;
            }
        }
   }
   private void HandleRoation()
   {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalMovementInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalMovementInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRoation =  Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRoation;
   }
public void HandleAllMovement()
   {
        HandleMovement();
        HandleRoation();
        HandleFalling();
   }
public void HandleAttack()
{
    attacking = true;
    animator.Play("Punching", 0, 0.25f);
}
public void HandleJump()
    {
        if(canJump == true)
            {
                animator.SetBool("isJumping",true);
                playerRigidbody.AddForce(transform.up * jumpHeight * 1.5f, ForceMode.Impulse);
                isJumping = true;
            }

        else
            {
                animator.SetBool("isJumping",false);
                animator.SetBool("isDoubleJumping",false);
            }
    }
   private void HandleFalling()
   {    
    if(jumps <= 0)
            {
                canJump = false;
                animator.SetBool("isJumping",false);
            }
        else
            {
                canJump = true;
            }

        if(isGrounded)
        {
            jumps = 1;
        }
   }

   private void OnCollisionStay(Collision col)
   {
        if(col.gameObject.tag == "Floor" || col.gameObject.tag == "Arena")
            {
                isGrounded = true;
                animator.SetBool("isJumping",false);
                inAirTimer = 0;
            }   
   }
   private void OnCollisionExit(Collision col)
   {
    isGrounded = false;
    jumps--;
   }
}
