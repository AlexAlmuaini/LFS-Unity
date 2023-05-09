using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DoorBehaviour : MonoBehaviour
{
    public float interpolateAmount;
    PlayerMovement playerMovement;
    [SerializeField] GameObject door, doubleJump;
    public bool doorOpening = false;
    public float kills_for_scene;
    void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if(playerMovement.kills >= kills_for_scene)
        {StartCoroutine(EOpen());}
    }
    public void DoorOpen()
    {
        interpolateAmount += Time.deltaTime;
        door.transform.position = Vector3.Lerp(door.transform.position, door.transform.position + Vector3.down * 5, interpolateAmount * 0.02f);
        if (interpolateAmount >=2f)
        {playerMovement.kills = 0;}
    }
    IEnumerator EOpen()
    {
        playerMovement.followCam = false;
        playerMovement.lockOnCamera = false;
        doorOpening = true;
        if(doubleJump != null)
        {
            doubleJump.SetActive(true);
        }
        yield return new WaitForSeconds(0.50f);
        DoorOpen();
    }
}
