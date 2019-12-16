using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKangaroo : MonoBehaviour
{
    // code from https://learn.unity.com/tutorial/environment-and-player?projectId=5c51479fedbc2a001fd5bb9f#5c7f8529edbc2a002053b786
    public float speed;
    public float accelSpeed;
    public float disGround; 
    public float jumpPower;
    public float xdrag;
    public bool isPunching;
    public bool canMove; //if false, stops player movement inputs from working.
    public int punchCharge = 200;
    public int punchState = 0; //set to 0 for first state, 1 for punching state and 2 for end state. -1 for if not punching.
    public Animator thisAnimator;

    private Rigidbody rb;
    private float moveHorCur = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        disGround = GetComponent<Collider>().bounds.extents.y;
        isPunching = false;
        canMove = true;
        punchState = -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHor = 0;//moved this here
        if(canMove){//just to check if player inputs are allowed.
            if(Input.GetButton("Fire1")
            && (!isPunching) )
            {
                isPunching = true;
                StartPunching();
            }
            //float moveHor = 0; //moved outside of scope
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveHor = -1;
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                moveHor = 1;
                this.GetComponent<SpriteRenderer>().flipX = true;
            }
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
        if(onGround && Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity += Vector3.up * Time.deltaTime * jumpPower;
            //thisAnimator.SetBool("isJumping", true);
        }
        thisAnimator.SetBool("isJumping", !onGround);
        float moveVer = Input.GetAxis ("Vertical");

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

    public void StartPunching(){
        if(punchCharge <= 0){
            punchPhase2();//redirects to next phase
        }
        thisAnimator.SetInteger("punchState", 0);
        canMove = false;
        punchCharge -= 1;
        
    }

    public void EndPunchAnimation()
    {
        thisAnimator.SetBool("isPunching", false);
        isPunching = false;
    }
}
