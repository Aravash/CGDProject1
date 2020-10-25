using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bulletRender : MonoBehaviour
{
    private Image[] bullets = new Image[6];
    private Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        
        int i = 0;
        foreach (Image img in bullets)
        {
            bullets[i] = transform.GetChild(i).GetComponent<Image>();
            Debug.Log("assigned 1 bullet to an image");
            //bullets[i].enabled = false;
            i++;
        }
        Debug.Log(bullets[5]);
    }

    private void Update()
    {
        int i = 0;
        foreach (Image img in bullets)
        {
            if (i < player.getCurrentAmmo())
            {
                bullets[i].enabled = true;
            }
            else bullets[i].enabled = false;

            i++;
        }
    }
}
