using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPunchable : MonoBehaviour
{
    public bool isPunched;
    public int punchType;   //Gets punch type from the hitbox. -1 == not punched; 0 == left; 1 == right; 2 == left uppercut; 3 == right uppercut; 
    // Start is called before the first frame update
    void Start()
    {
        isPunched = false;
        punchType = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Hitbox"))
        {
            isPunched = true;
            punchType = other.gameObject.GetComponent<Hitbox>().hitboxType;
        }
    }

    //Returns true if the enemy has been punched, false otherwise.
    public bool HasBeenPunched()
    {
        return isPunched;
    }

    //resets punch for e.g. enemies with higher health
    public void ResetPunch()
    {
        isPunched = false;
    }
}
