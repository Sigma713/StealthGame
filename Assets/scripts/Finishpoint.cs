using UnityEngine;
using UnityEngine.SceneManagement;

public class Finishpoint : MonoBehaviour
{
    public int maxLevels = 5; // Set the total number of levels

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string currentScene = SceneManager.GetActiveScene().name; // Get current level name
            PlayerPrefs.SetString("LastLevel", currentScene); // Save current level

            // Extract level number from name (e.g., "level1" → 1)
            int levelNumber;
            if (int.TryParse(currentScene.Replace("level", ""), out levelNumber))
            {
                if (levelNumber >= maxLevels) // If on the last level (level5)
                {
                    SceneManager.LoadScene("WinScreen"); // Load the Win Screen
                }
                else
                {
                    SceneManager.LoadScene("Loading"); // Load the Loading screen for next level transition
                }
            }
            else
            {
                SceneManager.LoadScene("Loading"); // Fallback in case of an issue
            }
        }
    }
}
