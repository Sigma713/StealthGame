using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void LoadLevel()
    {
        // Get the last played level from PlayerPrefs
        string lastLevel = PlayerPrefs.GetString("LastLevel", "level1"); 

        // Reload the same level
        SceneManager.LoadScene(lastLevel);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Load Main Menu scene
    }
}
