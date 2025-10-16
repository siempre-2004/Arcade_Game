using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathNote : MonoBehaviour
{
    public Renderer targetRenderer;
    private WebCamTexture webcamTexture;
    private Color32[] pixels;
    private Color detectedRedColor;
    private Color detectedGreenColor;
    private Color detectedBlueColor;
    private float debounceDelay = 3f;
    private float lastDetectionTime = 0f;
    private bool sceneLoaded = false; // Flag to prevent multiple scene loads
    public string DeathScene;

    public Color DetectedRedColor
    {
        get { return detectedRedColor; }
    }

    public Color DetectedGreenColor
    {
        get { return detectedGreenColor; }
    }

    public Color DetectedBlueColor
    {
        get { return detectedBlueColor; }
    }

    void Start()
    {
        webcamTexture = new WebCamTexture();
        targetRenderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    void Update()
    {
        if (!sceneLoaded && (detectedRedColor != Color.clear || detectedGreenColor != Color.clear || detectedBlueColor != Color.clear))
        {
            SceneManager.LoadScene(DeathScene); // Replace "YourSceneName" with the name of your scene
            sceneLoaded = true; // Set the flag to true to prevent multiple scene loads
        }

        if (webcamTexture.didUpdateThisFrame)
        {
            pixels = webcamTexture.GetPixels32();
            if (Time.time - lastDetectionTime > debounceDelay)
            {
                DetectColors();
                lastDetectionTime = Time.time;
            }
        }
    }

    public void DetectColors()
    {
        foreach (Color32 pixel in pixels)
        {
            if (pixel.r > 200 && pixel.g < 100 && pixel.b < 100)
            {
                detectedRedColor = new Color32(pixel.r, pixel.g, pixel.b, 255);
                Debug.Log("Red color detected");
                break;
            }
            else if (pixel.r < 110 && pixel.g > 110 && pixel.b < 100)
            {
                detectedGreenColor = new Color32(pixel.r, pixel.g, pixel.b, 255);
                Debug.Log("Green color detected");
                break;
            }
            else if (pixel.r < 100 && pixel.g < 100 && pixel.b > 200)
            {
                detectedBlueColor = new Color32(pixel.r, pixel.g, pixel.b, 255);
                Debug.Log("Blue color detected");
                break;
            }
        }
    }

    public void StopWebcam()
    {

        webcamTexture.Stop();
        Debug.Log("Webcam stopped.");

    }

    // This method is called when a new scene is loaded
    void OnDisable()
    {
        StopWebcam();
    }

    void OnDestroy()
    {
        StopWebcam();
    }

    void OnApplicationQuit()
    {
        StopWebcam();
    }

}
