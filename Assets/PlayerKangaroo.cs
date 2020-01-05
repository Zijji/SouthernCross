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
    public bool canPunch = true;    //If false, disables punch
    public int punchCharge = 50;
    private int resetPunchCharge;
    public int launchCharge = 50;

    //Uppercut variables
    public bool canUppercut = true;
    public bool punchIsUppercut = false;
    public float uppercutSpeed = 50.0f;

    private int resetLaunchCharge;
    public int punchCooldown = 100;
    private int resetPunchCooldown;
    //public int punchState = 0; //set to 0 for first state, 1 for punching state and 2 for end state. -1 for if not punching.
    public enum PunchState {notPunching, windUp, punch, cooldown};
    private PunchState curPunchState = PunchState.notPunching; 
    public float punchSpeed = 50f;
    public Animator thisAnimator;

    private Rigidbody rb;
    //private float moveHorCur = 0;

    public GameObject[] hitBoxLoc;     //The positions of the hitbox objects inside the array is important. 0 = left; 1 = right; 2 = leftUpper; 3 = rightUpper;

    public GameObject hitBox;

    //PunchType used for creating hitboxes
    public enum PunchType {left, right, leftupper, rightupper};
    private PunchType curPunchType;

    
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        disGround = GetComponent<Collider>().bounds.extents.y;
        isPunching = false;
        canMove = true;
        //punchState = -1;
        resetPunchCharge = punchCharge;
        resetLaunchCharge = launchCharge;
        resetPunchCooldown = punchCooldown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Calculates onGround to prevent falling while punching in mid air.
        bool onGround = false;
        LayerMask actorMask = LayerMask.GetMask("Ground");
        if(Physics.Raycast(transform.position, -Vector3.up, disGround + 0.1f,actorMask))  //source: https://answers.unity.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html
        {
            onGround = true;
        }
        
        /*
        Punching code

        Punching concept:
            when the player presses the punch button, the kangaroo stops moving and enters the charging state. After punchCharge has depleted,
            the kangaroo enters the next state, where it launches forward with a hitbox. Once launchCharge has depleted, the kangaroo enters the next state.
            This state is a cooldown state, where the kangaroo cannot move for punchCooldown amount of time.
        */


        //Re enables punch
        if(onGround && curPunchState == PunchState.notPunching)
        {
            canPunch = true;
        }
        if(Input.GetButton("Fire1")
        && !isPunching 
        && canPunch)
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                punchIsUppercut = true;
                //transform.Rotate(transform.rotation.x,transform.rotation.y,-90.0f);
                //this.GetComponent<SpriteRenderer>().flipX = this.GetComponent<SpriteRenderer>().flipY;
            }
            else
            {
                punchIsUppercut = false;
            }
            //Gets punch type (uppercut, left or right)
            

            if(this.GetComponent<SpriteRenderer>().flipX == true)
            {
                if(punchIsUppercut)
                {
                    curPunchType = PunchType.rightupper;
                }
                else
                {
                    curPunchType = PunchType.right;
                }
            }
            else
            {
                if(punchIsUppercut)
                {
                    curPunchType = PunchType.leftupper;
                }
                else
                {
                    curPunchType = PunchType.left;
                }
            }
            isPunching = true;//activates later code;
            canPunch = false;
            //punchState = 0;
            curPunchState = PunchState.windUp;
            if(!onGround){
                rb.useGravity = false;
                rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            }
            
        }

        if(isPunching){
            thisAnimator.SetBool("isPunching", true);
            canMove = false;
            if(curPunchState == PunchState.windUp){
                thisAnimator.SetInteger("punchState", 0);
                punchCharge -= 1;
            }
            if(curPunchState == PunchState.punch){
                thisAnimator.SetInteger("punchState", 1);
                launchCharge -= 1;
                //Creates hitbox for punches
                CreateHitbox();
            }
            if(curPunchState == PunchState.cooldown){
                thisAnimator.SetInteger("punchState", 2);
                punchCooldown -= 1;
                CreateHitbox();
            }
            if(punchCharge <= 0){
                    curPunchState = PunchState.punch;
                }
            if(launchCharge <= 0){
                curPunchState = PunchState.cooldown;
            }
            if(punchCooldown <= 0){
                thisAnimator.SetInteger("punchState", -1);
                curPunchState = PunchState.notPunching;//punch is over
                if(punchIsUppercut)
                {
                    //transform.Rotate(transform.rotation.x,transform.rotation.y,90.0f);
                    this.GetComponent<SpriteRenderer>().flipY = false;
                }
                if(!rb.useGravity)
                {
                    rb.useGravity = true;
                }
                resetValues();
            }
        }

    //        public void StartPunching(){
    //     if(punchCharge <= 0){
    //         isPunching = false;
    //         thisAnimator.SetInteger("punchState", -1);
            
    //         canMove = true;
    //     }
    //     thisAnimator.SetInteger("punchState", 0);
    //     canMove = false;
    //     if(punchCharge <=0){
    //         punchCharge -= 1;
    //     }
    // }      

        //flipping
        float moveHor = 0;//moved this here
        if(canMove){
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
        
        if(onGround && Input.GetKey(KeyCode.UpArrow)&&canMove)
        {
            rb.velocity += Vector3.up * Time.deltaTime * jumpPower;
            //thisAnimator.SetBool("isJumping", true);
        }
        thisAnimator.SetBool("isJumping", !onGround);
        float moveVer = Input.GetAxis ("Vertical");

        //Debug.Log(moveHor);
        Vector3 movement = new Vector3 (moveHor, 0.0f, 0.0f ); //if canMove = false, this won't do anything
        int dirFacing = -1;//gets the direction facing
        if(this.GetComponent<SpriteRenderer>().flipX){
            dirFacing = 1;
        }
        float moveSpeed = 0.0f; //Actual speed that the character moves at.
        //Moves character when punching
        if(curPunchState == PunchState.punch){
            if(punchIsUppercut)
            {
                movement = new Vector3(0.0f, 1.0f, 0.0f);  //
                moveSpeed = uppercutSpeed;
            }
            else
            {
                movement = new Vector3(dirFacing, 0.0f, 0.0f);
                moveSpeed = punchSpeed;

            }
        }
        else if(curPunchState == PunchState.notPunching)// if(curPunchState !=)
        {
            moveSpeed = speed;
        }
        //transform.position += movement * speed * Time.deltaTime;
        rb.velocity += movement * moveSpeed * Time.deltaTime;
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

    private void CreateHitbox()
    {
        GameObject nHB;
        int hitBoxLocIndex = 0;
        switch(curPunchType)
        {
            case PunchType.left:
                hitBoxLocIndex = 0;
                break;
            case PunchType.right:
                hitBoxLocIndex = 1;
                break;
            case PunchType.leftupper:
                hitBoxLocIndex = 2;
                break;
            case PunchType.rightupper:
                hitBoxLocIndex = 3;
                break;
        }
        nHB = Instantiate(hitBox, hitBoxLoc[hitBoxLocIndex].transform.position,Quaternion.identity);
        Destroy(nHB, 0.05f); 
    }

    public void EndPunchAnimation()
    {
        thisAnimator.SetBool("isPunching", false);
        isPunching = false;
    }


    public void resetValues(){//for use when punching is done.
        punchCharge = resetPunchCharge;
        launchCharge = resetLaunchCharge;
        punchCooldown = resetPunchCooldown;
        isPunching = false;
        canMove = true;
    }
}
