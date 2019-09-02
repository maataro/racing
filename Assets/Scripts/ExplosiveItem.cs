using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveItem : MonoBehaviour
{
    public GameObject explosionPrefab;
 


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
            Destroy(gameObject);
        }
    }
}
