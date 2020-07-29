using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// management class for all Nodes
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

    // locate the closest Node at a target position 
    public Node FindClosestNode(Node[] nodes, Vector3 pos)
    {
        Node closestNode = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Node n in nodes)
        {
            Vector3 diff = n.transform.position - pos;

            if (diff.sqrMagnitude < closestDistanceSqr)
            {
                closestNode = n;
                closestDistanceSqr = diff.sqrMagnitude;
            }
        }
        return closestNode;
    }

    public void ResetNodes()
    {
        foreach (Node node in allNodes)
        {
            node.PreviousNode = null;
        }
    }
}
