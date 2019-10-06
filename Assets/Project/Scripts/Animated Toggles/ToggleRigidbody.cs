using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ToggleRigidbody : IToggleActivator
{
    [SerializeField]
    Rigidbody2D body;

    Vector2 lastActiveVelocity = Vector2.zero;
    float lastAngularVelocity = 0f;

    protected override void AfterEnterNewBound(DisplayBounds bounds)
    {
        if (NumberOfBounds == 1)
        {
            // Turn the rigidbody into dynamic
            body.bodyType = RigidbodyType2D.Dynamic;

            // Apply the last held velocity to the body
            body.velocity = lastActiveVelocity;
            body.angularVelocity = lastAngularVelocity;
        }

        // Change the layer of the rigidbody
        body.gameObject.layer = GetLayer(HighestPriorityBound.Layer).LayerIndex;
    }

    protected override void AfterExitExistingBound(DisplayBounds bounds)
    {
        if (NumberOfBounds > 0)
        {
            // Change the layer of the rigidbody
            body.gameObject.layer = GetLayer(HighestPriorityBound.Layer).LayerIndex;
        }
        else
        {
            // Store velocity
            lastActiveVelocity = body.velocity;
            lastAngularVelocity = body.angularVelocity;

            // Turn the rigidbody into a kinematic
            body.bodyType = RigidbodyType2D.Kinematic;

            // Force the object to stop
            body.velocity = Vector2.zero;
            body.angularVelocity = 0;
        }
    }
}
