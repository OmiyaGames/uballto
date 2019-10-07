using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(DragDrop))]
public class WindowRect : MonoBehaviour, IPointerDownHandler
{
    public System.Action<WindowRect> OnFloatedToTop;

    [SerializeField]
    ProceduralSpriteGenerator.WindowLayer displayLayer;
    [SerializeField]
    RectTransform bottomLeftRange;
    [SerializeField]
    RectTransform topRightRange;
    [SerializeField]
    RawImage display;

    [Header("Toggle")]
    [SerializeField]
    bool canMinimize = true;
    [SerializeField]
    Toggle minimizeButton;
    [SerializeField]
    GameObject minimizeIcons;
    [SerializeField]
    GameObject expandIcons;
    [SerializeField]
    GameObject displayBox;

    [Header("Title Bar")]
    [SerializeField]
    BaseMeshEffect titleBarEffect;
    [SerializeField]
    Image titleBarBackground;
    [SerializeField]
    Sprite minimizeSprite;
    [SerializeField]
    Sprite expandSprite;

    DragDrop dragDrop = null;

    public ProceduralSpriteGenerator.WindowLayer DisplayLayer { get => displayLayer; }
    public RectTransform BottomLeftRange { get => bottomLeftRange; }
    public RectTransform TopRightRange { get => topRightRange; }
    public Vector3 BottomLeftPosition { get => BottomLeftRange.position; }
    public Vector3 TopRightPosition { get => TopRightRange.position; }
    public DragDrop DragScript { get => OmiyaGames.Utility.GetComponentCached(this, ref dragDrop); }
    public RawImage Display { get => display; }

    private void Start()
    {
        if(canMinimize)
        {
            minimizeButton.isOn = false;
        }
        else
        {
            minimizeButton.isOn = true;
            minimizeButton.interactable = false;
        }
        OnExpandToggleChanged(minimizeButton.isOn);
    }

    public void OnExpandToggleChanged(bool isExpanded)
    {
        // Expand the dialog!
        displayBox.SetActive(isExpanded);

        // Update icons
        minimizeIcons.SetActive(isExpanded);
        expandIcons.SetActive(!isExpanded);

        // Update title bar
        titleBarBackground.sprite = (isExpanded ? expandSprite : minimizeSprite);
        titleBarEffect.enabled = !isExpanded;

        // Run pointer down event
        OnPointerDown(null);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        OnFloatedToTop?.Invoke(this);
    }

    public void OnPointerHoverToggle()
    {
        CursorManager.SetClickCursor(true);
    }

    public void OnPointerExitToggle()
    {
        CursorManager.SetClickCursor(false);
    }
}
