using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bluebottle : MonoBehaviour
{

    public float launchSpeed = 8f;
    void OnTriggerEnter(Collider other){
        //Debug.Log("Object entered the trigger");
        Rigidbody rb = other.attachedRigidbody;
        if(rb.velocity.y < 0){
            rb.AddForce(0, launchSpeed, 0, ForceMode.Impulse);
            Destroy(transform.parent.gameObject);
        }
        
        //other.transform.TransformDirection(Vector3.up*launchSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
