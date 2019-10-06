using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DisplayBounds : MonoBehaviour
{
    [SerializeField]
    WindowRect rect;

    public int Priority
    {
        get => rect.transform.GetSiblingIndex();
    }

    public ProceduralSpriteGenerator.WindowLayer Layer
    {
        get => rect.DisplayLayer;
    }

    public System.Action<WindowRect> OnFloatedToTop
    {
        get => rect.OnFloatedToTop;
        set => rect.OnFloatedToTop = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IToggleActivator toggle = collision.GetComponent<IToggleActivator>();
        if(toggle != null)
        {
            toggle.EnterBound(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IToggleActivator toggle = collision.GetComponent<IToggleActivator>();
        if (toggle != null)
        {
            toggle.ExitBound(this);
        }
    }
}
