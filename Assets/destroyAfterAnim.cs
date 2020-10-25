using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyAfterAnim : MonoBehaviour
{
    ExplosionManager expMan;
    // Start is called before the first frame update
    void Start()
    {
        GameObject GameobjMan =  GameObject.FindGameObjectWithTag("ExplosionManager");
        expMan = GameobjMan.GetComponent<ExplosionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroySprite()
    {
        expMan.RemoveExplosion(gameObject);
        Destroy(gameObject);
    }
}
