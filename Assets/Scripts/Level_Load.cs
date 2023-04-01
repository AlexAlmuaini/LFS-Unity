using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Load : MonoBehaviour
{
    public bool next;
    public GameObject Player;
    private void OnTriggerEnter(Collider other)
    {
        if(other = Player.GetComponent<Collider>())
        {
            LoadNextLevel();
        }
    }

    public Animator transition;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);
    }

    private void Update()
    {
        if(next)
        {
            LoadNextLevel();
        }
    }
}
