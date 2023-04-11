using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectionCheck : MonoBehaviour
{
    public bool tnt_collected;
    public bool wire_collected;

    public GameObject tnt_pile;
    public GameObject layed_out_rope;
    private void Update()
    {
        globalStuff.tnt_collected = tnt_collected;
        globalStuff.wire_collected = wire_collected;
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
