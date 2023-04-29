using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class KingSlimeBehaviour : MonoBehaviour
{
    PlayerMovement playerMovement;
    MeshRenderer meshRenderer;
    Color originalColour, damageColor = Color.red;
    [SerializeField] GameObject nextSlime, currentSlime;
    private GameObject babyslime, deadSlime;
    private Transform player;
    [SerializeField] GameObject deathParticles;
    private float dist, flashTime = 0.15f, deathTime = 5.0f;
    public float moveSpeed, health = 25;
    public float detectionRad;
    public float damping;
    public bool isMoving;

    public Transform ReturnPost;
    public Transform patrolPost;
    private bool onGround;
    private float timer;
    private bool returnState;
    
   public float cooldown = 5.0f;

    private bool canAttack = true;
    private float attackTimer = 0;

    public float groundPoundRadius = 5.0f;
    public float groundPoundDamage = 10.0f;
    public float coneAngle = 90.0f;
    public float coneDistance = 20.0f;

    public float chargeSpeed = 10f;
    public float chargeDuration = 2f;

    private bool isCharging = false;
    private bool isRotating = false;
    private float rotationTimer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        isMoving = false;
        originalColour = meshRenderer.material.color;
        damageColor.a = 0.41f;
    }

    // Update is called once per frame
    void Update()
    {
        if(returnState)
        {
            ReturningToPost();
        }
        else
        {  
            HandleMovement();
        }
        HealthStates();
        
        //Attack cooldown
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= cooldown)
            {
                canAttack = true;
                attackTimer = 0.0f;
            }
        }
        if (isRotating)
        {
            rotationTimer += Time.deltaTime;
            RotateTowardsTarget(rotationTimer);
            
        }
        if (isCharging)
        {
            Charge();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Arena")
        {
            returnState = true;
        }
    }
    private void ReturningToPost()
    {
        var lookPos = ReturnPost.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        GetComponent<Rigidbody>().AddForce(transform.forward * moveSpeed * 3);
        if(Vector3.Distance(ReturnPost.position, transform.position) <= 1)
        {
            returnState = false;
        }
    }
    private void HandleMovement()
    {
        dist = Vector3.Distance(player.position, transform.position);
        if(!isCharging)
        {
            if(dist <= detectionRad)
            {
                var lookPos = player.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
                GetComponent<Rigidbody>().AddForce(transform.forward * 1200);
                isMoving = true;
            }
            else
            {
                var lookPos = patrolPost.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
                GetComponent<Rigidbody>().AddForce(transform.forward * 1200);
                isMoving = false;
                if (onGround)
                {
                    timer += Time.deltaTime;
                }
            }
        }   
    }

    private void OnCollisionStay(Collision col)
   {
        if(col.gameObject.tag == "Floor")
        {
            if(isMoving == true)
            {
                GetComponent<Rigidbody>().AddForce(transform.up * 75000);
            }

            if (isMoving == false)
            {
                onGround = true;
                if (timer >= 2)
                {
                    onGround = false;
                    timer = 0;
                    GetComponent<Rigidbody>().AddForce(transform.up * 75000 * 5);
                }
            }
        }

        if(col.gameObject.tag == "Player")
        {
            if(playerMovement.attacking)
            {
                StartCoroutine(EFlash());
                GetComponent<Rigidbody>().AddForce(transform.up * 40000);
                GetComponent<Rigidbody>().AddForce(transform.forward * -60000);
                health--;
            }
        }     
   }

   private void HealthStates()
   {
        if(health <= 0)
        {
            StartCoroutine(EDeath());
        }
   }

   IEnumerator EFlash()
   {
    meshRenderer.material.color = damageColor;
    yield return new WaitForSeconds(flashTime);
    meshRenderer.material.color = originalColour;
   }

   IEnumerator EDeath()
   {
    babyslime = Instantiate(nextSlime, transform.position + Vector3.left * - 2, transform.rotation);
    babyslime = Instantiate(nextSlime, transform.position, transform.rotation);
    babyslime = Instantiate(nextSlime, transform.position + Vector3.left * 2, transform.rotation);
    babyslime = Instantiate(nextSlime, transform.position + Vector3.back * - 2 , transform.rotation);
    babyslime = Instantiate(nextSlime, transform.position + Vector3.back * 2 , transform.rotation);
    deadSlime = Instantiate(deathParticles, transform.position, transform.rotation);
    Destroy(gameObject);
    yield return new WaitForSeconds(deathTime);
    Destroy(deadSlime);
   }


    void FixedUpdate()
    {
        if (canAttack)
        {
            canAttack = false;
            //Temporary random attack choice for testing
            // will be based on player position in relation to boss
            int num = Random.Range(0, 2);
            //print("Cooldown over, attack selected:" + num);
            num = 1;
            if (num == 0)
            {
                groundPound();
            }
            else if (num == 1 && dist >= detectionRad/3)
            {
                isRotating = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isCharging && collision.gameObject.CompareTag("Player"))
        {
            // deal damage to player and stop charging sequence
            //collision.GetComponent<PlayerController>().TakeDamage(attackDamage);
            isCharging = false;
            playerMovement.GetComponent<Rigidbody>().AddForce(transform.up * 3000);
            playerMovement.GetComponent<Rigidbody>().AddForce(transform.forward * 12000);
        }
    }

    void groundPound()
    {
        // Get boss's forward vector and calculate cone bounds
        Vector3 bossForward = transform.forward;
        Vector3 coneOrigin = transform.position + Vector3.up; // Add offset to avoid detecting boss's own collider
        float coneHalfAngle = coneAngle / 2.0f;
        float coneHalfRadians = coneHalfAngle * Mathf.Deg2Rad;
        float coneDistanceSquared = coneDistance * coneDistance;
        Vector3 coneRight = Quaternion.AngleAxis(coneHalfAngle, Vector3.up) * bossForward;
        Vector3 coneLeft = Quaternion.AngleAxis(-coneHalfAngle, Vector3.up) * bossForward;

        // Detect all colliders within cone shape in front of boss
        Collider[] hitColliders = Physics.OverlapSphere(coneOrigin, coneDistance);
        foreach (Collider hitCollider in hitColliders)
        {
            //print("Collision");
            if (hitCollider.tag == "Player")
            {
                print("Player Collision");
                // Check if player character is within cone angle and distance
                Vector3 toPlayer = hitCollider.transform.position - coneOrigin;
                print("to player vector" + toPlayer);
                float dotRight = Vector3.Dot(toPlayer.normalized, coneRight);
                float dotLeft = Vector3.Dot(toPlayer.normalized, coneLeft);

                if (toPlayer.magnitude <= coneDistance)
                {
                    // detected in circle
                    //hitCollider.GetComponent<PlayerController>().TakeDamage(attackDamage);
    
                    if (dotRight > Mathf.Cos(coneHalfRadians) && dotLeft < Mathf.Cos(coneHalfRadians))
                    {
                        // buggy but detects in front
                        print("in Front");
                    }
                }
            }
        }
    }

    void Charge()
    {
        Vector3 direction = Vector3.forward;
        direction.y = 0;
        GetComponent<Rigidbody>().AddForce(transform.forward * 7500);
    }

    void RotateTowardsTarget(float timer)
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
        if (Quaternion.Angle(transform.rotation, rotation) < 2f || timer > 5 )
        {
            isRotating = false;
            isCharging = true;
            transform.rotation = rotation;
            StartCoroutine(StopChargingAfterDelay());
        }


    }

    IEnumerator StopChargingAfterDelay()
    {
        yield return new WaitForSeconds(chargeDuration);
        isCharging = false;
    }
}
