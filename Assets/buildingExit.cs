using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buildingExit : MonoBehaviour
{
    private static buildingExit _instance;
    private bool storageDumpLoaded = false;

    public static buildingExit Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<buildingExit>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<buildingExit>();
                    singletonObject.name = typeof(buildingExit).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded called. Scene: " + scene.name);

        if (scene.name == "StorageFacility")
        {
            storageDumpLoaded = true;
            Debug.Log("storageDump scene loaded. Flag set to true.");
        }

        if (scene.name == "Game Scene 2.0" && storageDumpLoaded)
        {
            Debug.Log("GameScene loaded and storageDumpLoaded is true.");
            GameObject player = GameObject.Find("Player");
            GameObject storageFacility = GameObject.Find("Storage Facility");

            if (player != null && storageFacility != null)
            {
                Debug.Log("Player and storageFacility found.");
                player.transform.position = storageFacility.transform.position - new Vector3(0, 4, 0);
                Debug.Log("Player moved to one unit below storageFacility.");
            }
            else
            {
                if (player == null)
                {
                    Debug.LogWarning("Player object not found.");
                }
                if (storageFacility == null)
                {
                    Debug.LogWarning("storageFacility object not found.");
                }
            }
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
