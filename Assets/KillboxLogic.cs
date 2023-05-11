using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillboxLogic : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] Transform respawnPoint;
    float death;
    // Start is called before the first frame update
    void Start()
    {
     playerMovement = FindObjectOfType<PlayerMovement>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(death >= 3)
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        death++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
