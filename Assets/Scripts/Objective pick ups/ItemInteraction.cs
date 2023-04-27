using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    public int itemNumber;

    private void OnTriggerEnter(Collider other)
    {
        if(other == GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>())
        {
            ItemCollected();
            Destroy(gameObject);
        }
    }
    void ItemCollected()
    {
        if(itemNumber == 1)
        {
            globalStuff.tnt_collected = true;
        }
        if (itemNumber == 2)
        {
            globalStuff.torch_collected = true;
        }
        if (itemNumber == 3)
        {
            globalStuff.wire_collected = true;
        }
    }
}
