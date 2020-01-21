using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        Debug.Log("Collision!");
        if(other.CompareTag("Hitbox"))
        {
            Debug.Log("Destroy!");
            Destroy(gameObject);
        }
    }
    /*
    void OnCollisionEnter(Collision col){
        Debug.Log("Collision!");
        if(col.collider.CompareTag("Hitbox"))
        {
            Debug.Log("Destroy!");
            Destroy(gameObject);
        }
        
    }
    */
}
