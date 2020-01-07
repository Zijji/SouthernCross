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
    public bool isUppercut;
    public bool canMove; //if false, stops player movement inputs from working.
    
    public int punchCharge = 50;
    private int resetPunchCharge;
    public int launchCharge = 50;
    private int resetLaunchCharge;
    public int punchCooldown = 100;
    private int resetPunchCooldown;
    public int uppercutCooldown = 4;       //Time spent in uppercut stage 1. stages 2,3,4 are based on whether the roo is rising or falling
    private int uppercutWait = 0;       //Time spent in uppercut stage 1. stages 2,3,4 are based on whether the roo is rising or falling

    public GameObject hitbox;
    public GameObject leftPunchSP;      //left punch spawn point for hitboxes
    public GameObject rightPunchSP;           //right punch spawn point for hitboxes
    public GameObject leftUppercutSP;      //left punch spawn point for hitboxes
    public GameObject rightUppercutSP;           //right punch spawn point for hitboxes

    //public int punchState = 0; //set to 0 for first state, 1 for punching state and 2 for end state. -1 for if not punching.
    public enum PunchState { notPunching, windUp, punch, cooldown };
    private PunchState curPunchState = PunchState.notPunching;

    //public int punchState = 0; //set to 0 for first state, 1 for punching state and 2 for end state. -1 for if not punching.
    public enum UppercutState { notUppercut, windUp, uppercut, rising, peaking, falling };
    private UppercutState curUppercutState = UppercutState.notUppercut;
    
    public float punchSpeed = 50f;
    public Animator thisAnimator;

    private Rigidbody rb;
    private float moveHorCur = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        disGround = GetComponent<Collider>().bounds.extents.y;
        isPunching = false;
        isUppercut = false;
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
        if (Physics.Raycast(transform.position, -Vector3.up, disGround + 0.1f))  //source: https://answers.unity.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html
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

        if (Input.GetButton("Fire1")
        && (!isPunching))
        {
            isPunching = true;//activates later code;
            //Note: isPunching is active no matter what the punch, isUppercut only activates when uppercut punching
            isUppercut = false;
            if (Input.GetAxis("Vertical") > 0)
            {
                isUppercut = true;
            }
            else
            {
                //punchState = 0;
                curPunchState = PunchState.windUp;
                if (!onGround)
                {
                    rb.useGravity = false;
                    rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
                }
            }
            canMove = false;
            

        }

        if (isPunching)
        {
            if(isUppercut)
            {
                switch(curUppercutState)
                {
                    case UppercutState.notUppercut:      //Note: Does not use windup right now, may need it in future.
                        curUppercutState = UppercutState.uppercut;
                        uppercutWait = uppercutCooldown;
                        thisAnimator.SetBool("isUppercut", true);
                        break;
                    case UppercutState.windUp:      //Note: Does not use windup right now, may need it in future.
                        break;
                    case UppercutState.uppercut:
                        uppercutWait--;
                        if(uppercutWait <= 0)
                        {
                            uppercutWait = 0;
                            thisAnimator.SetInteger("uppercutState", 1);
                            curUppercutState = UppercutState.rising;
                            canMove = true;
                        }
                        break;
                    case UppercutState.rising:
                        if(onGround)    
                        {
                            thisAnimator.SetBool("isUppercut", false);
                            thisAnimator.SetInteger("uppercutState", 0);
                            isPunching = false;
                            isUppercut = false;
                            curUppercutState = UppercutState.notUppercut;
                        }
                        break;
                }
            }
            else
            {
                thisAnimator.SetBool("isPunching", true);
                if (curPunchState == PunchState.windUp)
                {
                    thisAnimator.SetInteger("punchState", 0);
                    punchCharge -= 1;
                }
                if (curPunchState == PunchState.punch)
                {
                    thisAnimator.SetInteger("punchState", 1);
                    launchCharge -= 1;
                }
                if (curPunchState == PunchState.cooldown)
                {
                    thisAnimator.SetInteger("punchState", 2);
                    punchCooldown -= 1;
                }
                if (punchCharge <= 0)
                {
                    curPunchState = PunchState.punch;
                }
                if (launchCharge <= 0)
                {
                    curPunchState = PunchState.cooldown;
                }
                if (punchCooldown <= 0)
                {
                    thisAnimator.SetInteger("punchState", -1);
                    curPunchState = PunchState.notPunching;//punch is over
                    if (!rb.useGravity)
                    {
                        rb.useGravity = true;
                    }
                    resetValues();
                }
            }
            
        }


        /*
            Uppercut code:

            Uppercut concept: 
                Kangaroo launches upward on a slightly forwards trajectory. 
        */

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
        if (canMove)
        {
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

        if (onGround && Input.GetButton("Jump") && canMove)
        {
            rb.velocity += Vector3.up * Time.deltaTime * jumpPower;
            //thisAnimator.SetBool("isJumping", true);
        }
        thisAnimator.SetBool("isJumping", !onGround);
        float moveVer = Input.GetAxis("Vertical");

        //Debug.Log(moveHor);
        Vector3 movement = new Vector3(moveHor, 0.0f, 0.0f); //if canMove = false, this won't do anything
        int dirFacing = -1;//gets the direction facing
        if (this.GetComponent<SpriteRenderer>().flipX)
        {
            dirFacing = 1;
        }
        float moveSpeed = 0.0f; //Actual speed that the character moves at.
        if(isUppercut && curUppercutState == UppercutState.uppercut)
        {
            movement = new Vector3(0.0f, 1.0f, 0.0f);
            moveSpeed = punchSpeed;
            //Creates hit boxes
            if(dirFacing == -1)
            {
                var newHitbox = Instantiate(hitbox, leftUppercutSP.transform.position, Quaternion.identity);
                Destroy(newHitbox, 0.1f);
            }
            else if(dirFacing == 1)
            {
                var newHitbox = Instantiate(hitbox, rightUppercutSP.transform.position, Quaternion.identity);
                Destroy(newHitbox, 0.1f);
            }
        }
        else if (curPunchState == PunchState.punch)
        {
            movement = new Vector3(dirFacing, 0.0f, 0.0f);
            moveSpeed = punchSpeed;
            if(dirFacing == -1)
            {
                var newHitbox = Instantiate(hitbox, leftPunchSP.transform.position, Quaternion.identity);
                Destroy(newHitbox, 0.1f);
            }
            else if(dirFacing == 1)
            {
                var newHitbox = Instantiate(hitbox, rightPunchSP.transform.position, Quaternion.identity);
                Destroy(newHitbox, 0.1f);
            }
            
        }
        else// if (curPunchState == PunchState.notPunching)// if(curPunchState !=)
        {
            moveSpeed = speed;
        }
        //transform.position += movement * speed * Time.deltaTime;
        rb.velocity += movement * moveSpeed * Time.deltaTime;
        rb.velocity = new Vector3(rb.velocity.x * xdrag, rb.velocity.y, rb.velocity.z);
        //Debug.Log(rb.velocity);


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


    public void resetValues()
    {//for use when punching is done.
        punchCharge = resetPunchCharge;
        launchCharge = resetLaunchCharge;
        punchCooldown = resetPunchCooldown;
        isPunching = false;
        canMove = true;
    }
}
