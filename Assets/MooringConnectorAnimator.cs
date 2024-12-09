using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MooringConnectorAnimator : MonoBehaviour
{
    public List<GameObject> ropes;
    public List<GameObject> chains;
    private RopeCreator rc;
    
    // Start is called before the first frame update
    void Start()
    {
        rc = GetComponent<RopeCreator>();
        
        if (ropes.Count != chains.Count)
        {
            throw new Exception("Size of rope and chain lists don't match!");
        }

        for (int i = 0; i < ropes.Count; i++)
        {
            Vector3 direction = FindConnectorDirection(ropes[i], chains[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 FindConnectorDirection(GameObject rope, GameObject chain)
    {
        
    }
}
