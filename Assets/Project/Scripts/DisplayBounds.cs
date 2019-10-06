using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DisplayBounds : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ToggleActivation toggle = collision.GetComponent<ToggleActivation>();
        if(toggle != null)
        {
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
