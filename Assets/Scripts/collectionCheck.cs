using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectionCheck : MonoBehaviour
{ 
    public GameObject tnt_pile;
    public GameObject layed_out_rope;
    private void Update()
    {
        if(globalStuff.tnt_collected)
        {
            tnt_pile.SetActive(true);
        }
        if(globalStuff.wire_collected)
        {
            layed_out_rope.SetActive(true);
        }
    }
}
