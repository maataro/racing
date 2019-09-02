using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraController : MonoBehaviour
{
     public GameObject player;
    [SerializeField] float anglerSpeed = 5f;
     private void Start()
     {
        transform.position = player.transform.position;
     }

     private void LateUpdate()
     {


        //Vector3 relativePos = player.transform.position - this.transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePos);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed);

        float step = anglerSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, step);


        transform.position = player.transform.position;
        transform.LookAt(player.transform.position);
        

    } 
  
}


