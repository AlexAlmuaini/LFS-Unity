using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpManager : MonoBehaviour
{
    PlayerMovement playerMovement;
    public bool canDoubleJump;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        canDoubleJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }

    private void OnTriggerEnter(Collider col)
    {
        canDoubleJump = true;
        this.gameObject.SetActive(false);
    }
    
    private void Spawn()
    {
        if(playerMovement.kills >= 4)
        {gameObject.transform.position = playerMovement.transform.position + Vector3.forward * 3;
        }
    }
}
