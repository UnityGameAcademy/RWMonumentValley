using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// allows player to click on a block to set path goal
[RequireComponent(typeof(Collider))]
public class Clickable : MonoBehaviour
{

    private Node[] childNodes;

    private Pathfinder pathfinder;
    private Graph graph;

    private bool isClicked;
    public bool IsClicked => isClicked;

    private void Start()
    {
        childNodes = GetComponentsInChildren<Node>();
        pathfinder = FindObjectOfType<Pathfinder>();
        if (pathfinder)
        {
            graph = pathfinder.GetComponent<Graph>();
        }
    }

    private void OnMouseDown()
    {
        if (graph == null || Camera.main == null || pathfinder == null)
        {
            return;
        }

        // raycast and find the closest node to hit point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Node closestNode = graph.FindClosestNode(childNodes, hit.point);

            isClicked = true;
            pathfinder.FindPath(closestNode);
        }
    }
}
