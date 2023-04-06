using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Spawn : MonoBehaviour
{
    public Transform[] door_spawn;
    private void Awake()
    {
        if (globalStuff.door_passed)
        {
            gameObject.transform.position = door_spawn[globalStuff.door_number].position;
            gameObject.transform.rotation = door_spawn[globalStuff.door_number].rotation;
        }
    }
}
