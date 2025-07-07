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
        connectorPart.transform.up = _chainNode.transform.position - _ropeNode.transform.position;
        // I do this because the rotations on the prefab are messy. The fbx file rotates the object by (-90, 0, 30) and
        // that aligns the forward direction with z axis. So we do the same rotation after finding the forward
        // direction. We need to scrap this if we fix the import of the connector
        connectorPart.transform.rotation *= Quaternion.Euler(-90, 0, -30);
    }
    
    void SetConnectorPosition()
    {
        connectorPart.transform.position = _ropeRc.GetNode(0).transform.position;
    }
    
}
