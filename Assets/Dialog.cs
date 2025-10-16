using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textcomponent;
    public string[] lines;
    public float textSpeed;
    public float linesDelay;
    public AudioSource mainCameraAudio;
    private static bool hasDialogueBeenShown = false;
    private int index;

    private void Awake()
    {
        // animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (hasDialogueBeenShown)
        {
            gameObject.SetActive(false); 
        }
        else
        {
            Time.timeScale = 0;
            textcomponent.text = string.Empty;
            StartDialogue();
            AudioListener.pause = true;
            mainCameraAudio.Pause();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SkipDialogue();
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textcomponent.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }
        yield return new WaitForSecondsRealtime(linesDelay);
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textcomponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        PlayMusic();
        EnablePlayerMovement();
        hasDialogueBeenShown = true;
    }

    void SkipDialogue()
    {
        StopAllCoroutines();
        EndDialogue();
    }

    void PlayMusic()
    {
        AudioListener.pause = false;
        mainCameraAudio.Play();
    }

    void EnablePlayerMovement()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }
}
