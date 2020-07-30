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

    // is the search complete?
    private bool isComplete;

    // has the goal been found?
    private bool hasFoundGoal;

    // structure containing all Nodes
    private Graph graph;

    public Node StartNode { get { return startNode; } set { startNode = value; } }
    public Node GoalNode { get { return goalNode; } set { goalNode = value; } }
    public List<Node> PathNodes => pathNodes;


    [SerializeField] private Color pathColor = Color.green;

    private void Awake()
    {
        graph = GetComponent<Graph>();
    }

    // initialize all Nodes/lists
    private void InitGraph()
    {
        if (graph == null || startNode == null || goalNode == null)
        {
            return;
        }

        frontierNodes = new List<Node>();
        exploredNodes = new List<Node>();
        pathNodes = new List<Node>();

        graph.ResetNodes();

        isComplete = false;
        hasFoundGoal = false;

        // first Node
        frontierNodes.Add(startNode);
    }

    // use a simple Breadth-first Search to explore one iteration
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
            // skip Edge if neighbor already explored or invalid
            if ( node.Edges[i] == null ||
                node.Edges.Count == 0 ||
                exploredNodes.Contains(node.Edges[i].neighbor) ||
                frontierNodes.Contains(node.Edges[i].neighbor))
            {
                continue;
            }

            // create PreviousNode breadcrumb trail if Edge is active
            if (node.Edges[i].isActive)
            {
                // set the neighbor's previous node to this node
                node.Edges[i].neighbor.PreviousNode = node;

                // add neighbor Nodes to frontier Nodes
                frontierNodes.Add(node.Edges[i].neighbor);
            }
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

        // follow the breadcrumb trail, creating a path until it ends
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

        if (startNode == null || goalNode == null)
        {
            Debug.Log("Missing Start or Goal node =============");
            return;
        }

        // special case if goalNode is the same as the startNode
        if (startNode == goalNode)
        {
            List<Node> pathOfOne = new List<Node>();
            pathOfOne.Add(goalNode);
            pathNodes = pathOfOne;
            isComplete = true;
            Debug.Log("FIND PATH COMPLETE WITH ONE-NODE....");
            return;
        }

        // safety prevents infinite loop
        const int maxIterations = 100;
        int iterations = 0;

        // initialize all Nodes
        InitGraph();

        // search the graph until goal is found or all nodes explored (or exceeding some limit)
        while (!isComplete && frontierNodes != null && iterations < maxIterations)
        {
            iterations++;

            // if we still have frontier Nodes to check
            if (frontierNodes.Count > 0)
            {
                // remove the first Node
                Node currentNode = frontierNodes[0];
                frontierNodes.RemoveAt(0);

                // and add to the exploredNodes
                if (!exploredNodes.Contains(currentNode))
                {
                    exploredNodes.Add(currentNode);
                }

                // add unexplored neighboring Nodes to frontier
                ExpandFrontier(currentNode);

                // if we have found the goal
                if (frontierNodes.Contains(goalNode))
                {
                    // generate the Path to the goal
                    pathNodes = GetPathNodes();
                    isComplete = true;
                    hasFoundGoal = true;
                    Debug.Log("FIND PATH COMPLETE WITH GOAL....");
                }
            }
            // if whole graph explored but no path found
            else
            {
                isComplete = true;
                Debug.Log("FIND PATH COMPLETE NO GOAL FOUND....");
            }
        }

        //float timeElapsed = Time.realtimeSinceStartup - timeStarted;
    }

    public void FindPath(Node goal)
    {
        goalNode = goal;
        FindPath();
    }

    private void OnDrawGizmosSelected()
    {
        if (isComplete)
        {
            foreach (Node node in pathNodes)
            {
                
                if (node == goalNode || node == startNode)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(node.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
                }
                else
                {
                    Gizmos.color = pathColor;
                    Gizmos.DrawWireSphere(node.transform.position, 0.15f);
                }

                Gizmos.color = Color.red;
                if (node.PreviousNode != null)
                {
                    Gizmos.DrawLine(node.transform.position, node.PreviousNode.transform.position);
                }
            }
        }
    }

    public void SetStartNode(Vector3 position)
    {
        StartNode = graph.FindClosestNode(position);
    }
}
