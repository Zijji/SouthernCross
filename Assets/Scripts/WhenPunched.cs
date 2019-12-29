using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenPunched : MonoBehaviour
{
    private Rigidbody rb;
    private Transform trans;
    private GameObject player;
    public Animator thisAnimator;
    public bool playerPunching = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();

        if (GameObject.Find("Kangaroo"))
        {//if the kangaroo is in the scene
            player = GameObject.Find("Kangaroo");
            playerPunching = player.GetComponent<PlayerKangaroo>().isPunching;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
