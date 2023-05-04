using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboardScript : MonoBehaviour
{
    public Transform cam;
    ButtonBehaviourScript button;
    Canvas canvas;

    private void Awake()
    {
        button = FindObjectOfType<ButtonBehaviourScript>();
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }

    private void Update()
    {
        if (button.canPressButton)
        {
            canvas.enabled = true;
        }
        else
        {
            canvas.enabled = false;
        }

        if (button.buttonPressed)
        {
            Destroy(this.gameObject);
        }
    }
}