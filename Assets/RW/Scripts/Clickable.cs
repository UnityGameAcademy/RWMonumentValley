using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RW.MonumentValley
{
    // allows player to click on a block to set path goal
    [RequireComponent(typeof(Collider))]
    public class Clickable : MonoBehaviour
    {
        // Nodes under this Transform
        private Node[] childNodes;

        // reference to Graph
        private Graph graph;

        // invoked when collider is clicked
        public Action<Node> clickAction;

        private void Start()
        {
            childNodes = GetComponentsInChildren<Node>();
            graph = FindObjectOfType<Graph>();
        }

        private void OnMouseDown()
        {
            // validate components
            if (graph == null)
            {
                return;
            }

            // raycast and find the path to the closest node
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                // find the closest Node in Screen space
                Node clickedNode = graph.FindClosestNode(childNodes, hit.point, true);

                // trigger event clickable event
                clickAction?.Invoke(clickedNode);

            }
        }
    }
}