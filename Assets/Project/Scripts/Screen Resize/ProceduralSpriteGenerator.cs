using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSpriteGenerator : IScreenResizeDetector
{
    public const int NumberOfLayers = (int)WindowLayer.AllLayers;

    public enum WindowLayer
    {
        Base,
        BaseAndWater,
        //BaseAndWind,
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
        public Camera Camera { get => camera; }

        public void Setup()
        {
            if (Camera != null)
            {
                Camera.targetTexture = GetTexture(Layer);
                Camera.forceIntoRenderTexture = true;
            }
        }
    }

    private class TextureInfo
    {
        public TextureInfo(WindowLayer layer, RenderTexture texture)
        {
            Layer = layer;
            Texture = texture;
        }

        public Camera Camera { get; set; } = null;
        public WindowLayer Layer { get; }
        public RenderTexture Texture { get; }
    }

    private static readonly Dictionary<WindowLayer, TextureInfo> allTextures = new Dictionary<WindowLayer, TextureInfo>(NumberOfLayers);

    [SerializeField]
    Camera orthogonalCamera;
    [SerializeField]
    [UnityEngine.Serialization.FormerlySerializedAs("spritesToSet")]
    TextureSetter[] renderCameras;

    public static RenderTexture GetTexture(WindowLayer layer)
    {
        return allTextures[layer].Texture;
    }

    public override void OnScreenSizeChanged(int lastScreenWidth, int lastScreenHeight, float lastScreenResolution)
    {
        // Check if not setup yet, or the screen resolution really did change
        if ((allTextures.Count == 0) || (Screen.width != lastScreenWidth) || (Screen.height != lastScreenHeight))
        {
            CleanUpTextures();

            // Go through all the layers
            WindowLayer layer;
            foreach (TextureSetter set in renderCameras)
            {
                layer = set.Layer;
                RenderTexture newRenderTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.Default, 0);
                newRenderTexture.name = layer.ToString();

                // Add the texture into the dictionary
                allTextures.Add(layer, new TextureInfo(layer, newRenderTexture));
            }
        }
    }

    private void Start()
    {
        TextureInfo textureInfo;
        foreach (TextureSetter set in renderCameras)
        {
            if(allTextures.TryGetValue(set.Layer, out textureInfo))
            {
                textureInfo.Camera = set.Camera;
                set.Setup();
            }
        }
    }

    private void OnApplicationQuit()
    {
        CleanUpTextures();
    }

    private void CleanUpTextures()
    {
        // Check if allSprites is filled
        if (allTextures.Count > 0)
        {
            // Go through all the values
            foreach (TextureInfo info in allTextures.Values)
            {
                // Destroy the texture and sprite
                if(info.Camera != null)
                {
                    info.Camera.targetTexture = null;
                }
                Destroy(info.Texture);
            }

            // Clear the dictionary
            allTextures.Clear();
        }
    }
}
