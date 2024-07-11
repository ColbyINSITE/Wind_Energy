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

    public List<GameObject> linkPrefabs;
    public TextAsset csvFile; 
    public Material ropeMat;
    public int skipLines;
    public bool isChain;
    
    void Awake()
    {
        csvReader = new CSVReader();
        table = csvReader.ReadCSVFile(Application.dataPath + "\\TurbineResearch\\CSV Files\\" + csvFile.name +".csv", skipLines);
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

    ObiSolver CreateSolver()
    {
        // create an object containing both the solver and the updater:
        GameObject solverObject = new GameObject("solver", typeof(ObiSolver), typeof(ObiFixedUpdater));
        ObiSolver solver = solverObject.GetComponent<ObiSolver>();
        ObiFixedUpdater updater = solverObject.GetComponent<ObiFixedUpdater>();
        updater.substeps = 30;

        // add the solver to the updater:
        updater.solvers.Add(solver);
        
        solver.gravity = Vector3.zero;

        return solver;
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

        chainRenderer.sectionTwist = 90;

        return ropeObject;
    }

    GameObject CreateRope()
    {
        // create a rope:
        GameObject ropeObject = new GameObject("Rope", typeof(ObiRod), typeof(ObiRopeExtrudedRenderer));

        // get component reference:
        ObiRopeExtrudedRenderer ropeRenderer = ropeObject.GetComponent<ObiRopeExtrudedRenderer>();

        // load the default rope section:
        ropeRenderer.section = Resources.Load<ObiRopeSection>("DefaultRopeSection");
        
        // set rope material
        ropeObject.GetComponent<MeshRenderer>().material = ropeMat;

        return ropeObject;
    }

    void SetupRope()
    {
        GameObject ropeObject;

        if (isChain)
            ropeObject = CreateChain();
        else
            ropeObject = CreateRope();
        
        ObiRod rope = ropeObject.GetComponent<ObiRod>();
        
        // create a blueprint 
        ObiRodBlueprint blueprint = ScriptableObject.CreateInstance<ObiRodBlueprint>();
        blueprint.resolution = 0.5f;

        SetupBlueprint(blueprint);
        IEnumerator bpSetup = blueprint.Generate();

        while (bpSetup.MoveNext()) {}
        
        // instantiate and set the blueprint:
        rope.rodBlueprint = ScriptableObject.Instantiate(blueprint);

        ObiSolver solver = CreateSolver();
        
        // parent the cloth under a solver to start simulation:
        rope.transform.parent = solver.transform;

        int groupIndex = 0;
        for (int i = 0; i < table[0].Count; i += 3)
        {
            Vector3 position = new Vector3(table[0][i], table[0][i + 2], table[0][i + 1]);
            AddAttachment(ropeObject, CreateEmptyGameObject( position, name + " - Node " + i/3), 
                rope.blueprint.groups[groupIndex]);
            groupIndex += 1;
        }
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
