using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using OmiyaGames;

[RequireComponent(typeof(Rigidbody2D))]
public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public delegate void OnDragDetected(DragDrop source, Vector2 oldPosition, Vector2 newPosition);
    public event OnDragDetected OnBeforeDrag;

    [SerializeField]
    Canvas uiCanvas = null;

    Vector3 offsetFromMousePosition, lastMouseWorldPosition = Vector3.zero, moveTo;
    Rigidbody2D body;
    Ray fromCamera;
    Plane playField;
    bool isDragged = false;

    #region Properties
    public Rigidbody2D Body
    {
        get
        {
            return Utility.GetComponentCached(this, ref body);
        }
    }

    public Camera RayCastCamera
    {
        get
        {
            return UiCanvas.worldCamera;
        }
    }

    public Canvas UiCanvas
    {
        get
        {
            if(uiCanvas == null)
            {
                uiCanvas = transform.parent.GetComponent<Canvas>();
            }
            return uiCanvas;
        }
    }
    #endregion

    void Start()
    {
        // Setup the plane
        playField = new Plane(
            // Normal is facing away from the camera
            (RayCastCamera.transform.forward * -1f),
            // Position is the camera, forward by canvas plane distance
            (RayCastCamera.transform.position + (RayCastCamera.transform.forward * UiCanvas.planeDistance)));
    }

    void FixedUpdate()
    {
        if(isDragged)
        {
            OnBeforeDrag?.Invoke(this, Body.position, moveTo);
            Body.MovePosition(moveTo);
            isDragged = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // get diff
        TryGetMouseWorldPosition(eventData.position, ref lastMouseWorldPosition);
        offsetFromMousePosition = Body.transform.position - lastMouseWorldPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Check if dragging
        if (eventData.dragging == true)
        {
            // Check if mouse position is detected
            if(TryGetMouseWorldPosition(eventData.position, ref lastMouseWorldPosition))
            {
                // Indicate to the next FixedUpdate to move the Rigidbody.
                moveTo = offsetFromMousePosition + lastMouseWorldPosition;
                isDragged = true;
            }
        }
    }

    private bool TryGetMouseWorldPosition(Vector3 pixelPosition, ref Vector3 newWorldPosition)
    {
        fromCamera = RayCastCamera.ScreenPointToRay(pixelPosition);

        float distance;
        bool isMouseDetected = playField.Raycast(fromCamera, out distance);
        if (isMouseDetected)
        {
            newWorldPosition = fromCamera.GetPoint(distance);
        }
        return isMouseDetected;
    }
}
