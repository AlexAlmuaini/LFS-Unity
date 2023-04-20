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
    public Vector3 moveDirection, enemyPos;
    Transform cameraObject, enemy;
    Rigidbody playerRigidbody;
    public LayerMask groundLayer;

    public float movementSpeed, rayCastOffset = 0.5f, jumpHeight, gravityIntensity = 1, inAirTimer = 0;
    public float rotationSpeed = 15, attackDelay = 0f, fallingVelocity = 33, leapingVelocity = 3, jumps, speedBoostMultiplier = 1,
     gravityMultiplier = 5, detectionRad = 200, dist;
    public bool isJumping, isGrounded, canJump, followCam, lockOnCamera, attacking, enemyNear = false;

    private void Awake()
    {
        // THIS IS HOW TO FIX NULL OBJECT ERROR ~ ALEX PLEASE REMEMBER THIS
        inputManager = GetComponent<InputManager>();
        doubleJumpManager = FindObjectOfType<DoubleJumpManager>();
        speedBoostManager = FindObjectOfType<SpeedBoostManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraObject = Camera.main.transform;
        jumpParticles = GameObject.Find("CloudParticals");
        speedParticles = GameObject.Find("LighteningTrail");
        jumpParticles.SetActive(false);
        speedParticles.SetActive(false);
        followCam = true;
    }

   private void HandleMovement()
   {
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
                if(speedBoostManager.SpeedBoostActive)
                {
                    speedParticles.SetActive(true);
                } 
                movementSpeed = 10f * speedBoostMultiplier;
            }

        else
            {
                animator.SetBool("isSprinting",false);
                speedParticles.SetActive(false);
                movementSpeed = 5f;
            }

        moveDirection = cameraObject.forward * inputManager.verticalMovementInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalMovementInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;


        if(speedBoostManager.SpeedBoostActive)
        {
            speedBoostMultiplier = 1.25f;
            speedParticles.SetActive(true);
        }

        Vector3 movementVelocity = new Vector3 (moveDirection.x, playerRigidbody.velocity.y, moveDirection.z);
        playerRigidbody.velocity = movementVelocity;

        if(attacking)
        {
            attackDelay += Time.deltaTime;
            if(attackDelay >= 0.5f)
            {
                animator.SetBool("attack",false);
                attackDelay = 0f;
                attacking = false;
            }
        }
   }
   private void EnemyDectection()
   {    
    enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    enemyPos = enemy.position;
    dist = Vector3.Distance(enemyPos, transform.position);
    if(dist <= detectionRad && inputManager.lockOnCam != 0)
    {
        lockOnCamera = true;
        followCam = false;
    }
    else
    {
        lockOnCamera = false;
        followCam = true;
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
        EnemyDectection();
   }
public void HandleAttack()
{
    attacking = true;
    animator.SetBool("attack",true);
    
}
public void HandleJump()
    {
        if(canJump == true)
            {
                animator.SetBool("isJumping",true);
                playerRigidbody.AddForce(transform.up * jumpHeight * 1.5f, ForceMode.Impulse);
                isJumping = true;
                if(doubleJumpManager.canDoubleJump == true)
                {
                    if(jumps == 1)
                    {
                        playerRigidbody.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
                        animator.Play("Base Layer.Jump",0,0);
                        jumpParticles.SetActive(true);
                    }
                    jumps--;
                }
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
            if(doubleJumpManager.canDoubleJump == true)
            {
                jumps = 2;
                jumpParticles.SetActive(false);
            }
            else
            {
                jumps = 1;
            }
        }
   }

   private void OnCollisionStay(Collision col)
   {
        if(col.gameObject.tag == "Floor")
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

   private void OnCollisionEnter(Collision col)
   {
    if(col.gameObject.tag == "Enemy")
    {
        if(attacking)
        {Debug.Log("POW!!");}
        else
        {
            Debug.Log("OWW!!");
            playerRigidbody.AddForce(transform.up * 300, ForceMode.Impulse);
            playerRigidbody.AddForce(transform.forward * -100000, ForceMode.Force);
        }
    }
   }
}
