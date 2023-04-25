using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    private void Update()
    {
        transform.localScale = new Vector3(50,50,50);
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
