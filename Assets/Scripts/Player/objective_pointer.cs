using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objective_pointer : MonoBehaviour
{
    public Transform Arrow;
    public Transform objective;
    void Update()
    {
        if (globalStuff.tnt_collected || globalStuff.torch_collected || globalStuff.wire_collected)
        {
            Arrow.gameObject.SetActive(true);
            Arrow.transform.LookAt(objective);
        }
        else
        {
            Arrow.gameObject.SetActive(false);
        }
    }
}
