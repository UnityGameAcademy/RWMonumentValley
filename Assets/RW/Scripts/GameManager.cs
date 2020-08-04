using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

// monitors win condition and stops/pauses gameplay as needed
public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    private bool isGameOver;
    public bool IsGameOver => isGameOver;

    [SerializeField] Canvas UICanvas;
    [SerializeField] ScreenFader screenFader;
    [SerializeField] ScreenFader winText;

    public UnityEvent initEvent;
    public UnityEvent restartEvent;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        UICanvas?.gameObject.SetActive(true);
    }

    private void Start()
    {

        screenFader?.FadeOff(1.5f);
        //winText?.FadeOff(0.1f);
        initEvent.Invoke();
        Debug.Log("Invoking INIT EVENT");
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
        //winText?.FadeOn(2f);

        restartEvent?.Invoke();

        // yield Animation time
        yield return new WaitForSeconds(2f);
        // fade out

       // screenFader?.FadeOn(1f);

        // show restart button

        yield return new WaitForSeconds(1f);


    }

    public void Restart(float delay)
    {
        StartCoroutine(RestartRoutine(delay));
    }

    private IEnumerator RestartRoutine(float delay = 1f)
    {
        yield return new WaitForSeconds(delay);
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }


}
