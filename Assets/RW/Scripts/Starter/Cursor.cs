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