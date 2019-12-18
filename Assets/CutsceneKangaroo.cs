using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneKangaroo : MonoBehaviour
{
    // code from https://learn.unity.com/tutorial/environment-and-player?projectId=5c51479fedbc2a001fd5bb9f#5c7f8529edbc2a002053b786
    public float speed;
    public float accelSpeed;
    public float disGround; 
    public float jumpPower;
    public float xdrag;
    public bool isPunching;
    public float xPress = 0.0f;
    public float yPress = 0.0f;

    public Animator thisAnimator;

    private Rigidbody rb;
    private float moveHorCur = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        disGround = GetComponent<Collider>().bounds.extents.y;
        isPunching = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isPunching)
        {
            isPunching = true;
            thisAnimator.SetBool("isPunching", true);
        }
        float moveHor = 0;
        if (xPress < 0.0f)
        {
            moveHor = -1;
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (xPress > 0.0f)
        {
            moveHor = 1;
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        /*
        if(moveHorCur != moveHor)
        {
            moveHorCur += moveHor * accelSpeed * Time.deltaTime;
            if(Mathf.Abs(moveHorCur) > Mathf.Abs(moveHor))
            {
                moveHorCur = moveHor;
            }
        }
        Debug.Log(moveHor);
        
        Debug.Log(moveHorCur);

        */
        /*
        if(Input.GetAxis ("Horizontal") < 0)
        {
            moveHorizontal = -1;
        }
        else if(Input.GetAxis ("Horizontal") > 0)
        {
            moveHorizontal = 1;
        }
        */
        bool onGround = false;
        
        if(Physics.Raycast(transform.position, -Vector3.up, disGround + 0.1f))  //source: https://answers.unity.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html
        {
            onGround = true;
            /*
            if(thisAnimator.GetBool("isJumping") == false)
            {
                thisAnimator.SetBool("isJumping", false);
            }
             */
        }
        if(onGround && yPress > 0.0f)
        {
            rb.velocity += Vector3.up * Time.deltaTime * jumpPower;
            //thisAnimator.SetBool("isJumping", true);
        }
        thisAnimator.SetBool("isJumping", !onGround);
        //float moveVer =  yPress > 0.0f;

        //Debug.Log(moveHor);
        Vector3 movement = new Vector3 (moveHor, 0.0f, 0.0f );

        //transform.position += movement * speed * Time.deltaTime;
        rb.velocity += movement * speed * Time.deltaTime;
        rb.velocity = new Vector3(rb.velocity.x * xdrag, rb.velocity.y, rb.velocity.z);
        Debug.Log(rb.velocity);

        
        //transform.Translate(movement * speed * Time.deltaTime);
        /*
        if (Input.GetKey(KeyCode.LeftArrow))
        {

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {

        }
        */
    }
    public void EndPunchAnimation()
    {
        thisAnimator.SetBool("isPunching", false);
        isPunching = false;
    }
}
