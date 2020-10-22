using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    GameObject child = null;

    // Start is called before the first frame update
    void Start()
    {
        activate();
    }

    public void activate()
    {
        if (child != null)
        {
            Destroy(child);
        }

        child = Instantiate(Resources.Load("Prefabs/Enemy") as GameObject);
    }


    // draw a circle in editor to ease level design
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.4f);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.3f, 0.3f);
        Gizmos.DrawSphere(transform.position, 0.45f);
    }
}
