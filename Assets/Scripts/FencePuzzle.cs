using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FencePuzzle : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject CutsceneCamera;
    public GameObject CutsceneCamera2;
    public GameObject timeline;
    public Rigidbody rb;
    public GameObject plank;
    public MeshRenderer rend;
    public bool startCutscene;
    
    void Awake()
    {
        CutsceneCamera.SetActive(false);
        CutsceneCamera2.SetActive(false);
        timeline.SetActive(false);
        playerCamera.SetActive(true);
        rend = plank.GetComponent<MeshRenderer>();
        rb = GameObject.Find("RPG-Character Variant").GetComponent<Rigidbody>();
    }

    IEnumerator FinishCut()
    {
        yield return new WaitForSeconds(7);
        playerCamera.SetActive(true);
        CutsceneCamera.SetActive(false);
        CutsceneCamera2.SetActive(false);
        timeline.SetActive(false);
        gameObject.SetActive(false);
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (startCutscene)
        {
            playerCamera.SetActive(false);
            timeline.SetActive(true);
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rend.enabled = false;
            StartCoroutine(FinishCut());
           


        }
    }
}
