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
    public float moveSpeed, health = 8;
    public float detectionRad;
    public float damping;
    public bool isMoving;

    public Transform ReturnPost;
    public Transform patrolPost;
    private bool onGround;
    private float timer;
    private bool returnState;
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
    }

    private void OnCollisionStay(Collision col)
   {
    if(col.gameObject.tag == "Floor")
        {
            if(isMoving == true)
            {
                GetComponent<Rigidbody>().AddForce(transform.up * 30000);
            }

            if (isMoving == false)
            {
                onGround = true;
                if (timer >= 2)
                {
                    onGround = false;
                    timer = 0;
                    GetComponent<Rigidbody>().AddForce(transform.up * 30000 * 5);
                }
            }
        }

        if(col.gameObject.tag == "Player")
        {
            if(playerMovement.attacking)
            {
                StartCoroutine(EFlash());
                GetComponent<Rigidbody>().AddForce(transform.up * 20000);
                GetComponent<Rigidbody>().AddForce(transform.forward * -30000);
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
}
