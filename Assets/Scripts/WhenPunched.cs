using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script is for whenever the unit that possesses it is attacked by the player in
    any way. That is, punched, jumped on or uppercutted. 
    Public bools are present as to whether or not the unit can be affected by
    these methods of attack. They are set to true by default.
*/
public class WhenPunched : MonoBehaviour
{
    private Rigidbody rb;
    private Transform trans;
    private GameObject player;
    public Animator thisAnimator;
    public bool playerPunching = false;
    public bool canPunched = true;
    public bool canJumped = true;
    public bool canUppercut = true;
    public float launchSpeedX;
    public float launchSpeedY;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Kangaroo"))
        {//if the kangaroo is in the scene
            player = GameObject.Find("Kangaroo");
            playerPunching = player.GetComponent<PlayerKangaroo>().isPunching;
        }
    }

    //if the player collides with the current object, is punching and the current object can be punched
    //Current issue: this script will need to be personalised for each user, as some scripts will need to disable other components to work.
    void OnCollisionEnter(Collision col){
        //Debug.Log("Collided");
        if(col.collider.gameObject.name == "Kangaroo" && canPunched && playerPunching){
            /*
                NEXT LINE IS UNIQUE TO THE DROPBEAR OBJECT
            */
            this.GetComponent<DropBear>().enabled = false; 
            rb.AddForce(launchSpeedX, launchSpeedY, 0, ForceMode.Impulse);
            /*
                TODO:
                    destroy object if flung off screen
                    if launched into wall, stick to wall, then disappear
                    animate death
                    make it so that unit flies in correct direction

            */
       }
    }
}
