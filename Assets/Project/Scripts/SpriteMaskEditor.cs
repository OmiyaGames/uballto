using System.Collections;
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
        public Rect ViewportRect { get => Window.Display.uvRect;  set => Window.Display.uvRect = value; }
    }

    [SerializeField]
    Camera orthogonalCamera;

    bool isDirty = true;
    List<WindowInfo> allWindows = null;
    Dictionary<DragDrop, WindowInfo> dragToWindowMap = null;
    readonly HashSet<ProceduralSpriteGenerator.WindowLayer> dirtyLayers = new HashSet<ProceduralSpriteGenerator.WindowLayer>();

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
        // Calculate the screen corners
        foreach (Transform child in transform)
        {
            WindowRect script = child.GetComponent<WindowRect>();
            if (script != null)
            {
                WindowInfo newInfo = new WindowInfo(script);
                AllWindows.Add(newInfo);
                DragToWindowMap.Add(script.DragScript, newInfo);
                script.DragScript.OnBeforeDrag += MarkDirty;

                ProceduralSpriteGenerator.SpriteData data = ProceduralSpriteGenerator.GetSprite(script.DisplayLayer);
                script.Display.texture = data.CameraTexture;
            }
        }
    }

    private void MarkDirty(DragDrop source, Vector2 from, Vector2 to)
    {
        if (DragToWindowMap.TryGetValue(source, out WindowInfo info) == true)
        {
            // Mark the corresponding window dirty
            info.IsDirty = true;
            isDirty = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDirty)
        {
            // Update all the windows
            UpdateAllWindows();

            // Draw the sprite
            //foreach (ProceduralSpriteGenerator.WindowLayer layer in dirtyLayers)
            //{
            //    ProceduralSpriteGenerator.SpriteData data = ProceduralSpriteGenerator.GetSprite(layer);

            //    // Go through all coordinates
            //    for (int i = 0; i < data.Count; ++i)
            //    {
            //        if (IsPixelOpaque(layer, data, i))
            //        {
            //            data[i] = 255;
            //        }
            //        else
            //        {
            //            data[i] = 0;
            //        }
            //    }

            //    // Apply all the changes
            //    data.Apply();
            //}

            // Re-calculate each window's rect
            isDirty = false;
        }
    }

    private bool IsPixelOpaque(ProceduralSpriteGenerator.WindowLayer layer, ProceduralSpriteGenerator.SpriteData data, int pixelIndex)
    {
        // Convert the index to coordinates
        Vector2Int pixelPosition = new Vector2Int(Mathf.FloorToInt(pixelIndex % data.Width), Mathf.FloorToInt(pixelIndex / data.Width));

        // Go through all the windows in reverse order
        bool isPixelOpaque = false;
        for (int index = (AllWindows.Count - 1); index >= 0; --index)
        {
            // Check if this window contains this pixel
            if (AllWindows[index].ViewportRect.Contains(pixelPosition))
            {
                // If so, check the window's layer
                if (layer == ProceduralSpriteGenerator.WindowLayer.None)
                {
                    // If it's the default layer, it should always be drawn
                    isPixelOpaque = true;
                }
                else
                {
                    // If on a special layer, see if this window exposes this layer
                    // The top-most window (i.e. last) should have the highest priority
                    isPixelOpaque = ((AllWindows[index].Window.DisplayLayer & layer) != 0);
                }
                break;
            }
        }

        return isPixelOpaque;
    }

    private void UpdateAllWindows()
    {
        // Setup some variables
        Rect updateRect = new Rect();
        Vector3 bottomLeftViewportPosition, topRightViewportPosition;

        // Reset the layer list
        dirtyLayers.Clear();
        dirtyLayers.Add(ProceduralSpriteGenerator.WindowLayer.None);

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

                // Check which layer needs to be added
                AddLayer(info, ProceduralSpriteGenerator.WindowLayer.WaterOnly, dirtyLayers);
                AddLayer(info, ProceduralSpriteGenerator.WindowLayer.WindOnly, dirtyLayers);

                // Mark this window as clean
                info.IsDirty = false;
            }
        }
    }

    private static void AddLayer(WindowInfo info, ProceduralSpriteGenerator.WindowLayer layerToCheck, HashSet<ProceduralSpriteGenerator.WindowLayer> dirtyLayers)
    {
        if (((info.Window.DisplayLayer & layerToCheck) != 0) && (dirtyLayers.Contains(layerToCheck) == false))
        {
            // Add the layer into the list
            dirtyLayers.Add(layerToCheck);
        }
    }
}
