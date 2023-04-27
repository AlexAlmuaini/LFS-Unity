using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSplinesScript : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;
    CameraManager cameraManager;
    public float interpolateAmount, transInterpolateAmount;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    [SerializeField] public Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform pointC;
    [SerializeField] private Transform pointD;
    [SerializeField] private Transform pointABCD;
    private Vector3 positionInSpline;
    public bool splineCam, transition;
    public float vertical1 = 0, vertical;
    public float vertical2 = 0, horizontal = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        cameraManager = FindObjectOfType<CameraManager>();
    }   

    // Update is called once per frame
    void Update()
    {   
        //inverse lerp to get the players x position in relation to the start and finish then devide it over the finish
        //to get the percentage for the interpolateAmount.
        
        if(vertical1 != 1)
        {
            vertical1 = InverseLerp(pointA.position, pointB.position, playerMovement.transform.position);
        }
        else{vertical1 = 1;}
        
        horizontal = InverseLerp(pointB.position, pointC.position, playerMovement.transform.position);

        if (horizontal < 1)
        {
            vertical2 = 0;
        } 
        else
        {
            if(vertical2 != 1)
        {
            vertical2 = InverseLerp(pointC.position, pointD.position, playerMovement.transform.position);
        }
        else{vertical2 = 1;}
        }
        
        interpolateAmount = horizontal/2 + vertical1/4 + vertical2/4;
        //Interpolates between 4 points using 2 functions, the first interpolates a-b & b-c and then interpolates between them
        // to get a-b-c then interpolates between a-b-c & b-c-d
        positionInSpline = CubicLerp(pointA.position, pointB.position, pointC.position, pointD.position, interpolateAmount);
        pointABCD.position = positionInSpline;
    }

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
     {
         Vector3 AB = b - a;
         Vector3 AV = value - a;
         return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
     }


    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a,b,t);
        Vector3 bc = Vector3.Lerp(b,c,t);

        return Vector3.Lerp(ab, bc, interpolateAmount);
    }


    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticLerp(a,b,c,t);
        Vector3 bc_cd = QuadraticLerp(b,c,d,t);

        return Vector3.Lerp(ab_bc, bc_cd, interpolateAmount);
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            transition = true;
        }
    }

    //Player detection to switch cameras to allow player to enter and exit spline view
    private void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            splineCam = true;
            playerMovement.followCam = false;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            splineCam = false;
            transition = true;
            playerMovement.followCam = true;
        }
    }
}
