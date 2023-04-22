using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpManager : MonoBehaviour
{
    public bool canDoubleJump;
    
    // Start is called before the first frame update
    void Awake()
    {
        canDoubleJump = false;
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
        
    }
}
