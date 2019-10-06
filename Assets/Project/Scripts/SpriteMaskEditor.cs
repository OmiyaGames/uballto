﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaskEditor : MonoBehaviour
{
    public class WindowInfo
    {
        public WindowInfo(WindowRect window)
        {
            Window = window;
        }

        public WindowRect Window { get; }

        public bool IsDirty { get; set; } = true;

        // FIXME: get rid of the getter
        public Rect ViewportRect { set => Window.Display.uvRect = value; }
    }

    [SerializeField]
    Camera orthogonalCamera;

    List<WindowInfo> allWindows = null;
    Dictionary<DragDrop, WindowInfo> dragToWindowMap = null;

    #region Properties
    /// <summary>
    /// A list of Windows scripts as child of this transform.  Sorted in reverse priority order, i.e. the last window is the last activated, and as such, has the highest priority.
    /// </summary>
    public List<WindowInfo> AllWindows
    {
        get
        {
            if (allWindows == null)
            {
                allWindows = new List<WindowInfo>(transform.childCount);
            }
            return allWindows;
        }
    }

    public Dictionary<DragDrop, WindowInfo> DragToWindowMap
    {
        get
        {
            if (dragToWindowMap == null)
            {
                dragToWindowMap = new Dictionary<DragDrop, WindowInfo>(transform.childCount);
            }
            return dragToWindowMap;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // FIXME: figure out which render texture to release resources from
        HashSet<ProceduralSpriteGenerator.WindowLayer> activeLayers = new HashSet<ProceduralSpriteGenerator.WindowLayer>();

        // Calculate the screen corners
        foreach (Transform child in transform)
        {
            WindowRect script = child.GetComponent<WindowRect>();
            if (script != null)
            {
                // Setup the window wrapper
                WindowInfo newInfo = new WindowInfo(script);
                AllWindows.Add(newInfo);

                // Bind to all the windows events
                DragToWindowMap.Add(script.DragScript, newInfo);
                script.DragScript.OnBeforeDrag += MarkDirty;

                // Apply the texture to the window's display
                script.Display.texture = ProceduralSpriteGenerator.GetTexture(script.DisplayLayer);

                // Add this window's layer into the active set
                activeLayers.Add(script.DisplayLayer);
            }
        }

        // Go through all layers
        ProceduralSpriteGenerator.WindowLayer layer;
        for (int layerId = 0; layerId < ProceduralSpriteGenerator.NumberOfLayers; ++layerId)
        {
            layer = (ProceduralSpriteGenerator.WindowLayer)layerId;
            if (activeLayers.Contains(layer) == false)
            {
                // Release the unused textures
                ProceduralSpriteGenerator.GetTexture(layer).Release();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Setup some variables
        Rect updateRect = new Rect();
        Vector3 bottomLeftViewportPosition, topRightViewportPosition;

        // Go through all windows
        foreach (WindowInfo info in AllWindows)
        {
            // Check if the window is dirty
            if (info.IsDirty)
            {
                // Grab the viewport position of each corner of this window
                bottomLeftViewportPosition = orthogonalCamera.WorldToViewportPoint(info.Window.BottomLeftPosition);
                topRightViewportPosition = orthogonalCamera.WorldToViewportPoint(info.Window.TopRightPosition);

                // Convert these viewport positions to pixel positions
                updateRect.xMin = bottomLeftViewportPosition.x;
                updateRect.yMin = bottomLeftViewportPosition.y;
                updateRect.xMax = topRightViewportPosition.x;
                updateRect.yMax = topRightViewportPosition.y;
                info.ViewportRect = updateRect;

                // Mark this window as clean
                info.IsDirty = false;
            }
        }
    }

    private void MarkDirty(DragDrop source, Vector2 from, Vector2 to)
    {
        if (DragToWindowMap.TryGetValue(source, out WindowInfo info) == true)
        {
            // Mark the corresponding window dirty
            info.IsDirty = true;
        }
    }
}
