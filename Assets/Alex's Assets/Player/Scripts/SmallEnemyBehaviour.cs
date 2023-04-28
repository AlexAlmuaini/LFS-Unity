using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SmallEnemyBehaviour : MonoBehaviour
{
    public Transform patrolPost;
    PlayerMovement playerMovement;
    MeshRenderer meshRenderer;
    [SerializeField] GameObject deathParticles;
    GameObject slimeParticles;
    Color originalColour, damageColor = Color.red;
    private Transform player;
    private float dist,deathTime = 1f, flashTime = 0.15f;
    public float moveSpeed, health = 5;
    public float detectionRad;
    public float damping;
    public bool isMoving;
    private bool onGround;
    private float timer;
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

    void Update()
    {
        dist = Vector3.Distance(player.position, transform.position);
        
        if(dist <= detectionRad)
        {
            var lookPos = player.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            GetComponent<Rigidbody>().AddForce(transform.forward * moveSpeed);
            isMoving = true;
        }
        else
        {
            var lookPos = patrolPost.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            GetComponent<Rigidbody>().AddForce(transform.forward * 750);
            isMoving = false;
            if (onGround)
            {
                timer += Time.deltaTime;
            }
        }
        HealthStates();
    }

    private void OnCollisionStay(Collision col)
   {
    if(col.gameObject.tag == "Floor")
        {
            if(isMoving == true){GetComponent<Rigidbody>().AddForce(transform.up * 25500);}

            if (isMoving == false)
            {
                onGround = true;
                if (timer >= 2)
                {
                    onGround = false;
                    timer = 0;
                    GetComponent<Rigidbody>().AddForce(transform.up * 25500 * 5);
                }
            }
        } 
        
   }
   private void OnTriggerStay(Collider col)
   {
        if(col.gameObject.tag == "Player")
        {
            if(playerMovement.attacking)
            {
                StartCoroutine(EFlash());
                GetComponent<Rigidbody>().AddForce(transform.up * 25500);
                GetComponent<Rigidbody>().AddForce(transform.forward * -50000);
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
    IEnumerator EDeath()
   {
    playerMovement.kills++;
    slimeParticles = Instantiate(deathParticles, transform.position, transform.rotation);
    Destroy(gameObject);
    yield return new WaitForSeconds(deathTime);
    Destroy(slimeParticles);
   }
   IEnumerator EFlash()
   {
    meshRenderer.material.color = damageColor;
    yield return new WaitForSeconds(flashTime);
    meshRenderer.material.color = originalColour;
   }
}
