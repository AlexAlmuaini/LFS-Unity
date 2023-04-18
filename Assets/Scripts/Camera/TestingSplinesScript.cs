using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSplinesScript : MonoBehaviour
{
    // InputManager inputManager;
    // PlayerMovement playerMovement;
    public float interpolateAmount;
    public float interpolateAmountX;
    public float interpolateAmountZ;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform pointC;
    [SerializeField] private Transform pointD;
    [SerializeField] private Transform pointABCD;
    [SerializeField] private Transform player;
    public bool splineCam;
    // Start is called before the first frame update
    void Awake()
    {
        //inputManager = FindObjectOfType<InputManager>();
        //playerMovement = FindObjectOfType<PlayerMovement>();
    }   

    // Update is called once per frame
    void Update()
    {
        //inverse lerp to get the players x position in relation to the start and finish then devide it over the finish
        //to get the percentage for the interpolateAmount.
        interpolateAmount = InverseLerp(pointA.position, pointD.position, player.position);

        //Interpolates between 4 points using 2 functions, the first interpolates a-b & b-c and then interpolates between them
        // to get a-b-c then interpolates between a-b-c & b-c-d
        pointABCD.position = CubicLerp(pointA.position, pointB.position, pointC.position, pointD.position, interpolateAmount);
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
}
