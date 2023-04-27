using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    Vector3 offset;

    [SerializeField] float look_sensitivity = 0.2f;
    [SerializeField] float max_look_speed = 2f;

    private Input controls;
    private Vector2 rotation;

    private float rotationX;
    private float rotationY;

    [SerializeField] public bool lookCamEnabled = false;
    

    private void Awake()
    {
        controls = new Input();
        offset = transform.position - target.transform.position;
        lookCamEnabled = true;
    }

    public void OnEnable()
    {
        controls.Enable();
        
    }

    public void OnDisable()
    {
        controls.Disable();
    }

    public void Update()
    {
        var look = controls.Player.Look.ReadValue<Vector2>();
        var move = controls.Player.Move.ReadValue<Vector2>();
        
        if(lookCamEnabled)
        {
            Look(look);
        }

        /*else if(!lookCamEnabled)
        {
            Look(move);
            if(look.x is < 0 or > 0)
            {
                lookCamEnabled = true;
            }
        }*/
    }

    private void Look(Vector2 rotate)
    {
        if (rotate.x > 0.5 || rotate.x < -0.5)
        {
            var clampedRotateX = Mathf.Clamp(rotate.x, -look_sensitivity, look_sensitivity);
            offset = Quaternion.Euler(0, clampedRotateX, 0) * offset;
        }

        var desiredAngle = target.transform.eulerAngles.y;
        var clampedDesiredAngle = Mathf.Clamp(desiredAngle, -max_look_speed, max_look_speed);
        var qRotation = Quaternion.Euler(0, clampedDesiredAngle, 0);
        transform.position = target.transform.position + (qRotation * offset);
        transform.LookAt(target.transform);
    }
}
