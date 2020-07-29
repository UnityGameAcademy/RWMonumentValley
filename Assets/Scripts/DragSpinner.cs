﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// allows a target Transform to be rotated based on mouse click and drag
[RequireComponent(typeof(Collider))]
public class DragSpinner : MonoBehaviour
{
    public enum SpinAxis
    {
        X,
        Y,
        Z
    }

    // transform to spin
    [SerializeField] private Transform targetToSpin;

    // axis of rotation
    [SerializeField] private SpinAxis spinAxis = SpinAxis.X; 

    // used to calculate angle to mouse pointer
    [SerializeField] private Transform pivot;

    // vector from pivot to mouse pointer
    private Vector2 directionToMouse;

    // are we currently spinning?
    private bool isSpinning;

    // angle (degrees) from clicked screen position 
    private float angleToMouse;

    // angle to mouse on previous frame
    private float previousAngleToMouse;

    // Vector representing axis of rotation
    private Vector3 axisDirection;

    // minimum distance in pixels before activating mouse drag
    [SerializeField] private int minDragDist = 10;


    void Start()
    {
        switch (spinAxis)
        {
            case (SpinAxis.X):
                axisDirection = Vector3.right;
                break;
            case (SpinAxis.Y):
                axisDirection = Vector3.up;
                break;
            case (SpinAxis.Z):
                axisDirection = Vector3.forward;
                break;
        }
    }

    private void OnMouseDrag()
    {
        // if collider has been clicked...
        if (isSpinning && Camera.main != null && pivot != null)
        {
            // get the angle to the current mouse position
            directionToMouse = Input.mousePosition - Camera.main.WorldToScreenPoint(pivot.position);
            angleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

            // if we have dragged a minimum threshold, rotate the target to follow the mouse movements around the pivot
            // (Unity uses left-handed coordinate system so positive rotations are clockwise)
            if (directionToMouse.magnitude > minDragDist)
            {
                Vector3 newRotationVector = (previousAngleToMouse - angleToMouse) * axisDirection;
                targetToSpin.Rotate(newRotationVector);
                previousAngleToMouse = angleToMouse;
            }
        }
    }

    // begin spin 
    private void OnMouseDown()
    {
        isSpinning = true;
        directionToMouse = Input.mousePosition - Camera.main.WorldToScreenPoint(pivot.position);
        previousAngleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
    }

    // end spin on mouse release; then round to right angle
    private void OnMouseUp()
    {
        isSpinning = false;
        RoundToRightAngles(targetToSpin);
    }

    // round to nearest 90 degrees
    private void RoundToRightAngles(Transform xform)
    {
        float roundedXAngle = Mathf.Round(xform.eulerAngles.x / 90f) * 90f;
        float roundedYAngle = Mathf.Round(xform.eulerAngles.y / 90f) * 90f;
        float roundedZAngle = Mathf.Round(xform.eulerAngles.z / 90f) * 90f;

        xform.eulerAngles = new Vector3(roundedXAngle, roundedYAngle, roundedZAngle);
    }
}
