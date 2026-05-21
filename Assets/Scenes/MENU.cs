using UnityEngine;

using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour

{
    public void Start()
    {
        
    }
    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
    }

    public void StartGame()

    {
Debug.Log("Play_WORKING");
        SceneManager.LoadScene(1);
    }
      public void QuitGame()
    { 
    
Debug.Log("Quit_WORKING");
        Application.Quit();

    }

}

