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
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace RW.MonumentValley
{

    // monitors win condition and stops/pauses gameplay as needed
    public class GameManager : MonoBehaviour
    {
        private PlayerController playerController;

        private bool isGameOver;
        public bool IsGameOver => isGameOver;

        [SerializeField] Canvas UICanvas;
        [SerializeField] ScreenFader screenFader;
        [SerializeField] ScreenFader winText;

        // invoked when starting the level
        public UnityEvent initEvent;

        // invoked before restarting the level
        public UnityEvent restartEvent;

        public float delayTime = 2f;

        private void Awake()
        {
            playerController = FindObjectOfType<PlayerController>();
            UICanvas?.gameObject.SetActive(true);
        }

        private void Start()
        {
            screenFader?.FadeOff(delayTime);
            initEvent.Invoke();
        }

        private void Update()
        {
            if (playerController != null && playerController.HasReachedGoal())
            {
                Win();
            }
        }

        private void Win()
        {
            // flag to ensure Win only triggers once
            if (isGameOver)
            {
                return;
            }
            isGameOver = true;

            // disable player controls
            playerController?.EndGame();

            // play win animation
            StartCoroutine(WinRoutine());
        }

        private IEnumerator WinRoutine()
        {
            restartEvent?.Invoke();

            // yield Animation time
            yield return new WaitForSeconds(2f);

        }

        public void Restart(float delay)
        {
            StartCoroutine(RestartRoutine(delay));
        }

        private IEnumerator RestartRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex);
        }
    }
}
