using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpManager : MonoBehaviour
{
    public bool canDoubleJump;
    [SerializeField] GameObject jumpCollectParticles;
    [SerializeField] Transform player;
    
    // Start is called before the first frame update
    void Awake()
    {
        jumpCollectParticles.SetActive(false);
        canDoubleJump = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        canDoubleJump = true;
        jumpCollectParticles.SetActive(true);
        Destroy(gameObject);
    }
}
