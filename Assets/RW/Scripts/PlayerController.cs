using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles Player input and movement
public class PlayerController : MonoBehaviour
{
    // visualization indicating where clicked
    [SerializeField] GameObject cursor;

    // time to move one unit
    [Range(0.25f, 5f)]
    [SerializeField] float moveTime = 0.25f;
    

    // pathfinding fields
    private Clickable[] clickables;
    private Pathfinder pathfinder;
    private Graph graph;

    private Node currentNode;
    private Node nextNode;
    private bool hasReachedDestination;
    


    // movement fields
    private bool isMoving;
    private bool canMove;

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
        canMove = true;
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
        if (!isMoving && canMove)
        {
            pathfinder.FindPath(clickedNode);
            FollowPath();
        }
    }

    public void FollowPath()
    {
        StartCoroutine(FollowPathRoutine());
    }

    private IEnumerator FollowPathRoutine()
    {
        isMoving = true;
        int i = 0;
        hasReachedDestination = false;

        while (i < pathfinder.PathNodes.Count - 1)
        {
            currentNode = pathfinder.PathNodes[i];
            nextNode = pathfinder.PathNodes[i + 1];

            yield return StartCoroutine(MoveToPositionRoutine(currentNode.transform.position, nextNode.transform.position));
            i++;
        }
        hasReachedDestination = true;
        isMoving = false;
    }

    private IEnumerator MoveToPositionRoutine(Vector3 startPosition, Vector3 targetPosition)
    {
        float t = 0;
        moveTime = Mathf.Clamp(moveTime, 0.1f, 5f);

        while (t < 1)
        {
            t += Time.deltaTime / moveTime ;
            transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.Clamp(t,0,1));

            // if over halfway, change parent to next node
            if (t > 0.51f)
            {
                transform.parent = nextNode.transform;
            }

            // wait one frame
            yield return null;
        }

        UpdatePlayerNode();

    }

    public void SnapToNearestNode()
    {
        Node nearestNode = graph?.FindClosestNode(transform.position, true);
        if (nearestNode != null)
        {
            transform.position = nearestNode.transform.position;
        }
    }

    public bool HasReachedGoal()
    {
        if (pathfinder == null || graph == null || graph.GoalNode == null)
            return false;

        float distanceSqr = (graph.GoalNode.transform.position - transform.position).sqrMagnitude;

        return (distanceSqr < 0.01f);
    }

    private void UpdatePlayerNode()
    {
        pathfinder?.SetStartNode(transform.position);
        currentNode = pathfinder.StartNode;
    }

    public void EndGame()
    {
        canMove = false;
    }
}
