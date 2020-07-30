using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles Player input and movement
public class PlayerController : MonoBehaviour
{
    // visualization indicating where clicked
    [SerializeField] GameObject cursor;
    [SerializeField] float moveSpeed;

    // pathfinding fields
    private Clickable[] clickables;
    private Pathfinder pathfinder;
    private Graph graph;
    private List<Node> currentPath;
    private Node currentNode;

    // movement fields
    private bool isMoving;

    private void Awake()
    {
        // listen to each clickable's clickEvent
        clickables = FindObjectsOfType<Clickable>();
        pathfinder = FindObjectOfType<Pathfinder>();

        foreach (Clickable c in clickables)
        {
            c.clickAction += OnClick;
        }

        if (pathfinder != null)
        {
            graph = pathfinder.GetComponent<Graph>();
        }
    }

    private void Start()
    {
        pathfinder?.SetStartNode(transform.position);

    }

    private void OnDisable()
    {
        // unsubscribe to each clickable's clickEvent
        foreach (Clickable c in clickables)
        {
            c.clickAction -= OnClick;
        }
    }

    private void OnClick(Node clickedNode)
    {
        //Debug.Log("PLAYERCONTROLLER OnClick: click at" + clickedNode.transform.position.ToString());

        pathfinder.FindPath(clickedNode);
        FollowPath();

    }

    public void FollowPath()
    {
        StartCoroutine(FollowPathRoutine());
    }

    private IEnumerator FollowPathRoutine()
    {

        yield return null;
    }

    private IEnumerator MoveToPositionRoutine(Vector3 targetPosition)
    {
        yield return null;
    }

    public void MoveToNode(Node node)
    {
        
    }

    public void MoveToNearestNode()
    {
        Node nearestNode = graph.FindClosestNode(transform.position);
        MoveToNode(nearestNode);
    }

    public bool HasReachedGoal()
    {
        if (pathfinder == null || graph == null || pathfinder.GoalNode == null)
            return false;

        float distanceSqr = (pathfinder.GoalNode.transform.position - transform.position).sqrMagnitude;

        return (distanceSqr < 0.01f);
    }
}
