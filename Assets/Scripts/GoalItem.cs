using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalItem : MonoBehaviour
{
    public GameObject goalPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("NPC"))
        {
            Instantiate(goalPrefab, transform.position, goalPrefab.transform.rotation);
            Destroy(gameObject);
        }
    }

}
