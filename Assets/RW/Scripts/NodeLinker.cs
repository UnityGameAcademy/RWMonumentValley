using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace RW.MonumentValley
{
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
                // check difference between desired and current angle
                Quaternion targetAngle = Quaternion.Euler(l.activeEulerAngle);
                float angleDiff = Quaternion.Angle(l.xform.rotation, targetAngle);

                if (Mathf.Abs(angleDiff) < 0.01f)
                {
                    EnableLink(l.nodeA, l.nodeB, true);
                }
                else
                {
                    EnableLink(l.nodeA, l.nodeB, false);
                }
            }
        }

        // toggle active state between Neighbor nodes
        public void EnableLink(Node nodeA, Node nodeB, bool state)
        {
            if (nodeA == null || nodeB == null)
                return;

            nodeA.EnableEdge(nodeB, state);
            nodeB.EnableEdge(nodeA, state);
        }
    }

    // class to activate/deactivate special edges between Nodes based on rotation
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
}