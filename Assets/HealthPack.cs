using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    PlayerHealth playerHealth;
    [SerializeField] GameObject particles;
    void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }
     
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            Instantiate(particles, transform.position, transform.rotation);
            playerHealth.currentHealth = 100;
            Destroy(this.gameObject);
        }
    }
}
