using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviourScript : MonoBehaviour
{
    InputManager inputManager;


    public bool canPressButton = false, buttonPressed = false;

    void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
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
