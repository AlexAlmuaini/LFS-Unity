using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLogic : MonoBehaviour
{
    Animator animator;
    private GameObject rubble;
    [SerializeField] GameObject rubbleParticles;
    [SerializeField] private float timer, platformFallTime = 1f, platformRecharge = .75f;
    [SerializeField] private bool fall;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("falling", false);
        fall = false;
    }
    void Update()
    {
        if(fall)
            {
                StartCoroutine(EHandlePlatformFalling());
            }
    }

    private void OnCollisionStay(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            timer = (timer + Time.deltaTime);
            if(timer >= .5f && timer <= .52f)
            {
                rubble = Instantiate(rubbleParticles, transform.position, transform.rotation);
                animator.SetBool("falling", true);
            }
            if(timer >= 3.5f)
            {
                fall = true;
            }
        }   
    }

    private void OnCollisionExit(Collision col)
    {
        StartCoroutine(EPlatformRecharge());
    }
    IEnumerator EPlatformRecharge()
    {
        yield return new WaitForSeconds(platformRecharge);
        Destroy(rubble);
        animator.SetBool("falling", false);
        timer = 0;
    }
    IEnumerator EHandlePlatformFalling()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.down * 5, Time.deltaTime * 1.5F);
        yield return new WaitForSeconds(platformFallTime);
        Destroy(gameObject);
        Destroy(rubble);
    }
}
