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

        public Rect PixelRect { get; set; } = new Rect();
    }

    [SerializeField]
    Camera orthogonalCamera;

    List<WindowInfo> allWindows = null;
    readonly HashSet<ProceduralSpriteGenerator.WindowLayer> dirtyLayers = new HashSet<ProceduralSpriteGenerator.WindowLayer>();
    Dictionary<DragDrop, WindowInfo> dragToWindowMap = null;
    bool isDirty = true;

    #region Properties
    /// <summary>
    /// A list of Windows scripts as child of this transform.  Sorted in reverse priority order, i.e. the last window is the last activated, and as such, has the highest priority.
    /// </summary>
    public List<WindowInfo> AllWindows
    {
        get
        {
            allWindows = new List<WindowInfo>(transform.childCount);
            return allWindows;
        }
    }

    public Dictionary<DragDrop, WindowInfo> DragToWindowMap
    {
        get
        {
            dragToWindowMap = new Dictionary<DragDrop, WindowInfo>(transform.childCount);
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
                AllWindows.Add(new WindowInfo(script));
            }
            script.DragScript.OnAfterDrag += MarkDirty;
        }
    }

    private void MarkDirty(DragDrop source, UnityEngine.EventSystems.PointerEventData input, Vector2 movedTo)
    {
        WindowInfo info;
        if (DragToWindowMap.TryGetValue(source, out info) == true)
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
            foreach (ProceduralSpriteGenerator.WindowLayer layer in dirtyLayers)
            {
                ProceduralSpriteGenerator.SpriteData data = ProceduralSpriteGenerator.GetSprite(layer);

                // Go through all coordinates
                Vector2 pixelPosition = Vector2.zero;
                for (int i = 0; i < data.Count; ++i)
                {
                    if (IsPixelOpaque(layer, data, i))
                    {
                        data[i] = 255;
                    }
                    else
                    {
                        data[i] = 0;
                    }
                }
            }

            // Re-calculate each window's rect
            isDirty = false;
        }
    }

    private bool IsPixelOpaque(ProceduralSpriteGenerator.WindowLayer layer, ProceduralSpriteGenerator.SpriteData data, int pixelIndex)
    {
        Vector2 pixelPosition;
        // Convert the index to coordinates
        pixelPosition.x = pixelIndex % data.Width;
        pixelPosition.y = pixelIndex / data.Width;

        // Go through all the windows in reverse order
        bool isPixelOpaque = false;
        for(int index = (AllWindows.Count - 1); index >= 0; --index)
        {
            // Check if this window contains this pixel
            if (AllWindows[index].PixelRect.Contains(pixelPosition))
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
        foreach (WindowInfo info in allWindows)
        {
            // Check if the window is dirty
            if (info.IsDirty)
            {
                // Grab the viewport position of each corner of this window
                bottomLeftViewportPosition = orthogonalCamera.WorldToViewportPoint(info.Window.BottomLeftPosition);
                topRightViewportPosition = orthogonalCamera.WorldToViewportPoint(info.Window.TopRightPosition);

                // Convert these viewport positions to pixel positions
                updateRect.xMin = bottomLeftViewportPosition.x * ProceduralSpriteGenerator.TextureWidthPixel;
                updateRect.yMin = bottomLeftViewportPosition.y * ProceduralSpriteGenerator.TextureHeightPixel;
                updateRect.xMax = topRightViewportPosition.x * ProceduralSpriteGenerator.TextureWidthPixel;
                updateRect.yMax = topRightViewportPosition.y * ProceduralSpriteGenerator.TextureHeightPixel;
                info.PixelRect = updateRect;

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
