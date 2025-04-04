using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingMenu : MonoBehaviour
{
    public int maxLevels = 5; // Set the total number of levels

    public void LoadNextLevel()
    {
        // Get the last played level from PlayerPrefs
        string currentScene = PlayerPrefs.GetString("LastLevel", "level1"); 

        int levelNumber;
        if (int.TryParse(currentScene.Replace("level", ""), out levelNumber))
        {
            if (levelNumber < maxLevels)
            {
                string nextScene = "level" + (levelNumber + 1); // Load the next level
                SceneManager.LoadScene(nextScene);
                PlayerPrefs.SetString("LastLevel", nextScene); // Save new last level
            }
            else
            {
                SceneManager.LoadScene("Main Menu"); // If it's the last level, go to Main Menu
            }
        }
        else
        {
            SceneManager.LoadScene("level1"); // Fallback if something goes wrong
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Load Main Menu scene
    }
}
