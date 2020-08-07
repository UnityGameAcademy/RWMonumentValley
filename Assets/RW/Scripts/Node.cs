
/*
 * Copyright (c) 2020 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RW.MonumentValley
{
    public class Node : MonoBehaviour
    {
        // gizmo colors
        [SerializeField] private float gizmoRadius = 0.1f;
        [SerializeField] private Color defaultGizmoColor = Color.black;
        [SerializeField] private Color selectedGizmoColor = Color.blue;
        [SerializeField] private Color inactiveGizmoColor = Color.gray;

        // neighboring nodes + active state
        [SerializeField] private List<Edge> edges = new List<Edge>();

        // Nodes specifically excluded from Edges
        [SerializeField] private List<Node> excludedNodes;

        [Space(20)]
        // invoked when Player enters this node
        public UnityEvent playerEvent;

        private Graph graph;
        private Node previousNode;

        // 3d compass directions to check for horizontal neighbors automatically (east/west/north/south)
        public static Vector3[] neighborDirections =
        {
            new Vector3(1f, 0f, 0f), 
            new Vector3(-1f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 0f, -1f),
        };

        public Node PreviousNode { get { return previousNode; } set { previousNode = value; } }
        public List<Edge> Edges => edges;


        private void Awake()
        {
            graph = Object.FindObjectOfType<Graph>();
        }

        private void Start()
        {
            if (graph != null)
            {
                FindNeighbors();
            }
        }

        private void OnDrawGizmos()
        {
            // draws a sphere gizmo
            Gizmos.color = defaultGizmoColor;
            Gizmos.DrawSphere(transform.position, gizmoRadius);
        }

        private void OnDrawGizmosSelected()
        {
            // draws a sphere gizmo
            Gizmos.color = selectedGizmoColor;
            Gizmos.DrawSphere(transform.position, gizmoRadius);

            // draws a line to each neighbor
            foreach (Edge e in edges)
            {
                if (e.neighbor != null)
                {
                    Gizmos.color = (e.isActive) ? selectedGizmoColor : inactiveGizmoColor;
                    Gizmos.DrawLine(transform.position, e.neighbor.transform.position);
                }
            }
        }

        // fill out edge connections to neighboring nodes automatically
        private void FindNeighbors()
        {
            // validate graph
            if (graph == null)
            {
                return;
            }

            // search through possible neighbor offsets
            foreach (Vector3 direction in neighborDirections)
            {
                Node newNode = graph.FindClosestNode(transform.position + direction);

                // add to edges list if not already included and not excluded specifically
                if (newNode != null && !HasNeighbor(newNode) && !excludedNodes.Contains(newNode))
                {
                    Edge newEdge = new Edge { neighbor = newNode, isActive = true };
                    edges.Add(newEdge);
                }
            }
        }

        // is a Node already in Edges List?
        private bool HasNeighbor(Node node)
        {
            foreach (Edge e in edges)
            {
                if (e.neighbor != null && e.neighbor.Equals(node))
                {
                    return true;
                }
            }
            return false;
        }

        // given a specific neighbor, sets active state
        public void EnableEdge(Node neighborNode, bool state)
        {
            foreach (Edge e in edges)
            {
                if (e.neighbor.Equals(neighborNode))
                {
                    e.isActive = state;
                }
            }
        }
    }

}