using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [Header("Scripts to Disable on Pause")]
    public Behaviour playerLookScript;
    public Behaviour playerShootScript;

    [Header("UI Panels")]
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    void Start()
    {
        // 1. Force menus closed on start
        pauseMenuUI.SetActive(false);
        if (optionsMenuUI != null) optionsMenuUI.SetActive(false);

        // 2. Ensure time is running and state is correct
        Time.timeScale = 1f;
        GameIsPaused = false;

        // 3. Lock the cursor for the FPS controller
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        // Listen for the Escape key to toggle the menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                // If we are in the options menu, go back to pause menu. Otherwise, resume game.
                if (optionsMenuUI.activeSelf)
                    CloseOptions();
                else
                    Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);

        // Unfreeze time
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Lock the cursor back to the center of the screen for the FPS controller
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);

        // Freeze game time
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Unlock the cursor so the player can click the UI buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Turn the camera and shooting scripts OFF
        if (playerLookScript != null) playerLookScript.enabled = false;
        if (playerShootScript != null) playerShootScript.enabled = false;
    }

    public void OpenOptions()
    {
        // Hide main pause menu, show options
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void CloseOptions()
    {
        // Hide options, show main pause menu
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Exiting Game...");
        // Note: Application.Quit() does not work in the Unity Editor, only in builds.
        Application.Quit();
    }
}