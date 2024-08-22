using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAnimator : MonoBehaviour
{
    public List<RopeCreator> RopeCreators;
    public float waitTime;
    public bool skipFirstColumn;
    public float frameDuration;
    
    private void Start()
    {
        StartCoroutine(StartAnimation(waitTime));
    }

    private void AnimateRopes(RopeCreator rp)
    {
        for (int i = 0; i < rp.GetNodeCount(); i++)
        {
            Transform node = rp.transform.GetChild(i);
            StartCoroutine(LoopNodes(rp, node, i, rp.animationFrameLimit));
        }
    }

    IEnumerator LoopNodes(RopeCreator rp, Transform node, int nodeIdx, int animationFrameLimit)
    {
        for (int i = 1; i < animationFrameLimit; i++)
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
        int buffer = 0;
        if (skipFirstColumn)
        {
            buffer = 1;
        }
        
        List<List<float>> table = rp.GetTable();
        Vector3 startPosition = node.position;
        Vector3 endPosition = new Vector3(table[frame][buffer + 3 * nodeIdx], table[frame][buffer + 3 * nodeIdx + 2], table[frame][buffer + 3 * nodeIdx + 1]);
        float timeStep = 0f;
        while (timeStep < frameDuration)
        {
            node.position = Vector3.Lerp(startPosition, endPosition, timeStep/frameDuration);
            timeStep += Time.deltaTime;
            yield return null;
        }

        node.position = endPosition;
    }

    IEnumerator StartAnimation(float waitTime)
    {
        float elapsedTime = 0;
        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        foreach (RopeCreator rp in RopeCreators)
        {
            AnimateRopes(rp);
        }
    }
}
