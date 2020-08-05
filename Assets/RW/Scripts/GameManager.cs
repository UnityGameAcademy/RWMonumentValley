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
