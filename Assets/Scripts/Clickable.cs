using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// allows player to click on a block to set path goal
[RequireComponent(typeof(Collider))]
public class Clickable : MonoBehaviour
{

    private Node[] childNodes;
    private Graph graph;

    private bool isClicked;
    public bool IsClicked { get { return isClicked; } set { isClicked = value; } }

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
        if (graph == null || Camera.main == null)
        {
            return;
        }

        // raycast and find the path to the closest node
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            isClicked = true;
            Node clickedNode = graph.FindClosestNode(childNodes, hit.point);

            // trigger some clickable event
            if (clickAction != null)
            {
                clickAction.Invoke(clickedNode);
            }
        }
    }
}
