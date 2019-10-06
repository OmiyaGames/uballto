using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using OmiyaGames;

[RequireComponent(typeof(Rigidbody2D))]
public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public delegate void OnDragDetected(DragDrop source, PointerEventData input, Vector2 movedTo);
    public event OnDragDetected OnAfterDrag;

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
            Body.MovePosition(moveTo);
            isDragged = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // get diff
        bool isDetected;
        offsetFromMousePosition = Body.transform.position - GetMouseWorldPosition(eventData.position, out isDetected);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Check if dragging
        if (eventData.dragging == true)
        {
            // Check if mouse position is detected
            bool isDetected;
            moveTo = offsetFromMousePosition + GetMouseWorldPosition(eventData.position, out isDetected);
            if(isDetected)
            {
                // Indicate to the next FixedUpdate to move the Rigidbody.
                isDragged = true;
                OnAfterDrag?.Invoke(this, eventData, moveTo);
            }
        }
    }

    private Vector3 GetMouseWorldPosition(Vector3 pixelPosition, out bool isMouseDetected)
    {
        fromCamera = RayCastCamera.ScreenPointToRay(pixelPosition);

        float distance;
        isMouseDetected = playField.Raycast(fromCamera, out distance);
        if (isMouseDetected)
        {
            lastMouseWorldPosition = fromCamera.GetPoint(distance);
        }
        return lastMouseWorldPosition;
    }
}
