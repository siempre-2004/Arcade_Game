using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryEnter : MonoBehaviour
{
    public string sceneToLoad;  

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.CompareTag("Player"))  
        {
            loadNewScene();  
        }
    }

    void loadNewScene()
    {
        SceneManager.LoadScene(sceneToLoad); 
    }

    void SaveMapData()
    {
        GameObject mapManager = GameObject.Find("MapManager"); 
        MapRandom mapRandom = mapManager.GetComponent<MapRandom>();
        mapRandom.SaveMapData();
    }
}
