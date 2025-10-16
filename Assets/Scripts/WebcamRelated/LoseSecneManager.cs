using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoseSceneManager : MonoBehaviour
{
    public string MainMenuScene; // The name of the main menu scene
    public float LoseSceneDuration = 5f; // Duration to display the win scene

    void Start()
    {
        StartCoroutine(TransitionToMainMenu());
    }

    private IEnumerator TransitionToMainMenu()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(LoseSceneDuration);

        // Load the main menu scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(MainMenuScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Main menu scene loaded.");
    }
}
