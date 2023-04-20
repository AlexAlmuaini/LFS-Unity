using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSplinesScript : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;
    CameraManager cameraManager;
    public float interpolateAmount, transInterpolateAmount;
    public float interpolateAmountX;
    public float interpolateAmountZ;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    [SerializeField] public Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform pointC;
    [SerializeField] private Transform pointD;
    
    [SerializeField] private Transform pointAB_BC;
    [SerializeField] private Transform pointBC_CD;

    [SerializeField] private Transform pointABCD;
    private Vector3 positionInSpline;
    public bool splineCam, transition;
    
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
        interpolateAmount = InverseLerp(pointA.position, pointD.position, playerMovement.transform.position);
        //Interpolates between 4 points using 2 functions, the first interpolates a-b & b-c and then interpolates between them
        // to get a-b-c then interpolates between a-b-c & b-c-d
        positionInSpline = CubicLerp(pointA.position, pointB.position, pointC.position, pointD.position, interpolateAmount);
        if(transition)
        {
            CameraTransitionSmoothing();
        }
        if(splineCam && !transition)
        {
        pointABCD.position = positionInSpline;
        }
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

    private void CameraTransitionSmoothing()
    {
        if (splineCam)
        {
            transInterpolateAmount = (transInterpolateAmount + Time.deltaTime);
            pointABCD.position = Vector3.Lerp(playerMovement.transform.position + Vector3.up * 1.6f, positionInSpline, transInterpolateAmount);
            if(transInterpolateAmount >= 1f)
            {
                transInterpolateAmount = 0;
                transition = false;
            }
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
