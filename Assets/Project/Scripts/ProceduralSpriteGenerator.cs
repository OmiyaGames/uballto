using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSpriteGenerator : MonoBehaviour
{
    public enum WindowLayer
    {
        Default,
        //Water,
        NumberOfLayers
    }

    [System.Serializable]
    public struct SetSprite
    {
        [SerializeField]
        SpriteMask mask;
        [SerializeField]
        SpriteRenderer testRenderer;
        [SerializeField]
        WindowLayer layer;

        public WindowLayer Layer { get => layer; }

        public void Setup()
        {
            if(mask != null)
            {
                mask.sprite = GetSprite(Layer).Sprite;
            }
            if (testRenderer != null)
            {
                testRenderer.sprite = GetSprite(Layer).Sprite;
            }
        }
    }

    public class SpriteData
    {
        public SpriteData(Sprite sprite, float pixelsPerUnit)
        {
            Sprite = sprite;
            PixelsPerUnit = pixelsPerUnit;
            Data = new byte[sprite.texture.width * sprite.texture.height];
        }

        public float PixelsPerUnit { get; }
        public Sprite Sprite { get; }
        public byte[] Data { get; }
        public Texture2D Texture => Sprite.texture;
        public float Width => Texture.width;
        public float Height => Texture.height;

        public void Apply()
        {
            Texture.LoadRawTextureData(Data);
            Texture.Apply();
        }
    }

    private static readonly Dictionary<WindowLayer, SpriteData> allSprites = new Dictionary<WindowLayer, SpriteData>((int)WindowLayer.NumberOfLayers);

    [SerializeField]
    Camera orthogonalCamera;
    [SerializeField]
    float pixelsPerUnit = 100;
    [SerializeField]
    SetSprite[] spritesToSet;

    public static SpriteData GetSprite(WindowLayer layer = WindowLayer.Default)
    {
        return allSprites[layer];
    }

    // Start is called before the first frame update
    void Awake()
    {
        // If allSprites is already filled, don't do anything!
        if(allSprites.Count > 0)
        {
            return;
        }

        // FIXME: figure out the height and width of the texture
        double textureHeightDecimal = orthogonalCamera.orthographicSize * 2 * pixelsPerUnit;
        double textureWidthDecimal = (textureHeightDecimal / Screen.height) * Screen.width;
        int textureHeightPixel = (int)textureHeightDecimal;
        int textureWidthPixel = (int)textureWidthDecimal;
        WindowLayer layer;

        // Go through all the sprites
        const int numLayers = (int)WindowLayer.NumberOfLayers;
        for (int index = 0; index < numLayers; ++index)
        {
            layer = (WindowLayer)index;
            Texture2D newTexture = new Texture2D(textureWidthPixel, textureHeightPixel, TextureFormat.Alpha8, false);
            newTexture.name = layer.ToString();

            // Create the sprite
            Sprite newSprite = Sprite.Create(newTexture,
                // Get the full rect of the texture
                new Rect(0, 0, textureWidthPixel, textureHeightPixel),
                new Vector2(0.5f, 0.5f), pixelsPerUnit);
            newSprite.name = newTexture.name;

            // Add the sprite into the dictionary
            allSprites.Add(layer, new SpriteData(newSprite, pixelsPerUnit));
        }
    }

    private void Start()
    {
        foreach(SetSprite set in spritesToSet)
        {
            set.Setup();

            // FIXME: Test code
            SpriteData data = GetSprite(set.Layer);
            for (int y = 0; y < data.Height; ++y)
            {
                for (int x = 0; x < data.Width; ++x)
                {
                    int i = Mathf.FloorToInt((y * data.Width) + x);
                    if ((x < (data.Width * 0.25f)) && (y < (data.Height * 0.75f)))
                    {
                        data.Data[i] = 128;
                    }
                }
            }
            data.Apply();
        }
    }

    private void OnApplicationQuit()
    {
        // Check if allSprites is filled
        if (allSprites.Count > 0)
        {
            // Go through all the values
            foreach(SpriteData sprite in allSprites.Values)
            {
                // Destroy the texture and sprite
                Destroy(sprite.Texture);
                Destroy(sprite.Sprite);
            }

            // Clear the dictionary
            allSprites.Clear();
        }
    }
}
