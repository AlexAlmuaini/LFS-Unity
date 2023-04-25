using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionCutscene : MonoBehaviour
{
    InputManager inputManager;
    collectionCheck collectionCheck;
    GameObject player;
    public GameObject explosion;
    public GameObject levelManager;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inputManager = player.GetComponent<InputManager>();
        collectionCheck = levelManager.GetComponent<collectionCheck>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other == player.GetComponent<Collider>())
        {
            if (inputManager.swapInput == 1
                && collectionCheck.tnt_pile
                && collectionCheck.layed_out_rope)
            {
                explosion.GetComponent<explosion>().enabled = true;
            }
        }
    }
}
