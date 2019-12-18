using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHatToPoint : MonoBehaviour
{
    public GameObject getHat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(getHat.transform.position != this.transform.position)
        {
            getHat.transform.position = this.transform.position;
        }
    }
}
