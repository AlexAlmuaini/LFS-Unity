using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpManager : MonoBehaviour
{
    PlayerMovement playerMovement;
    DoorBehaviour doorBehaviour;
    public bool canDoubleJump, canSpawn;
    [SerializeField] GameObject particles;
    [SerializeField] GameObject DoubleJumpAcquiredText;
    
    // Start is called before the first frame update
    void Awake()
    {
        DoubleJumpAcquiredText.SetActive(false);
        this.gameObject.SetActive(false);
        playerMovement = FindObjectOfType<PlayerMovement>();
        canDoubleJump = false;
    }


    private void OnTriggerEnter(Collider col)
    {
        DoubleJumpAcquiredText.SetActive(true);
        canDoubleJump = true;
        globalStuff.double_jump = true;
        particles.transform.position = this.gameObject.transform.position;
        Instantiate(particles, transform.position, transform.rotation);
        this.gameObject.SetActive(false);
    }
}
