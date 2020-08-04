using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [Space(20)] [Tooltip("Invoked when Player enters this Node")]
    // invoked when Player enters this node
    public UnityEvent playerEvent;

    private Graph graph;
    private Node previousNode;

    // 3d compass directions to check for horizontal neighbors automatically
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
        if (graph == null)
        {
            Debug.Log(gameObject.name + " NODE FindNeighbors: missing graph!");
            return;
        }

        // search through possible neighbor offsets
        foreach (Vector3 direction in neighborDirections)
        {
            Node newNode = graph.FindNodeAt(transform.position + direction);

            // add to edges list if not already included and not excluded specifically
            if (newNode != null && !HasNeighbor(newNode) && !excludedNodes.Contains(newNode))
            {
                Edge newEdge = new Edge { neighbor = newNode, isActive = true };
                edges.Add(newEdge);
            }
        }
    }

    // is a Node currently in edges List?
    private bool HasNeighbor(Node node)
    {
        foreach (Edge e in edges)
        {
            if (e.neighbor.Equals(node))
            {
                return true;
            }
        }
        return false;
    }

    // set state if an Edge exists between a target Node
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

// connection/link to neighboring node
[System.Serializable]
public class Edge
{
    public Node neighbor;
    public bool isActive;
}

