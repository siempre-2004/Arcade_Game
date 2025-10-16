using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamTexture : MonoBehaviour
{
    public Renderer targetRenderer;
    private WebCamTexture webcamTexture;
    private Color32[] pixels;
    public ObjectSpawning objectSpawning; // Reference to ObjectSpawning script
    public Color detectedRedColor;
    public Color detectedGreenColor;
    public Color detectedBlueColor;
    private float debounceDelay = 0f;
    private float lastDetectionTime = 0f;

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
        Debug.Log("WebcamTexture: " + webcamTexture);
        targetRenderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    void Update()
    {
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
                objectSpawning.DestroyPrefabByColor(detectedRedColor); // Destroy red prefab
                break;
            }
            /*else if (pixel.r < 100 && pixel.g > 200 && pixel.b < 100)
            {
                detectedGreenColor = new Color32(pixel.r, pixel.g, pixel.b, 255);
                Debug.Log("Green color detected");
                objectSpawning.DestroyPrefabByColor(detectedGreenColor); // Destroy green prefab
                break;
            }*/

            else if (pixel.r < 110 && pixel.g > 110 && pixel.b < 100)
            {
                detectedGreenColor = new Color32(pixel.r, pixel.g, pixel.b, 255);
                Debug.Log("Green color detected");
                objectSpawning.DestroyPrefabByColor(detectedGreenColor); // Destroy green prefab
                break;
            }


            else if (pixel.r < 110 && pixel.g < 110 && pixel.b > 200)
            {
                detectedBlueColor = new Color32(pixel.r, pixel.g, pixel.b, 255);
                Debug.Log("Blue color detected");
                objectSpawning.DestroyPrefabByColor(detectedBlueColor); // Destroy blue prefab
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

/*
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class WebcamTexture : MonoBehaviour
{
    public Renderer targetRenderer;
    private WebCamTexture webcamTexture;
    private Color32[] pixels;
    public ObjectSpawning objectSpawning; // Reference to ObjectSpawning script
    public Color detectedRedColor;
    public Color detectedGreenColor;
    public Color detectedBlueColor;
    private float debounceDelay = 0.5f;
    private float lastDetectionTime = 0f;

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
        if (targetRenderer == null)
        {
            Debug.LogError("Target Renderer is not set.");
            return;
        }

#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            StartCoroutine(WaitForCameraPermission());
            return;
        }
#endif

        InitializeWebcam();
    }

#if UNITY_ANDROID
    private System.Collections.IEnumerator WaitForCameraPermission()
    {
        yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.Camera));
        InitializeWebcam();
    }
#endif

    void InitializeWebcam()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            Debug.Log("Webcams detected: " + devices.Length);
            foreach (var device in devices)
            {
                Debug.Log("Webcam available: " + device.name);
            }

            webcamTexture = new WebCamTexture(devices[0].name, 640, 480, 30); // Set a specific resolution and frame rate
            targetRenderer.material.mainTexture = webcamTexture;
            webcamTexture.Play();

            Debug.Log("Webcam started with resolution: " + webcamTexture.width + "x" + webcamTexture.height);
        }
        else
        {
            Debug.LogError("No webcams detected.");
        }
    }

    void Update()
    {
        if (webcamTexture != null)
        {
            if (webcamTexture.isPlaying)
            {
                if (webcamTexture.didUpdateThisFrame)
                {
                    Debug.Log("Webcam frame updated.");
                    pixels = webcamTexture.GetPixels32();
                    if (Time.time - lastDetectionTime > debounceDelay)
                    {
                        DetectColors();
                        lastDetectionTime = Time.time;
                    }
                }
                else
                {
                    Debug.LogWarning("Webcam did not update this frame.");
                    // Try to restart the webcam if frames are not updating
                    RestartWebcam();
                }
            }
            else
            {
                Debug.LogError("WebcamTexture is not playing. Restarting...");
                RestartWebcam();
            }
        }
        else
        {
            Debug.LogError("WebcamTexture is not initialized.");
        }
    }

    void RestartWebcam()
    {
        webcamTexture.Stop();
        webcamTexture.Play();
    }

    public void DetectColors()
    {
        foreach (Color32 pixel in pixels)
        {
            if (pixel.r > 200 && pixel.g < 100 && pixel.b < 100)
            {
                detectedRedColor = new Color32(pixel.r, pixel.g, pixel.b, 255);
                Debug.Log("Red color detected");
                objectSpawning.DestroyPrefabByColor(detectedRedColor); // Destroy red prefab
                break;
            }
            else if (pixel.r < 110 && pixel.g > 110 && pixel.b < 100)
            {
                detectedGreenColor = new Color32(pixel.r, pixel.g, pixel.b, 255);
                Debug.Log("Green color detected");
                objectSpawning.DestroyPrefabByColor(detectedGreenColor); // Destroy green prefab
                break;
            }
            else if (pixel.r < 110 && pixel.g < 110 && pixel.b > 200)
            {
                detectedBlueColor = new Color32(pixel.r, pixel.g, pixel.b, 255);
                Debug.Log("Blue color detected");
                objectSpawning.DestroyPrefabByColor(detectedBlueColor); // Destroy blue prefab
                break;
            }
        }
    }
}
*/