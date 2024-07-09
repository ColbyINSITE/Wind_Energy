using System;
using System.Collections;
using System.Collections.Generic;
using Obi;
using TurbineResearch.Scripts;
using UnityEngine;

public class SetupRopes : MonoBehaviour
{
    private CSVReader csvReader;
    private List<List<float>> table;
    private int nodeCount = 0;
    
    
    public List<TextAsset> csvFiles; 
    public Material ropeMat;
    
    
    void Awake()
    {
        string path = "C:\\Users\\IMRE Lab\\Wind_Energy\\Assets\\TurbineResearch\\CSV Files\\Output1.csv";
        csvFiles = new List<TextAsset>();
        csvReader = new CSVReader();
        table = csvReader.ReadCSVFile(Application.dataPath + "\\TurbineResearch\\CSV Files\\Output1.csv", 2);
        CreateRope(); 
    }

    private void Start()
    {
        AnimateRopes();
    }

    private void AnimateRopes()
    {
        for (int i = 0; i < nodeCount; i++)
        {
            Transform node = transform.GetChild(i);
            StartCoroutine(LoopNodes(node, i));
        }
    }

    IEnumerator LoopNodes(Transform node, int nodeIdx)
    {
        for (int i = 1; i < 1200; i++)
        {
            IEnumerator lerpNode = LerpNode(node, nodeIdx, i);
            while (lerpNode.MoveNext())
            {
                yield return null;
            }
        }
    }

    IEnumerator LerpNode(Transform node, int nodeIdx, int frame)
    {
        Vector3 startPosition = node.position;
        Vector3 endPosition = new Vector3(table[frame][3 * nodeIdx], table[frame][3 * nodeIdx + 2], table[frame][3 * nodeIdx + 1]);
        float timeStep = 0f;
        float lerpDuration = 0.5f;
        while (timeStep < lerpDuration)
        {
            node.position = Vector3.Lerp(startPosition, endPosition, timeStep/lerpDuration);
            timeStep += 0.005f;
            yield return null;
        }

        node.position = endPosition;

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
        updater.substeps = 8;

        // add the solver to the updater:
        updater.solvers.Add(solver);

        return solver;
    }

    void CreateRope()
    {
        // create a rope:
        GameObject ropeObject = new GameObject("rope", typeof(ObiRod), typeof(ObiRopeExtrudedRenderer));

        // get component references:
        ObiRod rope = ropeObject.GetComponent<ObiRod>();
        ObiRopeExtrudedRenderer ropeRenderer = ropeObject.GetComponent<ObiRopeExtrudedRenderer>();

        // load the default rope section:
        ropeRenderer.section = Resources.Load<ObiRopeSection>("DefaultRopeSection");
        
        // set rope material
        ropeObject.GetComponent<MeshRenderer>().material = ropeMat;
        
        // create a blueprint 
        ObiRodBlueprint blueprint = ScriptableObject.CreateInstance<ObiRodBlueprint>();
        blueprint.resolution = 0.01f;

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
            AddAttachment(ropeObject, CreateEmptyGameObject(position), rope.blueprint.groups[groupIndex]);
            groupIndex += 1;
        }
    }

    Transform CreateEmptyGameObject(Vector3 position)
    {
        GameObject go = new GameObject();
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
}
