using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nightLEDScript : MonoBehaviour
{
    private static nightLEDScript _instance;

    public static nightLEDScript Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<nightLEDScript>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<nightLEDScript>();
                    singletonObject.name = typeof(nightLEDScript).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    public SerialController serialController; // Reference to the SerialController script
    public float timeBetweenLEDOff = 15f;

    public string loseScene = "EndGame_LoseState";


    private bool isCoroutineRunning = false;
    private IEnumerator messageCoroutine;

    void Awake()
    {
        Debug.Log("Awake called.");
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("Instance set and DontDestroyOnLoad called.");
        }
        else if (_instance != this)
        {
            Debug.Log("Another instance exists, destroying this one.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("Start called.");
        InitializeSerialController();
        if (serialController != null)
        {
            Debug.Log("SerialController found, sending initial message and starting coroutine.");
            if (!isCoroutineRunning)
            {
                messageCoroutine = SendMessagesWithDelay();
                StartCoroutine(messageCoroutine);
            }
        }
    }

    void InitializeSerialController()
    {
        Debug.Log("InitializeSerialController called.");
        serialController = GameObject.Find("SerialController")?.GetComponent<SerialController>();
        if (serialController == null)
        {
            Debug.LogError("SerialController not found");
        }
    }

    IEnumerator SendMessagesWithDelay()
    {
        Debug.Log("SendMessagesWithDelay coroutine started.");
        isCoroutineRunning = true;

        if (serialController != null)
        {
            Debug.Log("Sending A");
            serialController.SendSerialMessage("A");
            Debug.Log("Sending B");
            serialController.SendSerialMessage("B");
            Debug.Log("Sending D");
            serialController.SendSerialMessage("D");
            Debug.Log("Sending E");
            serialController.SendSerialMessage("E");

            yield return new WaitForSeconds(timeBetweenLEDOff);
            Debug.Log("Sending A");
            serialController.SendSerialMessage("A");
            yield return new WaitForSeconds(timeBetweenLEDOff);
            Debug.Log("Sending B");
            serialController.SendSerialMessage("B");
            yield return new WaitForSeconds(timeBetweenLEDOff);
            Debug.Log("Sending E");
            serialController.SendSerialMessage("E");
            yield return new WaitForSeconds(timeBetweenLEDOff);
            Debug.Log("Sending D");
            serialController.SendSerialMessage("D");
            SceneManager.LoadScene(loseScene);
        }

        isCoroutineRunning = false;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Destroy(gameObject);
        }
        Debug.Log("OnSceneLoaded called.");
        if (serialController == null)
        {
            Debug.Log("SerialController was null, reinitializing.");
            InitializeSerialController();
        }

        // Check if the coroutine is already running before starting it
        if (!isCoroutineRunning && serialController != null)
        {
            Debug.Log("Starting coroutine on scene load.");
            messageCoroutine = SendMessagesWithDelay();
            StartCoroutine(messageCoroutine);
        }
    }

    

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit called.");
        if (serialController != null)
        {
            serialController.SendSerialMessage("F");
        }
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy called.");
        if (_instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
