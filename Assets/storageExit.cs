using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorageExit : MonoBehaviour
{
    public string gameScene = "Game Scene 2.0";
    private bool delayFinished = false;
    private IEnumerator exitDelay;

    void Start()
    {
        Debug.Log("Start called.");
        exitDelay = ExitDelay();
        StartCoroutine(exitDelay);
    }
    
    void Update()
    {
        if (delayFinished)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

            if (movement.magnitude > 0)
            {
                Debug.Log("Exited");
                SceneManager.LoadScene(gameScene);
            }
        }
    }

    IEnumerator ExitDelay()
    {
        Debug.Log("exitDelay coroutine started.");
        yield return new WaitForSeconds(2);
        delayFinished = true;
    }

   
}
