using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    public float cooldown = 10.0f;

    private bool canAttack = true;
    private float attackTimer = 0;

    public float groundPoundRadius = 5.0f;
    public float groundPoundDamage = 10.0f;
    public float coneAngle = 60.0f;
    public float coneDistance = 10.0f;

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
    }

    void FixedUpdate()
    {
        if (canAttack)
        {
            canAttack = false;
            //Temporary random attack choice for testing
            int num = Random.Range(0, 1);
            print("Cooldown over, attack selected:" + num);
            if (num == 0)
            {
                groundPound();
            }
            else
            {

            }
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
            if (hitCollider.tag == "Player")
            {
                // Check if player character is within cone angle and distance
                Vector3 toPlayer = hitCollider.transform.position - coneOrigin;
                float dotRight = Vector3.Dot(toPlayer.normalized, coneRight);
                float dotLeft = Vector3.Dot(toPlayer.normalized, coneLeft);
                if (dotRight > Mathf.Cos(coneHalfRadians) && dotLeft > Mathf.Cos(coneHalfRadians) && toPlayer.sqrMagnitude <= coneDistanceSquared)
                {
                    // Apply damage to player character
                    //hitCollider.GetComponent<PlayerController>().TakeDamage(attackDamage);
                }
            }
        }
    }
}