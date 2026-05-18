using UnityEngine;

using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour

{

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

