using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    GameObject child = null;
    private Vector2 spot;

    // Start is called before the first frame update
    void Start()
    {
        spot = transform.GetChild(0).transform.position;
        activate(true);
    }

    public void activate(bool initial = false)
    {
        if (child != null)
        {
            Destroy(child);
        }
        child = Instantiate(Resources.Load("Prefabs/Enemy") as GameObject, gameObject.transform);
        child.GetComponent<enemyBehaviour>().setPatrolPos(spot);

        // if this is a REspawn, not an initial spawn, set to wake immediately
        if(!initial)
        {
            child.GetComponent<enemyBehaviour>().wake();
        }
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
