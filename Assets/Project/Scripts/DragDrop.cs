using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using OmiyaGames;

[RequireComponent(typeof(Rigidbody2D))]
public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 startDragPosition;
    Rigidbody2D body;

    public Rigidbody2D Body
    {
        get
        {
            return Utility.GetComponentCached(this, ref body);
        }
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.dragging == true)
        {
            // TODO: move the window to the new mouse position
            Body.MovePosition(eventData.position - startDragPosition);
            startDragPosition = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        startDragPosition = eventData.position;
    }
}
