using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Graph : MonoBehaviour
{
    // all of the Nodes in the current level/maze
    private List<Node> allNodes = new List<Node>();

    // 
    private void Awake()
    {
        allNodes = FindObjectsOfType<Node>().ToList();
    }

    // locate the specific Node at target position within rounding error
    public Node FindNodeAt(Vector3 pos)
    {
        foreach (Node n in allNodes)
        {
            Vector3 diff = n.transform.position - pos;

            if (diff.sqrMagnitude < 0.01f)
            {
                return n;
            }
        }
        return null;
    }

    public void ResetNodes()
    {
        foreach (Node node in allNodes)
        {
            node.PreviousNode = null;
        }
    }
}
