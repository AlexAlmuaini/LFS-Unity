using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerMovement playerMovement;
    ButtonBehaviourScript buttonBehaviourScript;
    DoorBehaviour doorBehaviour;
    public Vector2 movementInput, lookInput;
    public float sprintFloat, lockOnCam, verticalMovementInput, horizontalMovementInput,verticalLookInput, horizontalLookInput;
    public bool jumpInput = false, interactInput = false;

    private void Awake()
    {
        // THIS IS HOW TO FIX NULL OBJECT ERROR ~ ALEX PLEASE REMEMBER THIS
        playerMovement = FindObjectOfType<PlayerMovement>();
        buttonBehaviourScript = FindObjectOfType<ButtonBehaviourScript>();
        doorBehaviour = FindObjectOfType<DoorBehaviour>();
    }
    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Player.Look.performed += i => lookInput = i.ReadValue<Vector2>();
            playerControls.Player.Sprint.performed += i => sprintFloat = i.ReadValue<float>();
            playerControls.Player.LockOnCam.performed += i => lockOnCam = i.ReadValue<float>();
            playerControls.Player.Jump.performed += i => jumpInput = true;
            playerControls.Player.Interact.performed += i => interactInput = true;
        }

        playerControls.Enable();
    }   

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInput()
    {
        HandleMovementInput();
        HandleJumpInput();
        HandleLookInput();
        HandleInteractInput();
    }

    private void HandleMovementInput()
    {
        verticalMovementInput = movementInput.y;
        horizontalMovementInput = movementInput.x;
    }

    private void HandleLookInput()
    {
        verticalLookInput = lookInput.y;
        horizontalLookInput = lookInput.x;
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            playerMovement.HandleJump();
        }
    }

    private void HandleInteractInput()
    {
        if(buttonBehaviourScript.canPressButton && interactInput)
        {
            buttonBehaviourScript.buttonPressed = true;
            buttonBehaviourScript.canPressButton = false;
            doorBehaviour.DoorOpen();
            doorBehaviour.doorOpening = true;
            playerMovement.followCam = false;
        }
        if(interactInput)
        {
            playerMovement.HandleAttack();
        }
        interactInput = false;
    }
}
