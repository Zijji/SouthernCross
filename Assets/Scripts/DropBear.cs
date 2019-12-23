using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBear : MonoBehaviour
{
    private Rigidbody rb;
    private Transform trans;
    private GameObject player;
    public float attackDistance;
    public float moveSpeed;
    private float moveSpeedSet;
    public float xdrag;
    private bool isAttacking;
    public int attackCharge = 50;
    private int attackChargeReset;
    public int launchCharge = 50;
    private int launchChargeReset;
    public int attackCooldown = 100;
    public int attackSpeed;

    private int attackCooldownReset;
    public Animator thisAnimator;
    public enum AttackState { notAttacking, windUp, attack, cooldown };
    private AttackState curAttackState = AttackState.notAttacking;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeedSet = moveSpeed;
        isAttacking = false;
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        //reset values
        attackChargeReset = attackCharge;
        launchChargeReset = launchCharge;
        attackCooldownReset = attackCooldown;


        if (GameObject.Find("Kangaroo"))
        {//if the kangaroo is in the scene
            player = GameObject.Find("Kangaroo");
        }

    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed = 0; //resetting this for next frame
        //distance will be negative if player is right of DropBear, positive if on left
        float distance = trans.position.x - player.GetComponent<Transform>().position.x;

        Vector3 dirFacing = new Vector3(Mathf.Sign(-distance), 0.0f, 0.0f);//This is the direction the DropBear is facing

        //run towards player if the player is within range.
        if (distance <= 10 && distance >= -10 && !isAttacking)
        {
            moveSpeed = moveSpeedSet;
            if (Mathf.Sign(distance) * distance <= attackDistance)
            {
                isAttacking = true;
                curAttackState = AttackState.windUp;
            }
        }

        //attacking code. Identical to Kangaroo punching code.
        if (isAttacking)
        { //separate code block because player might move out of range, stuffing everthing up.

            thisAnimator.SetBool("isAttacking", true);
            if (curAttackState == AttackState.windUp)
            {
                thisAnimator.SetInteger("attackState", 0);
                attackCharge -= 1;
            }
            if (curAttackState == AttackState.attack)
            {
                thisAnimator.SetInteger("attackState", 1);
                launchCharge -= 1;
            }
            if (curAttackState == AttackState.cooldown)
            {
                thisAnimator.SetInteger("attackState", 2);
                attackCooldown -= 1;
            }
            if (attackCharge <= 0)
            {
                curAttackState = AttackState.attack;
            }
            if (launchCharge <= 0)
            {
                curAttackState = AttackState.cooldown;
            }
            if (attackCooldown <= 0)
            {
                thisAnimator.SetInteger("AttackState", -1);
                thisAnimator.SetBool("isAttacking", false);
                curAttackState = AttackState.notAttacking;//punch is over
                resetValues();
            }
        }

        //movement
        if (curAttackState == AttackState.attack)
        {
            moveSpeed = attackSpeed;
        }
        //flipping
        if (distance < 0)
            this.GetComponent<SpriteRenderer>().flipX = true;
        if (distance > 0)
            this.GetComponent<SpriteRenderer>().flipX = false;

        rb.velocity += dirFacing * moveSpeed * Time.deltaTime;
        rb.velocity = new Vector3(rb.velocity.x * xdrag, rb.velocity.y, rb.velocity.z);
    }
    public void resetValues()
    {
        attackCharge = attackChargeReset;
        launchCharge = launchChargeReset;
        attackCooldown = attackCooldownReset;
        isAttacking = false;
    }
}
