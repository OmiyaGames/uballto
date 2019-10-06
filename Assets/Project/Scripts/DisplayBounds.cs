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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ToggleActivation toggle = collision.GetComponent<ToggleActivation>();
        if(toggle != null)
        {
            gameObject.layer = 1;
            toggle.EnterBounds(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ToggleActivation toggle = collision.GetComponent<ToggleActivation>();
        if (toggle != null)
        {
            toggle.ExitBounds(this);
        }
    }
}
