using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyAfterAnim : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float multi = TimeManager.Instance.getTimeMultiplier();
        animator.SetFloat("timeMultiplier", multi);
    }

    void DestroySprite()
    {
        Destroy(gameObject);
    }
}
