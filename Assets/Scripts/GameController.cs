using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private ScenesManager scenesManager;
    private MapRandom mapRandom;

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "GameScene") {
            InitGameScene();
        }
    }

    void Start() {
        scenesManager = FindObjectOfType<ScenesManager>();
    }

    private void InitGameScene() {
        mapRandom = FindObjectOfType<MapRandom>();
        if (mapRandom != null) {
            //HandleGameLogic();
        } else {
            Debug.LogError("MapRandom component cant be found in GameScene.");
        }
    }

   /* private void HandleGameLogic() {
        Debug.Log("Night duration: " + mapRandom.nightDuration);
        mapRandom.Update();  
        StartCoroutine(WaitAndEndGame(mapRandom.nightDuration, true));
    }*/

    IEnumerator WaitAndEndGame(float delay, bool win) {
        yield return new WaitForSeconds(delay);
        scenesManager.LoadEndScene(win);
    }
}
