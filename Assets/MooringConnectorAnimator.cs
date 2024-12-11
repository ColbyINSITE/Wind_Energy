using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MooringConnectorAnimator : MonoBehaviour
{
    public GameObject connectorPart;
    public GameObject rope;
    public GameObject chain;
    private RopeCreator _ropeRc;
    private RopeCreator _chainRc;
    private GameObject _ropeNode;
    private GameObject _chainNode;
    
    // Start is called before the first frame update
    void Start()
    {
        if (rope == null || chain == null)
        {
            throw new Exception("You need a rope and a chain for this!");
        }

        connectorPart = Instantiate(connectorPart);

        _ropeRc = rope.GetComponent<RopeCreator>();
        _chainRc = chain.GetComponent<RopeCreator>();
        
        // get the first node on the rope and the last node on the chain to create the connection
        _ropeNode = _ropeRc.GetNode(1);
        _chainNode = _chainRc.GetNode(_chainRc.GetNodeCount() - 2);
    }

    private void Update()
    {
        SetConnectorPosition();
        SetConnectorRotation();
    }

    void SetConnectorRotation()
    {
        connectorPart.transform.forward = _chainNode.transform.position - _ropeNode.transform.position;
        connectorPart.transform.rotation *= Quaternion.Euler(-90, 0, -30);
    }
    
    void SetConnectorPosition()
    {
        connectorPart.transform.position = _ropeRc.GetNode(0).transform.position;
    }
    
}
