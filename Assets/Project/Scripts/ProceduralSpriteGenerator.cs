using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSpriteGenerator : MonoBehaviour
{
    public const int NumberOfLayers = 1 + (int)WindowLayer.AllLayers;

    public enum WindowLayer
    {
        Base,
        BaseAndWater,
        BaseAndWind,
        AllLayers
    }

    [System.Serializable]
    public struct TextureSetter
    {
        [SerializeField]
        WindowLayer layer;
        [SerializeField]
        Camera camera;

        public WindowLayer Layer { get => layer; }

        public void Setup()
        {
            if (camera != null)
            {
                camera.targetTexture = GetTexture(Layer);
            }
        }
    }

    private static readonly Dictionary<WindowLayer, RenderTexture> allTextures = new Dictionary<WindowLayer, RenderTexture>(NumberOfLayers);

    [SerializeField]
    Camera orthogonalCamera;
    [SerializeField]
    [UnityEngine.Serialization.FormerlySerializedAs("spritesToSet")]
    TextureSetter[] renderCameras;

    public static RenderTexture GetTexture(WindowLayer layer)
    {
        return allTextures[layer];
    }

    // Start is called before the first frame update
    void Awake()
    {
        // If allSprites is already filled, don't do anything!
        if (allTextures.Count > 0)
        {
            return;
        }

        // Go through all the layers
        WindowLayer layer;
        for (int layerId = 0; layerId < NumberOfLayers; ++layerId)
        {
            layer = (WindowLayer)layerId;
            RenderTexture newRenderTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.Default, 0);
            newRenderTexture.name = layer.ToString();

            // Add the texture into the dictionary
            allTextures.Add(layer, newRenderTexture);
        }
    }

    private void Start()
    {
        foreach (TextureSetter set in renderCameras)
        {
            set.Setup();
        }
    }

    private void OnApplicationQuit()
    {
        // Check if allSprites is filled
        if (allTextures.Count > 0)
        {
            // Go through all the values
            foreach (RenderTexture texture in allTextures.Values)
            {
                // Destroy the texture and sprite
                Destroy(texture);
            }

            // Clear the dictionary
            allTextures.Clear();
        }
    }
}
