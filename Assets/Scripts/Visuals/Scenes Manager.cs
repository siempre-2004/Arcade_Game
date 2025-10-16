using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public string[] sceneNames = { "MainMenu", "GameScene 2.0", "EndGame_WinState", "EndGame_LoseState"};
    public Animator transition;
    public float transitionTime = 1f;

    void Awake() 
    {
        DontDestroyOnLoad(this.gameObject); // Avoid destroying between scene transitions 
    }

    public void ChangeScene(int sceneIndex){
        if (sceneIndex >= 0 && sceneIndex < sceneNames.Length){
            StartCoroutine(LoadScene(sceneNames[sceneIndex]));
        }else{
            Debug.LogError("Invalid scene index!");
        }
    }

    IEnumerator LoadScene(string sceneName){
        if (transition != null) {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
        }
        SceneManager.LoadScene(sceneName);
    }

    public void LoadEndScene(bool win) // Win parameter for winning this game
    {
        int sceneIndex = win ? 2 : 3; // 2 for Winscene, 3 for Losescene
        ChangeScene(sceneIndex);
        StartCoroutine(ReturnToMainMenu(transitionTime + 5)); // Wait additional time before returning
    }

    IEnumerator ReturnToMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeScene(0); // 0 for MainMenu
    }
}



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public string[] sceneNames = { "MainMenu", "GameScene", "EndGame_WinState", "EndGame_LoseState" };
    public Animator transition;
    public float transitionTime = 1f;
    public KeyCode changeSceneKey = KeyCode.B; // Change this to the desired keyboard button

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // Avoid destroying between scene transitions 
    }

    void Update()
    {
        // Check if the specified keyboard button is pressed
        if (Input.GetKeyDown(changeSceneKey))
        {
            // Call ChangeScene method to change the scene to the next one in the array
            ChangeScene(GetNextSceneIndex());
        }
    }

    int GetNextSceneIndex()
    {
        // Find the index of the current scene and return the index of the next scene in the array
        string currentScene = SceneManager.GetActiveScene().name;
        int currentIndex = System.Array.IndexOf(sceneNames, currentScene);
        return (currentIndex + 1) % sceneNames.Length;
    }

    public void ChangeScene(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < sceneNames.Length)
        {
            StartCoroutine(LoadScene(sceneNames[sceneIndex]));
        }
        else
        {
            Debug.LogError("Invalid scene index!");
        }
    }

   private IEnumerator LoadScene(string sceneName)
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
        }
        SceneManager.LoadScene(sceneName);
    }

    public void LoadEndScene(bool win)
    {
        int sceneIndex = win ? 2 : 3; // 2 for WinScene, 3 for LoseScene
        StartCoroutine(LoadEndSceneCoroutine(sceneIndex));
        //ChangeScene(sceneIndex);
        //StartCoroutine(ReturnToMainMenu(10f)); // Wait additional time before returning
    }

    private IEnumerator LoadEndSceneCoroutine(int sceneIndex)
    {
        yield return StartCoroutine(LoadScene(sceneNames[sceneIndex]));
        StartCoroutine(ReturnToMainMenu(10f));
    }
    private IEnumerator ReturnToMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeScene(0); // 0 for MainMenu
    }
}          */


/*using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public Animator transition; // Animator controlling the fade animations
    public float transitionTime = 1f; // Time for the transition animation

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // Prevent this object from being destroyed when loading new scenes
    }

    // Method to trigger scene change with animation
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    // Coroutine to handle the scene loading with transition animation
    private IEnumerator LoadScene(string sceneName)
    {
        // Trigger the start of the fade-out animation
        if (transition != null)
        {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime); // Wait for the animation to complete
        }

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }
}*/


