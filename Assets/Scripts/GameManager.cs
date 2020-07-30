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


    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (playerController == null)
        {
            Debug.LogWarning("GAMEMANAGER Warning: missing PlayerController!");
            return;
        }

        if (isGameOver)
        {
            Win();
        }
    }

    private void Win()
    {
        // play win animation
    }

    public void Restart()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);


    }

}
