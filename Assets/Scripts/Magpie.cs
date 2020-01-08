using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magpie : MonoBehaviour
{
    public float flySpeed;
    private Vector3 home;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //set the starting point of the magpie to return to later.
        home = transform.position;
        if (GameObject.Find("Kangaroo"))
        {//if the kangaroo is in the scene
            player = GameObject.Find("Kangaroo");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float step = flySpeed * Time.deltaTime;
        //fly towards player and attack them. 
        //Once player damage is added, make it so that the magpie will fly back to home once the player has been attacked.
        if(player.GetComponent<PlayerKangaroo>().inSand)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
        if(!player.GetComponent<PlayerKangaroo>().inSand){
            transform.position = Vector3.MoveTowards(transform.position, home, step);
        }
    }
}
