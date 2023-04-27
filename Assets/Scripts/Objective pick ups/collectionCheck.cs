using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectionCheck : MonoBehaviour
{ 
    public GameObject tnt_pile;
    public GameObject layed_out_rope;
    item_placement itemPlacement;

    public GameObject itemPlacementChecker;

    private void Awake()
    {
        itemPlacement = itemPlacementChecker.GetComponent<item_placement>();
    }
    private void Update()
    {
        if(itemPlacement.tnt_placed)
        {
            tnt_pile.SetActive(true);
        }
        if(itemPlacement.rope_placed)
        {
            layed_out_rope.SetActive(true);
        }
    }
}
