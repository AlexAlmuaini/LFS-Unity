using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerMovement playerMovement;
    ButtonBehaviourScript buttonBehaviourScript;
    DoorBehaviour doorBehaviour;
    [SerializeField] public GameObject slime;
    private GameObject babyslime;
    public Vector2 movementInput, lookInput;
    public float sprintFloat, interpolateAmount, lockOnCam, verticalMovementInput, horizontalMovementInput,verticalLookInput, horizontalLookInput;
    public bool jumpInput = false, interactInput = false;

    public int swapInput = 0;
    public GameObject torch;

    private void Awake()
    {
        // THIS IS HOW TO FIX NULL OBJECT ERROR ~ ALEX PLEASE REMEMBER THIS
        playerMovement = FindObjectOfType<PlayerMovement>();
        buttonBehaviourScript = FindObjectOfType<ButtonBehaviourScript>();
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
            playerControls.Player.Swapitem.performed += i => swapInput += 1;
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
        verticalLookInput = -1*lookInput.y;
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
            StartCoroutine(ESpawn());
        }
        if(interactInput)
        {
            playerMovement.HandleAttack();
        }
        interactInput = false;
    }

    IEnumerator ESpawn()
   {
    if(!buttonBehaviourScript.buttonPressed)
    {
        buttonBehaviourScript.buttonPressed = true;
    }
    yield return new WaitForSeconds(1.0f);
    Destroy(buttonBehaviourScript.gameObject);
    babyslime = Instantiate(slime, transform.position + Vector3.left * - 10, transform.rotation);
    babyslime = Instantiate(slime, transform.position + Vector3.forward *  -10, transform.rotation);
   }

    private void Update()
    {
        if (swapInput == 0)
        {
            torch.active = false;
        }
        if (swapInput == 1)
        {
            torch.active = true;
        }
        if (swapInput >= 2)
        {
            swapInput = 0;
        }
    }
}
