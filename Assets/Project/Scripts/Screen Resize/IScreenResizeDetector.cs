using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IScreenResizeDetector : MonoBehaviour
{
    Vector2Int lastScreenDimensions = Vector2Int.zero;
    float lastScreenResolution = 0f;

    public static void GetCurrentScreenResolution(out float currentResolution)
    {
        currentResolution = Screen.width;
        currentResolution /= Screen.height;
    }

    public abstract void OnScreenSizeChanged(int lastScreenWidth, int lastScreenHeight, float lastScreenResolution);

    // Start is called before the first frame update
    public virtual void Awake()
    {
        // Update cache values
        lastScreenDimensions.x = Screen.width;
        lastScreenDimensions.y = Screen.height;
        GetCurrentScreenResolution(out lastScreenResolution);

        // Run the event
        OnScreenSizeChanged(lastScreenDimensions.x, lastScreenDimensions.y, lastScreenResolution);
    }

    public virtual void Update()
    {
        if ((Screen.width != lastScreenDimensions.x) || (Screen.height != lastScreenDimensions.y))
        {
            // Run the event
            OnScreenSizeChanged(lastScreenDimensions.x, lastScreenDimensions.y, lastScreenResolution);

            // Update cache values
            lastScreenDimensions.x = Screen.width;
            lastScreenDimensions.y = Screen.height;
            GetCurrentScreenResolution(out lastScreenResolution);
        }
    }
}
