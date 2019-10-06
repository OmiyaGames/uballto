using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IToggleActivator : MonoBehaviour
{
    public class LayerInfo
    {
        int layerIndex = -1;
        public LayerInfo(ProceduralSpriteGenerator.WindowLayer layer, string name)
        {
            LayerEnum = layer;
            LayerName = name;
        }

        public ProceduralSpriteGenerator.WindowLayer LayerEnum { get; }
        public string LayerName { get; }
        public int LayerIndex
        {
            get
            {
                if (layerIndex < 0)
                {
                    layerIndex = LayerMask.NameToLayer(LayerName);
                }
                return layerIndex;
            }
        }
    }

    static readonly Dictionary<ProceduralSpriteGenerator.WindowLayer, LayerInfo> layerMap = new Dictionary<ProceduralSpriteGenerator.WindowLayer, LayerInfo>()
    {
        { ProceduralSpriteGenerator.WindowLayer.Base, new LayerInfo(ProceduralSpriteGenerator.WindowLayer.Base, "Default") }
        , { ProceduralSpriteGenerator.WindowLayer.BaseAndWater, new LayerInfo(ProceduralSpriteGenerator.WindowLayer.BaseAndWater, "Water Dimension") }
    };
    readonly HashSet<DisplayBounds> inBounds = new HashSet<DisplayBounds>();

    public DisplayBounds HighestPriorityBound { get; private set; } = null;

    public static LayerInfo GetLayer(ProceduralSpriteGenerator.WindowLayer layer)
    {
        return layerMap[layer];
    }

    public int NumberOfBounds => inBounds.Count;

    public bool ContainsBounds(DisplayBounds bounds)
    {
        return inBounds.Contains(bounds);
    }

    protected abstract void AfterEnterNewBound(DisplayBounds newBounds);
    protected abstract void AfterExitExistingBound(DisplayBounds removedBounds);

    public void EnterBound(DisplayBounds bounds)
    {
        if (inBounds.Add(bounds))
        {
            // Find the highest priority bound
            UpdateHighestPriorityBound();
            AfterEnterNewBound(bounds);
        }
    }

    public void ExitBound(DisplayBounds bounds)
    {
        if (inBounds.Remove(bounds))
        {
            // Find the highest priority bound
            UpdateHighestPriorityBound();
            AfterExitExistingBound(bounds);
        }
    }

    private void UpdateHighestPriorityBound()
    {
        HighestPriorityBound = null;
        if (inBounds.Count > 0)
        {
            foreach (DisplayBounds bound in inBounds)
            {
                if (HighestPriorityBound == null)
                {
                    HighestPriorityBound = bound;
                }
                else if (bound.Priority > HighestPriorityBound.Priority)
                {
                    HighestPriorityBound = bound;
                }
            }
        }
    }
}
