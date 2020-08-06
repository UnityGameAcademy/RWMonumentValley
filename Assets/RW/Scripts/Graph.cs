﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RW.MonumentValley
{
    // management class for all Nodes
    public class Graph : MonoBehaviour
    {
        // all of the Nodes in the current level/maze
        private List<Node> allNodes = new List<Node>();

        // end of level
        [SerializeField] private Node goalNode;
        public Node GoalNode => goalNode;

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

        // locate the closest Node in a given array (screen Mode: true = screen space, false = world space)
        public Node FindClosestNode(Node[] nodes, Vector3 pos, bool screenMode)
        {
            Node closestNode = null;
            float closestDistanceSqr = Mathf.Infinity;

            foreach (Node n in nodes)
            {
                Vector3 diff = n.transform.position - pos;

                if (screenMode)
                {
                    Vector3 nodeScreenPosition = Camera.main.WorldToScreenPoint(n.transform.position);
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(pos);
                    diff = nodeScreenPosition - screenPosition;
                }

                if (diff.sqrMagnitude < closestDistanceSqr)
                {
                    closestNode = n;
                    closestDistanceSqr = diff.sqrMagnitude;
                }
            }
            return closestNode;
        }

        // find the closest Node in the entire Graph
        public Node FindClosestNode(Vector3 pos, bool screenMode = false)
        {
            return FindClosestNode(allNodes.ToArray(), pos, screenMode);
        }

        // clear breadcrumb trail
        public void ResetNodes()
        {
            foreach (Node node in allNodes)
            {
                node.PreviousNode = null;
            }
        }
    }

}