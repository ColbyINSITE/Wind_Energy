using System;
using System.Collections;
using System.Collections.Generic;
using Obi;
using TurbineResearch.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class RopeCreator : MonoBehaviour
{
    private TextReader _txtReader;
    public List<List<float>> Table;
    private int _nodeCount = 0;

    public GameObject solver;
    public List<GameObject> linkPrefabs;
    public TextAsset file; 
    public Material ropeMat;
    public int skipLines;
    public bool isChain;
    public float thicknessScale;
    public bool skipFirstColumn;
    public int animationFrameLimit;
    public float dynamicAttachmentCompliance;

    public GameObject turbine;
    
    void Awake()
    {
        _txtReader = new TextReader();
        Table = _txtReader.ReadCSVFile(
            Application.dataPath + "\\Resources\\" + file.name + ".txt", skipLines, ' ', animationFrameLimit);
        SetupRope(); 
    }

    void SetupBlueprint(ObiRodBlueprint blueprint)
    {
        int buffer = skipFirstColumn ? 1 : 0;
        
        // Procedurally generate the rope path (a simple straight line):
        int filter = ObiUtils.MakeFilter(ObiUtils.CollideWithEverything, 0);
        blueprint.path.Clear();

        for (int i = buffer; i < Table[0].Count; i += 3)
        {
            _nodeCount += 1;
            Vector3 position = new Vector3(Table[0][i], Table[0][i + 2], Table[0][i + 1]);
            blueprint.path.AddControlPoint(position, Vector3.zero, 
                Vector3.zero, Vector3.up, 0.1f, 0.1f, 1, filter, Color.white, "control" + _nodeCount);
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

        ropeRenderer.thicknessScale = thicknessScale;

        // load the default rope section:
        ropeRenderer.section = Resources.Load<ObiRopeSection>("DefaultRopeSection");
        
        // set rope material
        ropeObject.GetComponent<MeshRenderer>().material = ropeMat;

        return ropeObject;
    }

    void SetupRope()
    {
        GameObject ropeObject;

        ropeObject = isChain ? CreateChain() : CreateRope();

        ObiRod rope = ropeObject.GetComponent<ObiRod>();
        
        // create a blueprint 
        ObiRodBlueprint blueprint = ScriptableObject.CreateInstance<ObiRodBlueprint>();
        blueprint.resolution = 0.1f;
        //blueprint.keepInitialShape = false;

        SetupBlueprint(blueprint);
        IEnumerator bpSetup = blueprint.Generate();

        while (bpSetup.MoveNext()) {}
        
        // instantiate and set the blueprint:
        rope.rodBlueprint = Instantiate(blueprint);
        
        // parent the cloth under a solver to start simulation:
        rope.transform.parent = solver.transform;
        
        int buffer = skipFirstColumn ? 1 : 0;
        
        int groupIndex = -1;
        for (int i = buffer; i < Table[0].Count; i += 3)
        {
            groupIndex += 1;
            Vector3 position = new Vector3(Table[0][i], Table[0][i + 2], Table[0][i + 1]);
            AddStaticAttachment(ropeObject, CreateEmptyGameObject( position, name + " - Node " + i/3), 
                rope.blueprint.groups[groupIndex]);
        }

        if (!isChain)
            AddDynamicAttachment(ropeObject, turbine.transform, rope.blueprint.groups[groupIndex]);


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
    
    void AddStaticAttachment(GameObject actor, Transform targetTransform, ObiParticleGroup particleGroup)
    {
        var attachment = actor.AddComponent<ObiParticleAttachment>();
        attachment.target = targetTransform;
        attachment.particleGroup = particleGroup;
    }
    
    void AddDynamicAttachment(GameObject actor, Transform targetTransform, ObiParticleGroup particleGroup)
    {
        var attachment = actor.AddComponent<ObiParticleAttachment>();
        attachment.target = targetTransform;
        attachment.particleGroup = particleGroup;
        attachment.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
        attachment.compliance = dynamicAttachmentCompliance;
    }

    public int GetNodeCount()
    {
        return _nodeCount;
    }

    public List<List<float>> GetTable()
    {
        return Table;
    }
}
