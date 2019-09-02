using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public GameObject[] waypoints;

    private Rigidbody NpcRb;
    public float speed = 5f;
    private bool arrived = false;
    private int index = 0;
    private GameManager gameManager;
    

    // Start is called before the first frame update
    void Start()
    {
        NpcRb = GetComponent<Rigidbody>();
        // NpcRb.AddForce(Vector3.forward * 2f, ForceMode.Impulse);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 waypointPosition = waypoints[index].transform.position;
        Vector3 toNextWaypoint = waypointPosition - transform.position;
        if (toNextWaypoint.magnitude < 2f)
        {
            index = (index + 1) % waypoints.Length;
        }
        Vector3 toWaypointNormalized = toNextWaypoint.normalized;
        NpcRb.AddForce(toWaypointNormalized * speed);
        */
        if (gameManager.isGameActive)
        {
            Vector3 nextWaypoint = waypoints[index].transform.position;
            Vector3 toNextWaypoint = nextWaypoint - transform.position;
            if (toNextWaypoint.magnitude < 2f)
            {
                index = (index + 1) % waypoints.Length;
            }
            transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            //audioSource.PlayOneShot(goalSound);
            gameManager.GoalSound();
            gameManager.GameOver();
        }
    }

}
