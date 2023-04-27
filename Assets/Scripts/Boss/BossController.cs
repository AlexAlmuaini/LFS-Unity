using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

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
    public GameObject player;

    void Update()
    {
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
            else if (num == 1)
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
        transform.Translate(direction * chargeSpeed * Time.deltaTime * 2);
    }

    void RotateTowardsTarget(float timer)
    {
        print("rotatiing");
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;
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