using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    static CursorManager instance;

    [System.Serializable]
    public struct CursorInfo
    {
        public Texture2D cursor;
        public Vector2 centerOfCursor;
    }

    [SerializeField]
    CursorInfo defaultCursor;
    [SerializeField]
    CursorInfo dragCursor;
    [SerializeField]
    CursorInfo clickCursor;

    bool isDragDisplayed = false;
    bool isClickDisplayed = false;

    public static void SetClickCursor(bool enabled)
    {
        instance.isClickDisplayed = enabled;
        instance.UpdateCursor();
    }

    public static void SetDragCursor(bool enabled)
    {
        instance.isDragDisplayed = enabled;
        instance.UpdateCursor();
    }

    private void Awake()
    {
        SetCursor(defaultCursor);
        instance = this;
    }

    private void UpdateCursor()
    {
        if(isClickDisplayed)
        {
            SetCursor(clickCursor);
        }
        else if(isDragDisplayed)
        {
            SetCursor(dragCursor);
        }
        else
        {
            SetCursor(defaultCursor);
        }
    }

    private void SetCursor(CursorInfo info)
    {
        Cursor.SetCursor(info.cursor, info.centerOfCursor, CursorMode.Auto);
    }
}
