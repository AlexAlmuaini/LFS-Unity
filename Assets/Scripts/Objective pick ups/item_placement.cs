using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_placement : MonoBehaviour
{
    InputManager inputManager;
    GameObject player;

    public GameObject playerCam;
    public GameObject cutsceneCam;

    public bool rope_placed;
    public bool tnt_placed;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inputManager = player.GetComponent<InputManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == player.GetComponent<Collider>())
        {
            if (inputManager.swapInput == 3)
            {
                playerCam.SetActive(false);
                cutsceneCam.SetActive(true);
                StartCoroutine(Cutscene1());
            }
            if (inputManager.swapInput == 2)
            {
                playerCam.SetActive(false);
                cutsceneCam.SetActive(true);
                StartCoroutine(Cutscene2());
            }
        }
    }

    IEnumerator Cutscene1()
    {
        yield return new WaitForSeconds(2);
        rope_placed = true;
        globalStuff.wire_collected = false;
        yield return new WaitForSeconds(3);
        playerCam.SetActive(true);
        cutsceneCam.SetActive(false);
    }
    IEnumerator Cutscene2()
    {
        yield return new WaitForSeconds(2);
        tnt_placed = true;
        globalStuff.tnt_collected = false;
        yield return new WaitForSeconds(3);
        playerCam.SetActive(true);
        cutsceneCam.SetActive(false);
    }
}
