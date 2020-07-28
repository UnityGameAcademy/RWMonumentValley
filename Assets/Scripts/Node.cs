using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    #region INSPECTOR
    [SerializeField] private float gizmoRadius = 0.1f;

    [SerializeField] private Color defaultGizmoColor = Color.black;
    [SerializeField] private Color selectedGizmoColor = Color.blue;

    // connected neighboring nodes
    [SerializeField] private List<Edge> edges = new List<Edge>();
    #endregion

    #region PRIVATE
    private Graph graph;
    private Node previousNode;
    #endregion

    #region STATIC
    // 3d compass directions to check for adjacent/neighboring Nodes
    public static Vector3[] neighborDirections =
{
        // horizontal neighbors
        new Vector3(1f, 0f, 0f),
        new Vector3(-1f, 0f, 0f),
        new Vector3(0f, 0f, 1f),
        new Vector3(0f, 0f, -1f),

        //vertical neighbors


        // ramp/stairs neighbors
        new Vector3(0.5f, 0.5f, 0f),
        new Vector3(-0.5f, 0.5f, 0f),
        new Vector3(0f, 0.5f, 0.5f),
        new Vector3(0f,0.5f, -0.5f)

    };
    #endregion

    #region PROPERTIES
    public Node PreviousNode { get { return previousNode; } set { previousNode = value; } }
    public List<Edge> Edges => edges;
    #endregion

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

            // add to edges list if not already included
            if (newNode != null && !HasNeighbor(newNode))
            {
                Edge newEdge = new Edge { neighbor = newNode, isActive = true};
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
    public void EnableEdge(Node targetNode, bool state)
    {

    }


}

// connection/link to neighboring node
[System.Serializable]
public class Edge
{
    public Node neighbor;
    public bool isActive;
}

