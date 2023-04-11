using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    private void Update()
    {
        transform.localScale = new Vector3(30,30,30);
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
