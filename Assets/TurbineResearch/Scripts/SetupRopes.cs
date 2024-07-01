using System.Collections;
using System.Collections.Generic;
using Obi;
using TurbineResearch.Scripts;
using UnityEngine;

public class SetupRopes : MonoBehaviour
{
    private CSVReader csvReader;
    private List<List<float>> table;
    
    
    
    void Start()
    {
        csvReader = new CSVReader();
        table = csvReader.ReadCSVFile(
            "C:\\Users\\IMRE Lab\\Wind_Energy\\Assets\\TurbineResearch\\CSV Files\\Output1.csv", 2);
        CreateRope(); 
    }

    IEnumerator SetupBlueprint(ObiRopeBlueprint blueprint)
    {
        // Procedurally generate the rope path (a simple straight line):
        int filter = ObiUtils.MakeFilter(ObiUtils.CollideWithEverything, 0);
        blueprint.path.Clear();
        for (int i = 0; i < 6; i += 3)
        {
            blueprint.path.AddControlPoint(new Vector3(table[0][i],table[0][i+2],table[0][i+1]), -Vector3.right, 
                Vector3.right, Vector3.up, 0.1f, 0.1f, 1, filter, Color.white, "control");
        }
        blueprint.path.FlushEvents();

        // generate the particle representation of the rope (wait until it has finished):
        yield return StartCoroutine(blueprint.Generate());
    }

    ObiSolver CreateSolver()
    {
        // create an object containing both the solver and the updater:
        GameObject solverObject = new GameObject("solver", typeof(ObiSolver), typeof(ObiFixedUpdater));
        ObiSolver solver = solverObject.GetComponent<ObiSolver>();
        ObiFixedUpdater updater = solverObject.GetComponent<ObiFixedUpdater>();

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

        ObiRopeBlueprint blueprint = ScriptableObject.CreateInstance<ObiRopeBlueprint>();

        IEnumerator setupBlueprint = SetupBlueprint(blueprint);

        while (setupBlueprint.MoveNext()) {}
        
        // instantiate and set the blueprint:
        rope.ropeBlueprint = ScriptableObject.Instantiate(blueprint);

        ObiSolver solver = CreateSolver();
        
        // parent the cloth under a solver to start simulation:
        rope.transform.parent = solver.transform;
    }

    void AddConstraints(ObiRope rope)
    {
        // get a hold of the constraint type we want, in this case, pin constraints:
        var pinConstraints = rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;

        // remove all batches from it, so we start clean:
        pinConstraints.Clear();

        // create a new pin constraints batch
        var batch = new ObiPinConstraintsBatch();

        // Add a couple constraints to it, pinning the first and last particles in the rope:
        // batch.AddConstraint(rope.solverIndices[0], colliderA, Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);
        // batch.AddConstraint(rope.solverIndices[blueprint.activeParticleCount - 1], colliderB, Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);

        // set the amount of active constraints in the batch to 2 (the ones we just added).
        batch.activeConstraintCount = 2;

        // append the batch to the pin constraints:
        pinConstraints.AddBatch(batch);

        // this will cause the solver to rebuild pin constraints at the beginning of the next frame:
        rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
    }
}
