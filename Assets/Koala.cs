using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(IsPunchable))]
public class Koala : MonoBehaviour
{
    public bool isAlive = true;
    private IsPunchable isPunchComp;
    private Rigidbody rb;
    public float launchSpeedX;
    public float launchSpeedY;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isPunchComp = GetComponent<IsPunchable>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPunchComp.isPunched)
        {
            isAlive = false;
            //Re enables koala spinning and disables collision and other components
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<DropBear>().enabled = false;
            //send it flying
            rb.AddForce(launchSpeedX, launchSpeedY, 0, ForceMode.Impulse);
            //isPunchComp.punchType;
            GetComponent<Koala>().enabled = false;
        }
    }
}
