using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineLookAtScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleSplineCam();
    }

    private void HandleSplineCam()
    {
        Vector3 lookAtPosition = player.position + transform.up * 1.8f;
        var targetRotation = Quaternion.LookRotation(lookAtPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }
}
