using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using OmiyaGames;

[RequireComponent(typeof(Rigidbody2D))]
public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler//, IEndDragHandler
{
    [SerializeField]
    Canvas uiCanvas = null;

    Vector3 offsetFromMousePosition, lastMouseWorldPosition = Vector3.zero;
    Rigidbody2D body;
    Ray fromCamera;
    Plane playField;

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        // get diff
        offsetFromMousePosition = Body.transform.position - GetMouseWorldPosition(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.dragging == true)
        {
            Vector3 moveTo3D = offsetFromMousePosition + GetMouseWorldPosition(eventData.position);
            Body.MovePosition(new Vector2(moveTo3D.x, moveTo3D.y));
        }
    }

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    // Do...something?
    //}

    private Vector3 GetMouseWorldPosition(Vector3 pixelPosition)
    {
        float distance;
        fromCamera = RayCastCamera.ScreenPointToRay(pixelPosition);
        if (playField.Raycast(fromCamera, out distance) == true)
        {
            lastMouseWorldPosition = fromCamera.GetPoint(distance);
        }
        return lastMouseWorldPosition;
    }
}
