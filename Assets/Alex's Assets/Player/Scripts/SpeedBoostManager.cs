using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostManager : MonoBehaviour
{
    public bool SpeedBoostActive;
    void Awake()
    {
        SpeedBoostActive = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnTriggerEnter(Collider col)
    {
        Debug.Log("Bongus");
        SpeedBoostActive = true;
        this.gameObject.SetActive(false);
        //jumpCollectParticals.SetActive(true);
        
    }
}
