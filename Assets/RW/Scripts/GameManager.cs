using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// monitors win condition and stops/pauses gameplay as needed
public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    private bool isGameOver;
    public bool IsGameOver => isGameOver;

    [SerializeField] Canvas UICanvas;
    [SerializeField] ScreenFader screenFader;
    [SerializeField] ScreenFader winText;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        UICanvas?.gameObject.SetActive(true);
    }

    private void Start()
    {
        screenFader?.FadeOff(1.5f);
        winText?.FadeOff(0.1f);

    }

    private void Update()
    {
        if (playerController == null)
        {
            Debug.LogWarning("GAMEMANAGER Warning: missing PlayerController!");
            return;
        }

        if (playerController.HasReachedGoal())
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

        // play win animation
        StartCoroutine(WinRoutine());
    }

    private IEnumerator WinRoutine()
    {
        // play Animation
        winText?.FadeOn(2f);

        // yield Animation time
        yield return new WaitForSeconds(2f);
        // fade out

        screenFader?.FadeOn(1f);

        // show restart button

        yield return new WaitForSeconds(1f);

        Restart();
    }

    public void Restart()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }



}
