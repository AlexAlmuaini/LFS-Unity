using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviourScript : MonoBehaviour
{

    InputManager inputManager;

    public bool canPressButton = false, buttonPressed = false;
    private float interpolateAmount;

    void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }
    void Update()
    {
        if(buttonPressed)
        {
            interpolateAmount += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.down * .5f, interpolateAmount);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player")
            {
                canPressButton = true;
            }
        else
        {
            canPressButton = false;
        }
    }
}
