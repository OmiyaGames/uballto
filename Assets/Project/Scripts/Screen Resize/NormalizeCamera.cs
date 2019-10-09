using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class NormalizeCamera : MonoBehaviour
{
    [SerializeField]
    float expectedScreenResolution = 16f / 9f;

    Camera cameraCache;
    float lastScreenResolution = -1f, currentScreenResolution;

    public Camera Camera
    {
        get => OmiyaGames.Utility.GetComponentCached(this, ref cameraCache);
    }

    public static void GetCurrentScreenResolution(out float currentResolution)
    {
        currentResolution = Screen.width;
        currentResolution /= Screen.height;
    }

    // Start is called before the first frame update
    void Awake()
    {
        // Get current screen resolution
        GetCurrentScreenResolution(out lastScreenResolution);
        UpdateResolution(lastScreenResolution);
    }

    private void Update()
    {
        // Get current screen resolution
        GetCurrentScreenResolution(out currentScreenResolution);
        if (Mathf.Approximately(currentScreenResolution, lastScreenResolution))
        {
            UpdateResolution(currentScreenResolution);
            lastScreenResolution = currentScreenResolution;
        }
    }

    private void UpdateResolution(float currentResolution)
    {
        // Grab camera
        float cameraOrthoSize = Camera.orthographicSize;

        // Increase the orthographic size so the width of the screen is fit.
        float lostScreenWidth = expectedScreenResolution - currentResolution;
        Camera.orthographicSize = cameraOrthoSize * (1 + lostScreenWidth);
    }
}
