using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButtonManager : MonoBehaviour
{
    public Button playButton;
    public KeyCode playKey = KeyCode.S;

    private void Start()
    {
        // Ensure the play button triggers the correct scene load
        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void Update()
    {
        // Check for the specified keyboard key press
        if (Input.GetKeyDown(playKey))
        {
            OnPlayButtonClicked();
        }
    }

    private void OnPlayButtonClicked()
    {
        // Reset the saved state
        ResetSavedState();

        // Load the game scene
        SceneManager.LoadScene("Game Scene 2.0");
    }

    private void ResetSavedState()
    {
        // Clear all saved preferences
        PlayerPrefs.DeleteAll();
    }
}
