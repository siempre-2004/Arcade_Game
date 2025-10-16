using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class storageLED : MonoBehaviour
{
    private static storageLED _instance;

    public static storageLED Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<storageLED>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<storageLED>();
                    singletonObject.name = typeof(storageLED).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    private bool lookedIn = true;
    private bool isSceneLoaded = false;
    private bool isCoroutineRunning = false;
    private IEnumerator lookCoroutine;

    public string loseScene = "EndGame_LoseState";

    private float lookTime;
    private float waitTime;
    public float minWaitTime = 10f;
    public float maxWaitTime = 15f;
    public float minLookInTime = 2f;
    public float maxLookInTime = 5f;

    public SerialController serialController; // Reference to the SerialController script

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
        waitTime = Random.Range(minWaitTime, maxWaitTime);
        InitializeSerialController();
        if (serialController != null)
        {
            Debug.Log("SerialController found, sending initial message.");
            waitTime = Random.Range(minWaitTime, maxWaitTime);
            lookTime = Random.Range(minLookInTime, maxLookInTime);
            Debug.Log($"Initial wait time: {waitTime}, Initial look time: {lookTime}");

            if (!isCoroutineRunning)
            {
                lookCoroutine = LookInAndOutRoutine();
                StartCoroutine(lookCoroutine);
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

    IEnumerator LookInAndOutRoutine()
    {
        Debug.Log("LookInAndOutRoutine coroutine started.");
        isCoroutineRunning = true;

        yield return new WaitForSeconds(3);


        while (true)
        {
            if (lookedIn)
            {
                Debug.Log(waitTime);

                yield return new WaitForSeconds(waitTime);
                lookedIn = false;
                lookTime = Random.Range(minLookInTime, maxLookInTime);
                serialController.SendSerialMessage("C");
                Debug.Log("Looking in.");
                Debug.Log($"New look time: {lookTime}");
            }
            else
            {
                yield return new WaitForSeconds(lookTime);
                isSceneLoaded = SceneManager.GetSceneByName("StorageFacility").isLoaded;
                if (isSceneLoaded)
                {
                    serialController.SendSerialMessage("F");
                    SceneManager.LoadScene(loseScene);
                }
                lookedIn = true;
                waitTime = Random.Range(minWaitTime, maxWaitTime);
                serialController.SendSerialMessage("C");
                Debug.Log("Not looking in.");
                Debug.Log($"Reset look time: {lookTime}, New wait time: {waitTime}");
            }
        }
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

        if (!isCoroutineRunning && serialController != null)
        {
            Debug.Log("Restarting coroutine on scene load.");
            lookCoroutine = LookInAndOutRoutine();
            StartCoroutine(lookCoroutine);
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
