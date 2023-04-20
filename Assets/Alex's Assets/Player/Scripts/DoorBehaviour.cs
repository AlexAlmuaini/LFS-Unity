using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DoorBehaviour : MonoBehaviour
{
    private PlayableDirector director; 
    public GameObject controlPanel;
    public GameObject doorObject;
    public float moveDistance = 1;
    GameObject doorLeft, doorRight;
    public bool doorOpening = false;
    //Rigidbody leftDoorRigidBody, rightDoorRigidBody;
    void Awake()
    {
        doorObject = GameObject.Find("Door");
        doorLeft = GameObject.Find("DoorLeft");
        doorRight = GameObject.Find("DoorRight");
        director = doorObject.GetComponent<PlayableDirector>();
        director.Stop();
        //director.played += Director_Played;
        //director.stopped += Director_Stopped;
        //leftDoorRigidBody = doorLeft.GetComponent<Rigidbody>();
        //rightDoorRigidBody = doorRight.GetComponent<Rigidbody>();
    }

    public void DoorOpen()
    {
        //leftDoorRigidBody.AddForce(-transform.right * moveDistance, ForceMode.Impulse);
        //rightDoorRigidBody.AddForce(transform.right * moveDistance, ForceMode.Impulse);
        //doorLeft.transform.position = doorLeft.transform.position + new Vector3(0, 0, - moveDistance);
        //doorRight.transform.position = doorRight.transform.position + new Vector3(0, 0, moveDistance);
        director.Play();
    }
}
