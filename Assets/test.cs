using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float t;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(globalStuff.double_jump)
        {
            t = 1;
        }
        if(t ==1)
        {
            globalStuff.double_jump = true;
        }
    }
}
