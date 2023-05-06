using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Quaternion currentRotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
        Vector3 currentEulerAngle = currentRotation.eulerAngles;
        currentEulerAngle.x = 0f;
        currentEulerAngle.z = 0f;

        Quaternion newRotation = Quaternion.Euler(currentEulerAngle);
        transform.rotation = newRotation;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health; 
    }
}
