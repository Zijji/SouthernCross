﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePads : MonoBehaviour
{
    public float launchSpeed = 8f;
    void OnTriggerEnter(Collider other){
        //Debug.Log("Object entered the trigger");
        Rigidbody rb = other.attachedRigidbody;
        rb.AddForce(0, launchSpeed, 0);
        //other.transform.TransformDirection(Vector3.up*launchSpeed);
    }
    
    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
