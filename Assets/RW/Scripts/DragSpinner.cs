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

    // minimum distance in pixels before activating mouse drag
    [SerializeField] private int minDragDist = 10;

    [SerializeField] private NodeLinker nodeLinker;

    // vector from pivot to mouse pointer
    private Vector2 directionToMouse;

    // are we currently spinning?
    private bool isSpinning;

    private bool isActive;

    // angle (degrees) from clicked screen position 
    private float angleToMouse;

    // angle to mouse on previous frame
    private float previousAngleToMouse;

    // Vector representing axis of rotation
    private Vector3 axisDirection;


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
        EnableSpinner(true);
    }

    private void OnMouseDrag()
    {
        // if clicked...
        if (isSpinning && Camera.main != null && pivot != null && isActive)
        {
            // get the angle to the current mouse position
            directionToMouse = Input.mousePosition - Camera.main.WorldToScreenPoint(pivot.position);
            angleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

            // if we have dragged a minimum threshold, rotate the target to follow the mouse movements around the pivot
            // (left-handed coordinate system; positive rotations are clockwise)
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
        if (!isActive)
            return;

        isSpinning = true;
        directionToMouse = Input.mousePosition - Camera.main.WorldToScreenPoint(pivot.position);
        previousAngleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
    }

    // end spin on mouse release; then round to right angle
    private void OnMouseUp()
    {
        if (!isActive)
            return;

        isSpinning = false;

        // snap to nearest 90-degree interval
        RoundToRightAngles(targetToSpin);

        // don't like referencing this directly but not using separate game Events to save on word count
        nodeLinker?.UpdateLinks();

    }

    // round to nearest 90 degrees
    private void RoundToRightAngles(Transform xform)
    {
        float roundedXAngle = Mathf.Round(xform.eulerAngles.x / 90f) * 90f;
        float roundedYAngle = Mathf.Round(xform.eulerAngles.y / 90f) * 90f;
        float roundedZAngle = Mathf.Round(xform.eulerAngles.z / 90f) * 90f;

        xform.eulerAngles = new Vector3(roundedXAngle, roundedYAngle, roundedZAngle);
    }

    private void EnableSpinner(bool state)
    {
        isActive = state;
    }
}
