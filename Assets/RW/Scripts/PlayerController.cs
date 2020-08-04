using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles Player input and movement
public class PlayerController : MonoBehaviour
{
    // visualization indicating where clicked
    [SerializeField] GameObject cursor;

    // time to move one unit
    [Range(0.25f, 2f)]
    [SerializeField] float moveTime = 0.25f;
    [Range(0.5f, 3f)]
    [SerializeField] float walkAnimSpeed = 1f;

    // Animator Controller
    [SerializeField] Animator animController;

    // pathfinding fields
    private Clickable[] clickables;
    private Pathfinder pathfinder;
    private Graph graph;
    private Node currentNode;
    private Node nextNode;
    private bool hasReachedDestination;

    // flags
    private bool isMoving;
    private bool isGameOver;

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

        isGameOver = false;
    }

    private void Start()
    {
        pathfinder?.SetStartNode(transform.position);
        isMoving = false;

        // set AnimationClip speed
        animController?.SetFloat("walkSpeedMultiplier", walkAnimSpeed);
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
        if (!isMoving && !isGameOver)
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

            FaceNextNode(currentNode.transform.position, nextNode.transform.position);
            ToggleAnimation(isMoving);



            yield return StartCoroutine(MoveToPositionRoutine(currentNode.transform.position, nextNode.transform.position));
            i++;
        }
        hasReachedDestination = true;
        isMoving = false;
        ToggleAnimation(isMoving);
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

                // invoke UnityEvent associated with next Node
                nextNode?.playerEvent?.Invoke();
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
        nextNode = null;
    }
    // disable controls
    public void EndGame()
    {
        isGameOver = true;
    }

    // turn face the next Node, always projected on a plane at the Player's feet
    public void FaceNextNode(Vector3 startPosition, Vector3 nextPosition)
    {
        if (Camera.main == null)
        {
            return;
        }

        // convert next Node world space to screen space
        Vector3 nextPositionScreen = Camera.main.WorldToScreenPoint(nextPosition);

        // convert next Node screen point to Ray
        Ray rayToNextPosition = Camera.main.ScreenPointToRay(nextPositionScreen);

        // plane at player's feet
        Plane plane = new Plane(Vector3.up, startPosition);

        // distance from camera
        float cameraDistance = 0f;

        if (plane.Raycast(rayToNextPosition, out cameraDistance))
        {
            Vector3 nextPositionOnPlane = rayToNextPosition.GetPoint(cameraDistance);
            Vector3 diffVector = nextPositionOnPlane - startPosition;
            transform.rotation = Quaternion.LookRotation(diffVector);
        }
    }

    // toggle between idle and walking animation
    private void ToggleAnimation(bool state)
    {
        animController?.SetBool("isMoving", state);
    }

}
