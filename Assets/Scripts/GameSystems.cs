using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameSystems : MonoBehaviour
{
    public Image boss_HP_image;
     public GameObject boss_canvas;
    public float boss_max_hp;

    public GameObject Boss;
    KingSlimeBehaviour kingSlimeBehaviour;
    private void Awake()
    {
        kingSlimeBehaviour = Boss.GetComponent<KingSlimeBehaviour>();
    }

    private void Update()
    {
        boss_HP_image.fillAmount = kingSlimeBehaviour.health / boss_max_hp;
        if(kingSlimeBehaviour.health / boss_max_hp <= 0)
        {
            Destroy(boss_canvas);
        }
    }
}
