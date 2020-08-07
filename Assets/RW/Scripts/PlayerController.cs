
/*
 * Copyright (c) 2020 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RW.MonumentValley
{
    // handles Player input and movement
    public class PlayerController : MonoBehaviour
    {

        // time to move one unit
        [Range(0.25f, 2f)]
        [SerializeField] private float moveTime = 0.25f;

        // multiplier for walk AnimationClip
        [Range(0.5f, 3f)]
        [SerializeField] private float walkAnimSpeed = 1f;

        // player Animator Controller
        [SerializeField] private Animator animController;

        // click indicator
        [SerializeField] Cursor cursor;

        //[SerializeField] GameObject oldCursor;

        // cursor AnimationController
        private Animator cursorAnimController;

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

            // initialize fields
            if (pathfinder != null)
            {
                graph = pathfinder.GetComponent<Graph>();
            }

            isMoving = false;
            isGameOver = false;
        }

        private void Start()
        {

            // set AnimationClip speed
            animController?.SetFloat("walkSpeedMultiplier", walkAnimSpeed);

            // always start on a Node
            SnapToNearestNode();
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
            if (!isMoving && !isGameOver)
            {

                if (cursor != null)
                {
                    cursor.ShowCursor(clickedNode.transform.position);

                }

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
                t += Time.deltaTime / moveTime;
                transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.Clamp(t, 0, 1));

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
}