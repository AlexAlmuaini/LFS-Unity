using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;
    [SerializeField] TestingSplinesScript testingSplinesScript;
    DoorBehaviour doorBehaviour;
    [SerializeField] public GameObject door;
    public GameObject cameraPivotObject;
    public Transform targetTransform, cameraPivot;
    private Transform cameraTransform;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private  Vector3 cameraVectorPosition;
    Vector3 rotation;
    Quaternion targetRotation, currentRotation;
    private float defaultPosition;
    public LayerMask collisionLayers;
    public Vector3 offset, doorCamOffset;
    public float followSpeed = 0.2f, interpolateAmount, lookAngle, pivotAngle, lookSpeed = 2.0f, pivotSpeed = 2.0f, minPivotAngle, maxPivotAngle, cameraCollisionRadius = 0.2f
    , cameraCollisionOffset = 0.2f, minimumCollisionOffset = 0.2f, doorCamTimer = 0.0f;
    public bool fixRot;

    private void Awake()
    {
        // THIS IS HOW TO FIX NULL OBJECT ERROR ~ ALEX PLEASE REMEMBER THIS
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        inputManager = FindObjectOfType<InputManager>();
        testingSplinesScript = FindObjectOfType<TestingSplinesScript>();
        cameraTransform = Camera.main.transform;
        playerMovement = FindObjectOfType<PlayerMovement>();
        defaultPosition = cameraTransform.localPosition.z;
        doorBehaviour = FindObjectOfType<DoorBehaviour>();
    }
    private void FollowTarget()
    {
        Vector3 desiredPosition = targetTransform.position + offset;
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref cameraFollowVelocity, followSpeed);
        Vector3 targetDistance = new Vector3(0.0f, 1.6f,-0.7f);
        cameraPivotObject.transform.localPosition = targetDistance;
        transform.position = targetPosition;
    }
    private void rotationFix()
    {
        currentRotation = transform.rotation;
        fixRot = true;
    }

    private void RotateCamera()
    {

        lookAngle = lookAngle + inputManager.horizontalLookInput * lookSpeed;
        pivotAngle = pivotAngle + inputManager.verticalLookInput * pivotSpeed;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        if(fixRot)
        {
            interpolateAmount = (interpolateAmount + Time.deltaTime * 1.5f);
            targetRotation = Quaternion.Lerp(currentRotation, targetRotation, interpolateAmount);
            if(interpolateAmount >=1)
            {
                interpolateAmount = 0;
                fixRot= false;
            }
        }
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    public void HandelAllCameraMovement()
    {
        if(playerMovement.followCam)
        {
            FollowTarget();
            RotateCamera();
        } 
        if(doorBehaviour.doorOpening)
        {
            HandleDoorCam();
        }
        if (testingSplinesScript.splineCam)
        {
            HandleSplineCam();
        }  
        HandleCameraCollision();
    }

    public void HandleDoorCam()
    {
        //starts timer for how long to look at door
        doorCamTimer += Time.deltaTime;

        // resets camera local positions
        Vector3 targetDistance =  Vector3.zero;
        Vector3 desiredPosition = door.transform.position + doorCamOffset;
        Vector3 targetPosition = desiredPosition;
        transform.position = targetPosition;
        
        //points camera at door
        Vector3 lookAtPosition = door.transform.position + transform.up * 1.8f;
        var targetRot = Quaternion.LookRotation(lookAtPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5 * Time.deltaTime);
        cameraPivotObject.transform.localPosition = targetDistance + transform.up * 0.6f;
        //cameraTransform.position = targetDistance;

        // resets rotation
        Vector3 rotation;
        rotation = Vector3.zero;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;

        // switches cam back to player after 2.5 seconds
        if(doorCamTimer >= 2.5)
        {
            doorBehaviour.doorOpening = false;
            playerMovement.followCam = true;
        }
    }

    public void HandleSplineCam()
    {
        Vector3 lookAtPosition = playerMovement.transform.position + transform.up * 1.8f;
        var targetRotation = Quaternion.LookRotation(lookAtPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }

    private void HandleCameraCollision()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if(Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = - (distance - cameraCollisionOffset);
        }

        if(Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = targetPosition - minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
