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
        Debug.Log(curAttackState);
        Vector3 dirFacing = new Vector3(0.0f, 0.0f, 0.0f);//resetting this for next frame. This is the direction the DropBear is facing
        float distance = trans.position.x - player.GetComponent<Transform>().position.x;
        if (distance <= 10 && distance >= -10)
        {
            //attacking code. Identical to Kangaroo punching code.
            if (distance <= attackDistance && !isAttacking)
            {
                isAttacking = true;
                curAttackState = AttackState.windUp;
            }

            if (!isAttacking)
            {
                //distance will be negative if player is right of DropBear, positive if on left
                //run towards player
                if (Mathf.Sign(distance) * distance > attackDistance)
                {
                    dirFacing = new Vector3(Mathf.Sign(-distance), 0.0f, 0.0f);
                }
                if(curAttackState == AttackState.attack){
                    moveSpeed = attackSpeed;
                }
                rb.velocity += dirFacing * moveSpeed * Time.deltaTime;
                rb.velocity = new Vector3(rb.velocity.x * xdrag, rb.velocity.y, rb.velocity.z);
            }
        }
        if (isAttacking){ //separate code block because player might move out of range, stuffing everthing up.
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

    }
    public void resetValues()
    {
        attackCharge = attackChargeReset;
        launchCharge = launchChargeReset;
        attackCooldown = attackCooldownReset;
        isAttacking = false;
    }
}
