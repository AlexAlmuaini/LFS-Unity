using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    public int itemNumber;
    private bool itemCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other == GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>())
        {
            itemCollected = true;
            ItemCollection();
            Destroy(gameObject);
        }
    }
    void ItemCollection()
    {
        if (itemNumber == 0)
        {
            globalStuff.tnt_collected = true;
        }
        if (itemNumber == 1)
        {
            globalStuff.torch_collected = true;
        }
        if (itemNumber == 2)
        {
            globalStuff.wire_collected = true;
        }
    }
}
