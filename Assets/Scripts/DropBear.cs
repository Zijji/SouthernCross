using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBear : MonoBehaviour
{
    private Rigidbody rb;
    private Transform  trans;
    private GameObject player;
    public float attackDistance;
    public float moveSpeed;
    public float xdrag;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();

        if(GameObject.Find("Kangaroo")){//if the kangaroo is in the scene
            player = GameObject.Find("Kangaroo");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);//resetting this for next frame
        //if(player){
        float distance = trans.position.x - player.GetComponent<Transform>().position.x;
        if(distance <= 10 && distance >= -10){
            //distance will be negative if player is right of DropBear, positive if on left
            //run towards player
            if(Mathf.Sign(distance)*distance > attackDistance){
                if(distance > 0){
                    movement = new Vector3(Mathf.Sign(-distance), 0.0f, 0.0f);
                } else {
                    movement = new Vector3(Mathf.Sign(distance), 0.0f, 0.0f);
                }

            }

            rb.velocity += movement * moveSpeed * Time.deltaTime;
            rb.velocity = new Vector3(rb.velocity.x * xdrag, rb.velocity.y, rb.velocity.z);
            
        }

        //}
    }
}
