using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowRect : MonoBehaviour
{
    [SerializeField]
    ProceduralSpriteGenerator.WindowLayer displayLayer;
    [SerializeField]
    RectTransform bottomLeftRange;
    [SerializeField]
    RectTransform topRightRange;

    public ProceduralSpriteGenerator.WindowLayer DisplayLayer { get => displayLayer; }
    public RectTransform BottomLeftRange { get => bottomLeftRange; }
    public RectTransform TopRightRange { get => topRightRange; }
    public Vector3 BottomLeftPosition { get => BottomLeftRange.position; }
    public Vector3 TopRightPosition { get => TopRightRange.position; }
}
