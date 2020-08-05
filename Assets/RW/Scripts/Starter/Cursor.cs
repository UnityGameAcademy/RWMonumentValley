using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RW.MonumentValley
{
    [RequireComponent(typeof(Animator))]
    // marker identifying mouse clicks 
    public class Cursor : MonoBehaviour
    {
        // extra distance offset toward camera
        [SerializeField] private float offsetDistance = 5f;

        private Camera cam;

        // cursor AnimationController
        private Animator animController;

        private void Awake()
        {
            if (cam == null)
            {
                cam = Camera.main;
            }
            animController = GetComponent<Animator>();
        }

        // always look at camera
        void LateUpdate()
        {
            if (cam != null)
            {
                Vector3 cameraForward = cam.transform.rotation * Vector3.forward;
                Vector3 cameraUp = cam.transform.rotation * Vector3.up;

                transform.LookAt(transform.position + cameraForward, cameraUp);
            }
        }

        // show cursor at a position with an optional offset toward camera
        public void ShowCursor(Vector3 position)
        {
            if (cam != null && animController != null)
            {
                Vector3 cameraForwardOffset = cam.transform.rotation * new Vector3(0f, 0f, offsetDistance);
                transform.position = position - cameraForwardOffset;

                animController.SetTrigger("ClickTrigger");
            }
        }
    }

}