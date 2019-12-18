using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveTitleKangaroo : MonoBehaviour
{
    public GameObject removeObject;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(removeObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
