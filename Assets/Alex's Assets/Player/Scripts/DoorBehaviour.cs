using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DoorBehaviour : MonoBehaviour
{
    public float interpolateAmount;
    PlayerMovement playerMovement;
    [SerializeField] GameObject door;
    public bool doorOpening = false;
    void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if(playerMovement.kills >= 4)
        {StartCoroutine(EOpen());}
    }
    public void DoorOpen()
    {
        doorOpening = true;
        playerMovement.followCam = false;
        interpolateAmount += Time.deltaTime;
        door.transform.position = Vector3.Lerp(door.transform.position, door.transform.position + Vector3.down * 5, interpolateAmount * 0.02f);
        if (interpolateAmount >=3.5f)
        {playerMovement.kills = 0;}
    }
    IEnumerator EOpen()
    {
        yield return new WaitForSeconds(0.50f);
        DoorOpen();
    }
}
