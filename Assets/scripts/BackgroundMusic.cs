using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance; // Ensure only one AudioManager exists

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep music playing through scene changes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate AudioManagers
        }
    }
}
