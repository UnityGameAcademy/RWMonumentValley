using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// activates or deactivates special edges
public class NodeLinker : MonoBehaviour
{
    [SerializeField] public Link[] links;

    // enable/disable links based on transform's euler angles
    public void UpdateLinks()
    {
        foreach (Link l in links)
        {
            Vector3  diff  = l.xform.eulerAngles - l.activeEulerAngle;
            if (diff.sqrMagnitude < 0.01f)
            {
                l.nodeA.EnableEdge(l.nodeB, true);
                l.nodeB.EnableEdge(l.nodeA, true);
            }
            else
            {
                l.nodeA.EnableEdge(l.nodeB, false);
                l.nodeB.EnableEdge(l.nodeA, false);
            }
        }
    }
}

// class to activate/deactivate special edges between Nodes
[System.Serializable]
public class Link
{
    public Node nodeA;
    public Node nodeB;

    // transform to check
    public Transform xform;

    // euler angle needed to activate link
    public Vector3 activeEulerAngle;


}