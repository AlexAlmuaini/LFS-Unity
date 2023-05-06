using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Load : MonoBehaviour
{
    public bool next;
    public int sceneNumber;
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other == player.GetComponent<Collider>())
        {
            globalStuff.door_passed = true;
            globalStuff.door_number = SceneManager.GetActiveScene().buildIndex;

            LoadNextLevel();
        }
    }

    public Animator transition;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneNumber);
    }

    private void Update()
    {
        if(next)
        {
            LoadNextLevel();
        }
    }
}
