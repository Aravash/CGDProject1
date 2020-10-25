using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{


    public GameObject explosionPrefab;
    public List<GameObject> explosions;

    void Start()
    {
        explosions = new List<GameObject>();
    }


    void Update()
    {
        foreach (GameObject x in explosions)
        {
            Animator xyz = x.GetComponent<Animator>();
            float multi = TimeManager.Instance.getTimeMultiplier();
            xyz.SetFloat("timeMultiplier", multi);
        }
    }

    public void AddExplosion(Transform aaa)
    {
        GameObject qwerty = Instantiate(explosionPrefab, aaa.position, Quaternion.identity);
        explosions.Add(qwerty);
        Debug.Log("Added");
    }

    public void RemoveExplosion(GameObject bbb)
    {
        if(explosions.Contains(bbb))
        {
            explosions.Remove(bbb);
            Debug.Log("Removed " + bbb);
        }
        else
        {
            Debug.Log("Not Removed");
        }
    }
}
