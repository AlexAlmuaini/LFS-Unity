using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpManager : MonoBehaviour
{
    public bool canDoubleJump;
    GameObject jumpCollectParticals;

    
    // Start is called before the first frame update
    void Awake()
    {
        canDoubleJump = false;
        //doublejump power up effect
        jumpCollectParticals = GameObject.Find("JumpPowerUpParticals");
        jumpCollectParticals.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("Bingus");
        canDoubleJump = true;
        this.gameObject.SetActive(false);
        jumpCollectParticals.SetActive(true);
        
    }
}
