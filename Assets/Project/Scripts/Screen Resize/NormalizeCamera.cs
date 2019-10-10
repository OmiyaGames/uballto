using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class NormalizeCamera : IScreenResizeDetector
{
    [SerializeField]
    float expectedScreenResolution = 16f / 9f;

    Camera cameraCache;
    float currentScreenResolution;

    public Camera Camera
    {
        get => OmiyaGames.Utility.GetComponentCached(this, ref cameraCache);
    }

    public override void OnScreenSizeChanged(int lastScreenWidth, int lastScreenHeight, float lastScreenResolution)
    {
        // Grab camera
        float cameraOrthoSize = Camera.orthographicSize;

        // Get the current screen resolution
        GetCurrentScreenResolution(out float currentScreenResolution);

        // Increase the orthographic size so the width of the screen is fit.
        float lostScreenWidth = expectedScreenResolution - currentScreenResolution;
        Camera.orthographicSize = cameraOrthoSize * (1 + lostScreenWidth);
    }
}
