using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

// activates or deactivates special Edges between Nodes
public class NodeLinker : MonoBehaviour
{
    [SerializeField] public Link[] links;

    private void Start()
    {
        UpdateLinks();

    }

    // enable/disable based on transform's euler angles
    public void UpdateLinks()
    {
        foreach (Link l in links)
        {

            Quaternion targetAngle = Quaternion.Euler(l.activeEulerAngle);
            float angleDiff = Quaternion.Angle(l.xform.rotation, targetAngle);

            if (Mathf.Abs(angleDiff) < 0.01f)
            {

                //Debug.Log("active Euler angle = " + l.activeEulerAngle);
                //Debug.Log("current Euler angle = " + l.xform.eulerAngles);
                //Debug.Log("diff =  " + angleDiff);

                l.nodeA.EnableEdge(l.nodeB, true);
                l.nodeB.EnableEdge(l.nodeA, true);
                Debug.Log("Connected " + l.nodeA + " with " + l.nodeB);
            }
            else
            {
                l.nodeA.EnableEdge(l.nodeB, false);
                l.nodeB.EnableEdge(l.nodeA, false);
            }
        }
    }

    public void EnableLink(Node nodeA, Node nodeB, bool state)
    {
        if (nodeA == null || nodeB == null)
            return;
        nodeA.EnableEdge(nodeB, state);
        nodeB.EnableEdge(nodeA, state);
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

    public UnityEvent linkEvent;

}