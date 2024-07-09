using System.Collections;
using System.Collections.Generic;
using Obi;
using TurbineResearch.Scripts;
using UnityEngine;

public class SetupRopes : MonoBehaviour
{
    private CSVReader csvReader;
    private List<List<float>> table;

    public Material ropeMat;
    
    
    void Start()
    {
        csvReader = new CSVReader();
        table = csvReader.ReadCSVFile(
            "C:\\Users\\IMRE Lab\\Wind_Energy\\Assets\\TurbineResearch\\CSV Files\\Output1.csv", 2);
        CreateRope(); 
    }

    void SetupBlueprint(ObiRopeBlueprint blueprint)
    {
        // Procedurally generate the rope path (a simple straight line):
        int filter = ObiUtils.MakeFilter(ObiUtils.CollideWithEverything, 0);
        blueprint.path.Clear();
        int count = 1;
        for (int i = 0; i < table[0].Count; i += 3)
        {
            Vector3 position = new Vector3(table[0][i], table[0][i + 2], table[0][i + 1]);
            blueprint.path.AddControlPoint(position, Vector3.zero, 
                Vector3.zero, Vector3.up, 0.1f, 0.1f, 1, filter, Color.white, "control" + count);
            count++;
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
        GameObject ropeObject = new GameObject("rope", typeof(ObiRope), typeof(ObiRopeExtrudedRenderer));

        // get component references:
        ObiRope rope = ropeObject.GetComponent<ObiRope>();
        ObiRopeExtrudedRenderer ropeRenderer = ropeObject.GetComponent<ObiRopeExtrudedRenderer>();

        // load the default rope section:
        ropeRenderer.section = Resources.Load<ObiRopeSection>("DefaultRopeSection");
        
        // set rope material
        ropeObject.GetComponent<MeshRenderer>().material = ropeMat;
        
        // create a blueprint 
        ObiRopeBlueprint blueprint = ScriptableObject.CreateInstance<ObiRopeBlueprint>();

        SetupBlueprint(blueprint);
        IEnumerator bpSetup = blueprint.Generate();

        while (bpSetup.MoveNext()) {}
        
        // instantiate and set the blueprint:
        rope.ropeBlueprint = ScriptableObject.Instantiate(blueprint);

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
        return go.transform;
    }
    
    void AddAttachment(GameObject actor, Transform targetTransform, ObiParticleGroup particleGroup)
    {
        var attachment = actor.AddComponent<ObiParticleAttachment>();
        attachment.target = targetTransform;
        attachment.particleGroup = particleGroup;
    }
}
