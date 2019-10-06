using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DragDrop))]
public class WindowRect : MonoBehaviour
{
    [SerializeField]
    ProceduralSpriteGenerator.WindowLayer displayLayer;
    [SerializeField]
    RectTransform bottomLeftRange;
    [SerializeField]
    RectTransform topRightRange;
    [SerializeField]
    RawImage display;

    DragDrop dragDrop = null;

    public ProceduralSpriteGenerator.WindowLayer DisplayLayer { get => displayLayer; }
    public RectTransform BottomLeftRange { get => bottomLeftRange; }
    public RectTransform TopRightRange { get => topRightRange; }
    public Vector3 BottomLeftPosition { get => BottomLeftRange.position; }
    public Vector3 TopRightPosition { get => TopRightRange.position; }
    public DragDrop DragScript { get => OmiyaGames.Utility.GetComponentCached(this, ref dragDrop); }
    public RawImage Display { get => display; }

    public void OnExpandToggleChanged(bool isExpanded)
    {
        // FIXME: expand the dialog soon!
    }
}
