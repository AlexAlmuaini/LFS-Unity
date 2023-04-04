using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public PlayerController controller;


    void Awake()
    {
        controller = GameObject.Find("RPG-Character").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            controller.speedPickedUp = true;
            controller.max_speed = controller.boosted_speed;
            Destroy(this.gameObject);
        }
    }
}
