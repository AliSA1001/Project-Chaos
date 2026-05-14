using UnityEngine;

using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour

{

    public void StartGame()

    {
Debug.Log("WORKING");
        SceneManager.LoadScene("L1");

    }

}