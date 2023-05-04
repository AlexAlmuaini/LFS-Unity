using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    GameObject jumpParticles;
    GameObject speedParticles;
    GameObject slashParticles;
    GameObject slashParticles2, pointer1;
    DoubleJumpManager doubleJumpManager;
    SpeedBoostManager speedBoostManager;
    public Vector3 moveDirection, enemyPos;
    [SerializeField] GameObject pointer;
    Transform cameraObject;
    Rigidbody playerRigidbody;
    public GameObject[] enemies;
    GameObject enemy;

    public float movementSpeed, jumpHeight, inAirTimer = 0;
    public float rotationSpeed = 15, attackDelay = 0.0f, jumps, speedBoostMultiplier = 1, kills = 0, punchInt, dist,detectionRad = 200;
    public bool isJumping, isGrounded, canJump, followCam, attacking,lockOnCamera, doubleJump;
    private float speed;

    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] GameObject hurtParticles;
    private GameObject hurtInstance;


    private void Awake()
    {
        // THIS IS HOW TO FIX NULL OBJECT ERROR ~ ALEX PLEASE REMEMBER THIS
        inputManager = GetComponent<InputManager>();
        doubleJumpManager = FindObjectOfType<DoubleJumpManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        jumpParticles = GameObject.Find("CloudParticals");
        slashParticles = GameObject.Find("SlashParticles");
        slashParticles2 = GameObject.Find("SlashParticles2");
        pointer = GameObject.Find("Pointer");
        cameraObject = Camera.main.transform;
        followCam = true;
        jumpParticles.SetActive(false);
        speed = movementSpeed;
    }
    private void EnemyDectection()
   {    
    enemies = GameObject.FindGameObjectsWithTag("Enemy");
    
    if(enemies.Length > 0)
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyPos = enemy.transform.position;
        dist = Vector3.Distance(enemyPos, transform.position);

        if(dist <= detectionRad && inputManager.lockOnCam != 0)
            {
                pointer.transform.position = enemyPos - Vector3.down * 2;
                pointer.SetActive(true);
                lockOnCamera = true;
                followCam = false;
            }
        else
            {
                pointer.SetActive(false);
                lockOnCamera = false;
                followCam = true;
            }
    }
    else{pointer.SetActive(false);lockOnCamera = false;followCam = true;}
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
        EnemyDectection();
   }
public void HandleAttack()
{
    attacking = true;
        if (punchInt == 0)
        {
            animator.Play("Base Layer.Punching", 0, .45f);
            punchInt++;
            slashParticles2.GetComponent<ParticleSystem>().Play();
        }
        else if (punchInt == 1)
        {
            animator.Play("Base Layer.Cross Punch", 0, .425f);
            punchInt--;
            slashParticles.GetComponent<ParticleSystem>().Play();
        }
        
}
public void HandleJump()
    {
        if(canJump == true)
            {
                animator.SetBool("isJumping",true);
                playerRigidbody.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
                isJumping = true;
                if(doubleJumpManager == null)
                {
                    doubleJump = true;
                }
                else
                {
                    doubleJump = doubleJumpManager.canDoubleJump;
                }
                
                if(doubleJump == true)
                {
                    if(jumps == 1)
                    {
                        playerRigidbody.AddForce(transform.up * jumpHeight * .75f, ForceMode.Impulse);
                        animator.Play("Base Layer.Jump",0,0);
                        jumpParticles.SetActive(true);
                    }
                    jumps--;
                }
            }

        else
            {
                animator.SetBool("isJumping",false);
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
    }

   private void OnCollisionStay(Collision col)
   {
        if(col.gameObject.tag == "Floor")
            {            
                jumps = 1;
                isGrounded = true;
                animator.SetBool("isJumping",false);
                inAirTimer = 0;
            if (globalStuff.double_jump == true)
                {
                    jumps = 2;
                    jumpParticles.SetActive(false);
                }

            }   
   }
   private void OnCollisionExit(Collision col)
   {
    isGrounded = false;
    jumps--;
   }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("collision");
            playerHealth.TakeDamage(5);
            StartCoroutine(BleedParticles());
            this.GetComponent<Rigidbody>().AddForce(transform.up * 10000);
            this.GetComponent<Rigidbody>().AddForce(transform.forward * -15000);
            

        }
    }

    IEnumerator BleedParticles()
    {
        hurtInstance = Instantiate(hurtParticles, transform.position, transform.rotation);
        yield return new WaitForSeconds(1.0F);
        Destroy(hurtInstance);
    }
}
