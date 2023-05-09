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


     private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            SpeedBoostActive = true;
            Destroy(this.gameObject);
        }
    }

}
