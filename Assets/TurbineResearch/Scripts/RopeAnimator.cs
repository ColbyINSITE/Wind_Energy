using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAnimator : MonoBehaviour
{
    public List<RopeCreator> RopeCreators;
    
    private void Start()
    {
        foreach (RopeCreator rp in RopeCreators)
        {
            AnimateRopes(rp);
        }
    }

    private void AnimateRopes(RopeCreator rp)
    {
        for (int i = 0; i < rp.GetNodeCount(); i++)
        {
            Transform node = rp.transform.GetChild(i);
            StartCoroutine(LoopNodes(rp, node, i));
        }
    }

    IEnumerator LoopNodes(RopeCreator rp, Transform node, int nodeIdx)
    {
        for (int i = 1; i < 1200; i++)
        {
            IEnumerator lerpNode = LerpNode(rp, node, nodeIdx, i);
            while (lerpNode.MoveNext())
            {
                yield return null;
            }
        }
    }

    IEnumerator LerpNode(RopeCreator rp, Transform node, int nodeIdx, int frame)
    {
        List<List<float>> table = rp.GetTable();
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
}
