using UnityEngine;
using UnityEngine.SceneManagement;

public class Stop : MonoBehaviour
{
    public GameObject panel;
    public GameObject continueButton;
    public GameObject exitButton;
    private bool isPaused = false;

    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false); // Ensure the panel starts inactive
        }
        else
        {
            Debug.LogError("Panel is not assigned in the Inspector.");
        }
        //if (continueButton != null) continueButton.SetActive(false);
        //if (exitButton != null) exitButton.SetActive(false);
    }

    void Update()
    {
        TogglePause();
    }

    private void TogglePause()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && panel != null)
        {
            isPaused = !isPaused; // thay doi trang thai
            panel.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f; 
            Debug.Log($"Game Paused: {isPaused}");
        }
    }
    private void ToggleButtons(bool show)
    {
        if (continueButton != null) continueButton.SetActive(show);
        if (exitButton != null) exitButton.SetActive(show);
    }
    public void ContinueGame()
    {
        if (panel != null)
        {
            isPaused = false;
            panel.SetActive(false);
            Time.timeScale = 1f;
            Debug.Log("Game Resumed");
        }
    }

    // Called by the "Exit" button
    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Time.timeScale = 1f; // Reset time scale to normal before exiting
        Application.Quit();
    }
}
