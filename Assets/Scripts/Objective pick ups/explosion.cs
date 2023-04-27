using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    private float scale;
    private void Update()
    {
        scale += Time.deltaTime;
        transform.localScale = new Vector3(scale * 25,scale * 25,scale * 25);
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
