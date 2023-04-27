using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //input fields
    private Input actions;
    private InputAction move;
    private InputAction look;
    
    //movement
    private Rigidbody rb;
    
    //parameters
    [SerializeField] private float movement_force = 1.0f;
    [SerializeField] private float jump_force = 5.0f;
    [SerializeField] private float start_speed = 5.0f;
    public float max_speed = 5.0f;

    private Vector3 force_direction = Vector3.zero;

    [SerializeField] private Camera playerCamera;
    private Animator animator;
    public FollowCamera followCamera;
    public bool speedPickedUp;
    public float currentSpeedTime = 0f;
    public float startingSpeedTime = 5f;
    public float boosted_speed = 10.0f;
    public float jumps = 2.0f;

    DoubleJumpManager doubleJumpManager;


    public void Start()
    {
        Collider myCollider = GetComponent<Collider>();

        // Set the tag of the collider
        myCollider.tag = "Player";
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        actions = new Input();
        animator = GetComponent<Animator>();
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        doubleJumpManager = FindObjectOfType<DoubleJumpManager>();
        followCamera = cam.GetComponent<FollowCamera>();
        max_speed = start_speed;
        currentSpeedTime = startingSpeedTime;
        speedPickedUp = false;
    }

    private void OnEnable()
    {
        actions.Player.Jump.started += DoJump;
        actions.Player.Attack.started += DoAttack;
        actions.Player.Interact.started += DoInteraction;
        actions.Player.ToggleLockCam.started += ToggleLockCam;

        move = actions.Player.Move;
        look = actions.Player.Look;

        actions.Player.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Jump.started -= DoJump;
        actions.Player.Attack.started -= DoAttack;
        actions.Player.Interact.started -= DoInteraction;
        actions.Player.ToggleLockCam.started -= ToggleLockCam;
        
        actions.Player.Disable();
    }

    private void FixedUpdate()
    {
        force_direction += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movement_force;
        force_direction += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movement_force;

        //clamp force directions 

        rb.AddForce(force_direction, ForceMode.Impulse);
        force_direction = Vector3.zero;

        if (rb.velocity.y < 0f)
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if(horizontalVelocity.sqrMagnitude > max_speed * max_speed)
        {
            rb.velocity = horizontalVelocity.normalized * max_speed + Vector3.up * rb.velocity.y;
        }

        LookAt();
        HandleSpeedTimer();
    }

    private void HandleSpeedTimer()
    {
        if(speedPickedUp)
        {
            currentSpeedTime -= 1 * Time.deltaTime;
            Debug.Log(currentSpeedTime);

            if (currentSpeedTime <= 0)
            {
                max_speed = start_speed;
                speedPickedUp = false;
            }
        }
    }
    
    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if(move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);

        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
    private Vector3 GetCameraForward(Camera cam)
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera cam)
    {
        Vector3 right = cam.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            force_direction += Vector3.up * jump_force;
            jumps--;
        }

        if(!IsGrounded() && doubleJumpManager.canDoubleJump == true)
                {
                    if(jumps == 1)
                    {
                        force_direction += Vector3.up * jump_force;
                        jumps--;
                    }
                }
        animator.SetTrigger("Jump");
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);

        if(Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            return true;
        }
        else
        {
            return false;
        }
    } 
    
    private void DoAttack(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Attack");
    }


    private void DoInteraction(InputAction.CallbackContext obj)
    {
        Debug.Log("Do Interaction");
    }

    private void ToggleLockCam(InputAction.CallbackContext obj)
    {
        Debug.Log("Toggle Lock Cam Called");
        if(followCamera.lookCamEnabled)
        {
            followCamera.lookCamEnabled = false;
        }
        else
        {
            followCamera.lookCamEnabled = true;
        }
    }
}

