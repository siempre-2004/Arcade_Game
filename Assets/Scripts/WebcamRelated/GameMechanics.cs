using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Include this namespace for UI components
using System.Collections;

public class ObjectSpawning : MonoBehaviour
{
    public GameObject redPrefab;
    public GameObject greenPrefab;
    public GameObject bluePrefab;
    public Transform[] spawnPoints;
    public WebcamTexture webcamTextureScript;

    public TextMeshProUGUI RedScore;
    public TextMeshProUGUI GreenScore;
    public TextMeshProUGUI BlueScore;

    private int redScoreCount = 0;
    private int greenScoreCount = 0;
    private int blueScoreCount = 0;
    private int totalPrefabsSpawned = 0;
    private int totalPrefabsDestroyed = 0;

    private int redCount;
    private int greenCount;
    private int blueCount;

    public TextMeshProUGUI popupText; // Reference to the TextMeshProUGUI object displaying the first message
    public TextMeshProUGUI popupText2; // Reference to the TextMeshProUGUI object displaying the second message
    public Image popupImage; // Reference to the Image object for the popup image
    public float popupDuration = 3f; // Duration of the popup message

    public string WinScene;

    void Start()
    {
        // Generate random counts for each prefab
        GeneratePrefabCounts();
        StartSpawn();
    }

    void GeneratePrefabCounts()
    {
        redCount = Random.Range(1, 2);
        greenCount = Random.Range(1, 2);
        blueCount = Random.Range(1, 2);
        totalPrefabsSpawned = redCount + greenCount + blueCount;
    }

    void DisplayPrefabCountMessage()
    {
        string message = string.Format("You need to collect Red: {0} Green: {1} Blue: {2} to win", redCount, greenCount, blueCount);
        Debug.Log(message); // Log the message to the console
        ShowPopupMessage(popupText, message);
        StartCoroutine(HidePopupAfterDelay(popupText, popupDuration));
    }

    void DisplayGuardWarningMessage()
    {
        string message = "Use the joystick to return to the game scene\nDon't let the guard catch you";
        Debug.Log(message); // Log the message to the console
        ShowPopupMessage(popupText2, message);
        StartCoroutine(HidePopupAfterDelay(popupText2, popupDuration));
    }

    void ShowPopupImage()
    {
        popupImage.gameObject.SetActive(true);
        StartCoroutine(HidePopupImageAfterDelay(popupDuration));
    }

    void ShowPopupMessage(TextMeshProUGUI popup, string message)
    {
        popup.text = message;
        popup.gameObject.SetActive(true);
    }

    IEnumerator HidePopupAfterDelay(TextMeshProUGUI popup, float delay)
    {
        yield return new WaitForSeconds(delay);
        popup.gameObject.SetActive(false);
    }

    IEnumerator HidePopupImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupImage.gameObject.SetActive(false);
    }

    void StartSpawn()
    {
        // Start spawning objects
        List<GameObject> prefabs = new List<GameObject>();

        // Add red prefabs to the list
        for (int i = 0; i < redCount; i++)
        {
            prefabs.Add(redPrefab);
        }

        // Add green prefabs to the list
        for (int i = 0; i < greenCount; i++)
        {
            prefabs.Add(greenPrefab);
        }

        // Add blue prefabs to the list
        for (int i = 0; i < blueCount; i++)
        {
            prefabs.Add(bluePrefab);
        }

        // Shuffle the prefabs list
        ShuffleList(prefabs);

        // Determine the number of prefabs to spawn (minimum of the total counts and available spawn points)
        int spawnCount = Mathf.Min(totalPrefabsSpawned, spawnPoints.Length);

        for (int i = 0; i < spawnCount; i++)
        {
            // Instantiate the prefab at the spawn point
            Instantiate(prefabs[i], spawnPoints[i].position, Quaternion.identity);
        }

        // Display the prefab count message after spawning
        DisplayPrefabCountMessage();
        // Display the guard warning message
        DisplayGuardWarningMessage();
        // Display the popup image
        ShowPopupImage();
    }

    public void DestroyPrefabByColor(Color color)
    {
        // Check which prefab matches the color and add it to the list
        GameObject prefabToDestroy = null;
        if (color == webcamTextureScript.DetectedRedColor)
        {
            prefabToDestroy = FindFirstWithTag("RedObject");
            if (prefabToDestroy != null)
            {
                redScoreCount++;
                RedScore.text = redScoreCount.ToString() + " Collected";
            }
        }
        else if (color == webcamTextureScript.DetectedGreenColor)
        {
            prefabToDestroy = FindFirstWithTag("GreenObject");
            if (prefabToDestroy != null)
            {
                greenScoreCount++;
                GreenScore.text = greenScoreCount.ToString() + " Collected";
            }
        }
        else if (color == webcamTextureScript.DetectedBlueColor)
        {
            prefabToDestroy = FindFirstWithTag("BlueObject");
            if (prefabToDestroy != null)
            {
                blueScoreCount++;
                BlueScore.text = blueScoreCount.ToString() + " Collected";
            }
        }

        // If no matching color found or no prefabs to destroy, return
        if (prefabToDestroy == null)
        {
            return;
        }

        // Destroy the first prefab of the specified color
        Destroy(prefabToDestroy);

        // Increment the total prefabs destroyed count
        totalPrefabsDestroyed++;

        // Check if all prefabs are destroyed
        if (totalPrefabsDestroyed == totalPrefabsSpawned)
        {
            webcamTextureScript.StopWebcam();
            // All prefabs are destroyed, transition to win scene
            SceneManager.LoadScene(WinScene);
        }
    }

    private GameObject FindFirstWithTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        if (objectsWithTag.Length > 0)
        {
            return objectsWithTag[0];
        }
        return null;
    }

    // Fisher-Yates shuffle algorithm to shuffle arrays
    void ShuffleArray<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    // Fisher-Yates shuffle algorithm to shuffle lists
    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}


/*using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ObjectSpawning : MonoBehaviour
{
    public GameObject redPrefab;
    public GameObject greenPrefab;
    public GameObject bluePrefab;
    public Transform[] spawnPoints;
    public WebcamTexture webcamTextureScript;

    public TextMeshProUGUI RedScore;
    public TextMeshProUGUI GreenScore;
    public TextMeshProUGUI BlueScore;

    private int redScoreCount = 0;
    private int greenScoreCount = 0;
    private int blueScoreCount = 0;
    private int totalPrefabsSpawned = 0;
    private int totalPrefabsDestroyed = 0;

    private int redCount;
    private int greenCount;
    private int blueCount;

    public TextMeshProUGUI popupText; // Reference to the TextMeshProUGUI object displaying the message
    public Image popupImage; // Reference to the Image component for the popup
    public float popupDuration = 3f; // Duration of the popup message

    public string WinScene;


    void Start()
    {

        StartSpawn();
    }

    void GeneratePrefabCounts()
    {
        redCount = Random.Range(1, 9);
        greenCount = Random.Range(1, 9);
        blueCount = Random.Range(1, 9);
        totalPrefabsSpawned = redCount + greenCount + blueCount;
    }

    void DisplayPrefabCountMessage()
    {
        string message = string.Format("You need to collect Red Box: {0} Green Box: {1} Blue Box: {2} to win", redCount, greenCount, blueCount);
        Debug.Log(message); // Log the message to the console
        ShowPopupMessage(message);
        StartCoroutine(HidePopupAfterDelay(popupDuration));
    }

    void ShowPopupMessage(string message)
    {
        popupText.text = message;
        popupText.gameObject.SetActive(true);
        popupImage.gameObject.SetActive(true); // Show the image
    }

    IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupText.gameObject.SetActive(false);
        popupImage.gameObject.SetActive(false); // Hide the image
    }

    void StartSpawn()
    {
        // Start spawning objects
        List<GameObject> prefabs = new List<GameObject>();

        // Add red prefabs to the list
        for (int i = 0; i < redCount; i++)
        {
            prefabs.Add(redPrefab);
        }

        // Add green prefabs to the list
        for (int i = 0; i < greenCount; i++)
        {
            prefabs.Add(greenPrefab);
        }

        // Add blue prefabs to the list
        for (int i = 0; i < blueCount; i++)
        {
            prefabs.Add(bluePrefab);
        }

        // Shuffle both the prefabs list and the spawn points array
        ShuffleList(prefabs);
        ShuffleArray(spawnPoints);

        // Determine the number of prefabs to spawn (minimum of the total counts)
        int spawnCount = Mathf.Min(totalPrefabsSpawned, spawnPoints.Length);

        for (int i = 0; i < spawnCount; i++)
        {
            // Get the prefab to spawn
            GameObject prefabToSpawn = prefabs[i];

            // Instantiate the prefab at a random spawn point
            Instantiate(prefabToSpawn, spawnPoints[i].position, Quaternion.identity);
        }

        // Display the prefab count message after spawning
        DisplayPrefabCountMessage();
    }

    public void DestroyPrefabByColor(Color color)
    {
        // Check which prefab matches the color and add it to the list
        GameObject prefabToDestroy = null;
        if (color == webcamTextureScript.DetectedRedColor)
        {
            prefabToDestroy = FindFirstWithTag("RedObject");
            if (prefabToDestroy != null)
            {
                redScoreCount++;
                RedScore.text = redScoreCount.ToString();
                MoveAndAnimate(prefabToDestroy, "RedBox");//S_Changed
            }
        }
        else if (color == webcamTextureScript.DetectedGreenColor)
        {
            prefabToDestroy = FindFirstWithTag("GreenObject");
            if (prefabToDestroy != null)
            {
                greenScoreCount++;
                GreenScore.text = greenScoreCount.ToString();
                MoveAndAnimate(prefabToDestroy, "GreenBox");//S_Changed
            }
        }
        else if (color == webcamTextureScript.DetectedBlueColor)
        {
            prefabToDestroy = FindFirstWithTag("BlueObject");
            if (prefabToDestroy != null)
            {
                blueScoreCount++;
                BlueScore.text = blueScoreCount.ToString();
                MoveAndAnimate(prefabToDestroy, "BlueBox");//S_Changed
            }
        }

        // If no matching color found or no prefabs to destroy, return
        if (prefabToDestroy == null)
        {
            return;
        }

        // Destroy the first prefab of the specified color
        Destroy(prefabToDestroy);

        // Increment the total prefabs destroyed count
        //totalPrefabsDestroyed++;

        // Check if all prefabs are destroyed
       /* if (totalPrefabsDestroyed == totalPrefabsSpawned)
        {
            // All prefabs are destroyed, transition to win scene
            SceneManager.LoadScene(WinScene);
        }
    }

    private GameObject FindFirstWithTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        if (objectsWithTag.Length > 0)
        {
            return objectsWithTag[0];
        }
        return null;
    }

    // Fisher-Yates shuffle algorithm to shuffle arrays
    void ShuffleArray<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    // Fisher-Yates shuffle algorithm to shuffle lists
    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }*/

//S_Changed

/* private void TriggerAnimation(GameObject prefab, string triggerName)
 {
     Animator animator = prefab.GetComponent<Animator>();
     Vector3 targetPosition = prefab.transform.position + Vector3.up * 2f;
     StartCoroutine(MoveAndAnimate(prefab.transform, animator, targetPosition, triggerName));
 }

 IEnumerator MoveAndAnimate(Transform transform, Animator animator, Vector3 targetPosition, string triggerName)
 {
     float duration = 1f; // Adjust as needed
     float time = 0f;
     Vector3 startPosition = transform.position;

     while (time < duration)
     {
         transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
         time += Time.deltaTime;
         yield return null;
     }

     transform.position = targetPosition;

     // Trigger the animation
     animator.SetTrigger(triggerName); // Adjust triggerName according to the actual trigger parameter name
 }*/

/* private void TriggerAnimation(GameObject prefab)
 {
     // Get the Animator component attached to the prefab
     Animator animator = prefab.GetComponent<Animator>();

     // Trigger the animation based on the tag of the prefab
     if (prefab.CompareTag("RedObject"))
     {
         MoveUpAndAnimate(prefab, animator, "RedBoxTrigger");
     }
     else if (prefab.CompareTag("GreenObject"))
     {
         MoveUpAndAnimate(prefab, animator, "GreenBoxTrigger");
     }
     else if (prefab.CompareTag("BlueObject"))
     {
         MoveUpAndAnimate(prefab, animator, "BlueBoxTrigger");
     }
 }

 private void MoveUpAndAnimate(GameObject prefab, Animator animator, string triggerName)
 {
     // Move the prefab up (you may need to adjust the Y coordinate)
     Vector3 targetPosition = prefab.transform.position + Vector3.up * 2f;
     StartCoroutine(MovePrefab(prefab.transform, targetPosition, 1f));

     // Wait for a moment to ensure the box moves up before triggering the animation
     StartCoroutine(PlayAnimationAfterDelay(animator, triggerName, 1f));
 }

 IEnumerator MovePrefab(Transform transform, Vector3 targetPosition, float duration)
 {
     float time = 0f;
     Vector3 startPosition = transform.position;

     while (time < duration)
     {
         transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
         time += Time.deltaTime;
         yield return null;
     }

     transform.position = targetPosition;
 }

 IEnumerator PlayAnimationAfterDelay(Animator animator, string triggerName, float delay)
 {
     yield return new WaitForSeconds(delay);
     animator.SetTrigger(triggerName);
 }*/

/*private void MoveAndAnimate(GameObject prefab, string triggerName)
 {
     Animator animator = prefab.GetComponent<Animator>();
     Vector3 targetPosition = prefab.transform.position + Vector3.up * 2f;
     StartCoroutine(MoveAndTriggerAnimation(prefab.transform, animator, targetPosition, triggerName,prefab));
 }

 IEnumerator MoveAndTriggerAnimation(Transform transform, Animator animator, Vector3 targetPosition, string triggerName, GameObject prefab)
 {
     float duration = 1f; // Adjust as needed
     float time = 0f;
     Vector3 startPosition = transform.position;

     while (time < duration)
     {
         transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
         time += Time.deltaTime;
         yield return null;
     }

     transform.position = targetPosition;

     // Trigger the animation
     animator.SetTrigger(triggerName);

     yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

     Destroy(prefab);

     totalPrefabsDestroyed++;
     if (totalPrefabsDestroyed == totalPrefabsSpawned)
     {
         SceneManager.LoadScene(WinScene);
     }
 } // this is the last and latest version

}
*/
