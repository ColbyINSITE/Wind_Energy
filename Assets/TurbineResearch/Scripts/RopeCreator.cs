using System;
using System.Collections;
using System.Collections.Generic;
using Obi;
using TurbineResearch.Scripts;
using UnityEngine;

public class RopeCreator : MonoBehaviour
{
    private CSVReader csvReader;
    private List<List<float>> table;
    private int nodeCount = 0;

    public GameObject _solver;
    public List<GameObject> linkPrefabs;
    public TextAsset csvFile; 
    public Material ropeMat;
    public int _skipLines;
    public bool _isChain;
    public float _thicknessScale;
    
    void Awake()
    {
        csvReader = new CSVReader();
        table = csvReader.ReadCSVFile(Application.dataPath + "\\TurbineResearch\\CSV Files\\" + csvFile.name +".csv", _skipLines);
        SetupRope(); 
    }

    void SetupBlueprint(ObiRodBlueprint blueprint)
    {
        // Procedurally generate the rope path (a simple straight line):
        int filter = ObiUtils.MakeFilter(ObiUtils.CollideWithEverything, 0);
        blueprint.path.Clear();

        for (int i = 0; i < table[0].Count; i += 3)
        {
            nodeCount += 1;
            Vector3 position = new Vector3(table[0][i], table[0][i + 2], table[0][i + 1]);
            blueprint.path.AddControlPoint(position, Vector3.zero, 
                Vector3.zero, Vector3.up, 0.1f, 0.1f, 1, filter, Color.white, "control" + nodeCount);
        }
        blueprint.path.FlushEvents();
    }

    GameObject CreateChain()
    {
        // create a rope:
        GameObject ropeObject = new GameObject("Chain", typeof(ObiRod), typeof(ObiRopeChainRenderer));

        // get component reference:
        ObiRopeChainRenderer chainRenderer = ropeObject.GetComponent<ObiRopeChainRenderer>();

        // load the default rope section:
        foreach (GameObject linkPrefab in linkPrefabs)
        {
            chainRenderer.linkPrefabs.Add(linkPrefab);
        }
        
        // TODO: Don't hardcode this
        chainRenderer.linkScale = new Vector3(170, 170, 170);
        chainRenderer.sectionTwist = 90;

        return ropeObject;
    }

    GameObject CreateRope()
    {
        // create a rope:
        GameObject ropeObject = new GameObject("Rope", typeof(ObiRod), typeof(ObiRopeExtrudedRenderer));

        // get component reference:
        ObiRopeExtrudedRenderer ropeRenderer = ropeObject.GetComponent<ObiRopeExtrudedRenderer>();

        ropeRenderer.thicknessScale = _thicknessScale;

        // load the default rope section:
        ropeRenderer.section = Resources.Load<ObiRopeSection>("DefaultRopeSection");
        
        // set rope material
        ropeObject.GetComponent<MeshRenderer>().material = ropeMat;

        return ropeObject;
    }

    void SetupRope()
    {
        GameObject ropeObject;

        if (_isChain)
            ropeObject = CreateChain();
        else
            ropeObject = CreateRope();
        
        ObiRod rope = ropeObject.GetComponent<ObiRod>();
        
        // create a blueprint 
        ObiRodBlueprint blueprint = ScriptableObject.CreateInstance<ObiRodBlueprint>();
        blueprint.resolution = 0.1f;
        //blueprint.keepInitialShape = false;

        SetupBlueprint(blueprint);
        IEnumerator bpSetup = blueprint.Generate();

        while (bpSetup.MoveNext()) {}
        
        
        // instantiate and set the blueprint:
        rope.rodBlueprint = ScriptableObject.Instantiate(blueprint);
        
        // parent the cloth under a solver to start simulation:
        rope.transform.parent = _solver.transform;

        int groupIndex = 0;
        for (int i = 0; i < table[0].Count; i += 3)
        {
            Vector3 position = new Vector3(table[0][i], table[0][i + 2], table[0][i + 1]);
            AddAttachment(ropeObject, CreateEmptyGameObject( position, name + " - Node " + i/3), 
                rope.blueprint.groups[groupIndex]);
            groupIndex += 1;
        }
        
        /*Vector3 position1 = new Vector3(table[0][0], table[0][2], table[0][1]);
        AddAttachment(ropeObject, CreateEmptyGameObject( position1, name + " - Node " + 0),
            rope.blueprint.groups[0]);
        Vector3 position2 = new Vector3(table[0][table[0].Count-3], table[0][table[0].Count-1], table[0][table[0].Count-2]);
        AddAttachment(ropeObject, CreateEmptyGameObject( position2, name + " - Node " + 1),
            rope.blueprint.groups[(table[0].Count/3)-3]);*/
        
    }

    Transform CreateEmptyGameObject(Vector3 position, string name)
    {
        GameObject go = new GameObject(name);
        go.transform.position = position;
        go.transform.parent = transform;
        return go.transform;
    }
    
    void AddAttachment(GameObject actor, Transform targetTransform, ObiParticleGroup particleGroup)
    {
        var attachment = actor.AddComponent<ObiParticleAttachment>();
        attachment.target = targetTransform;
        attachment.particleGroup = particleGroup;
    }

    public int GetNodeCount()
    {
        return nodeCount;
    }

    public List<List<float>> GetTable()
    {
        return table;
    }
}
