using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ToggleScript : IToggleActivator
{
    [SerializeField]
    MonoBehaviour script;

    protected override void AfterEnterNewBound(DisplayBounds bounds)
    {
        script.enabled = true;
    }

    protected override void AfterExitExistingBound(DisplayBounds bounds)
    {
        if (NumberOfBounds == 0)
        {
            script.enabled = false;
        }
    }

    protected override void AfterWindowOrderChanged()
    {
        // Do nothing!
    }
}
