using UnityEngine;
using System.Collections;

namespace RW.MonumentValley
{
    /**/
    public class CameraFacingBillboard : MonoBehaviour
    {
        public Camera cam;

        private void Start()
        {
            if (cam == null)
                cam = Camera.main;
        }

        // face in the same direction as the camera
        void LateUpdate()
        {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
        }
    }

}