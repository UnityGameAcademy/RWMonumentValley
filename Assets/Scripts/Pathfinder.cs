using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Graph))]
public class Pathfinder : MonoBehaviour
{
    // start and end Nodes
    [SerializeField] private Node startNode;
    [SerializeField] private Node goalNode;

    // next Nodes to explore
    private List<Node> frontierNodes;

    // Nodes already explored
    private List<Node> exploredNodes;

    // Nodes that form a path to the goal Node
    private List<Node> pathNodes;

    // have we completed the search
    private bool isComplete;
    private bool hasFoundGoal;

    // structure containing all Nodes
    private Graph graph;

    public Node StartNode { get { return startNode; } set { startNode = value; } }
    public Node GoalNode { get { return goalNode; } set { goalNode = value; } }

    private void Awake()
    {
        graph = GetComponent<Graph>();
    }

    private void Start()
    {
        FindPath();
    }

    private void InitGraph()
    {
        if (graph == null || startNode == null || goalNode == null)
        {
            return;
        }

        frontierNodes = new List<Node>();
        frontierNodes.Add(startNode);
        exploredNodes = new List<Node>();
        pathNodes = new List<Node>();

        graph.ResetNodes();
        isComplete = false;
        hasFoundGoal = false;
    }



    // use a simple Breadth-first Search to explore the graph
    private void ExpandFrontier(Node node)
    {
        // validate Node
        if (node == null )
        {
            return;
        }

        // loop through all Edges
        for (int i = 0; i < node.Edges.Count; i++)
        {
            // skip if already explored
            if ( node.Edges[i] == null ||
                node.Edges.Count == 0 ||
                exploredNodes.Contains(node.Edges[i].neighbor) ||
                frontierNodes.Contains(node.Edges[i].neighbor))
            {
                continue;
            }

            // set the neighbor's previous node to this node
            node.Edges[i].neighbor.PreviousNode = node;

            // add neighbor Nodes to frontier Nodes
            frontierNodes.Add(node.Edges[i].neighbor);

        }
    }

    // given a goal node, follow PreviousNode breadcrumbs to create a path
    private List<Node> GetPathNodes()
    {
        // create a new list of Nodes
        List<Node> path = new List<Node>();

        // start with the goal Node
        if (goalNode == null)
        {
            return path;
        }
        path.Add(goalNode);

        // follow the breadcrumb trail 
        Node currentNode = goalNode.PreviousNode;

        while (currentNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.PreviousNode;
        }
        return path;
    }

    public void FindPath()
    {
        Debug.Log("FIND PATH STARTED....");
        if (startNode == null || goalNode == null)
        {
            Debug.Log("Missing Start or Goal node =============");
            return;
        }
        const int maxIterations = 100;
        int iterations = 0;

        InitGraph();

        // search the graph until we find the goal or explore all nodes
        while (!isComplete && frontierNodes != null && iterations < maxIterations)
        {
            iterations++;

            // if we still have frontier Nodes to check
            if (frontierNodes.Count > 0)
            {

                Node currentNode = frontierNodes[0];
                frontierNodes.RemoveAt(0);

                if (!exploredNodes.Contains(currentNode))
                {
                    exploredNodes.Add(currentNode);
                }
                ExpandFrontier(currentNode);

                // if we have found the goal
                if (frontierNodes.Contains(goalNode))
                {
                    pathNodes = GetPathNodes();
                    isComplete = true;
                    hasFoundGoal = true;
                    Debug.Log("Found goal:  " + goalNode.name);
                }
            }

            // whole graph explored but no path found
            else
            {
                isComplete = true;
            }
        }
        Debug.Log("FIND PATH COMPLETE.... iterations:" + iterations);

    }

    private void OnDrawGizmos()
    {
        
    }
}
