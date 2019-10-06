using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ToggleActivation : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D body;

    Vector2 lastActiveVelocity = Vector2.zero;
    float lastAngularVelocity = 0f;
    readonly HashSet<DisplayBounds> inBounds = new HashSet<DisplayBounds>();

    public void EnterBounds(DisplayBounds bounds)
    {
        if (inBounds.Count == 0)
        {
            // Turn the rigidbody into dynamic
            body.bodyType = RigidbodyType2D.Dynamic;

            // Apply the last held velocity to the body
            body.velocity = lastActiveVelocity;
            body.angularVelocity = lastAngularVelocity;
        }
        inBounds.Add(bounds);
    }

    public void ExitBounds(DisplayBounds bounds)
    {
        if ((inBounds.Remove(bounds)) && (inBounds.Count == 0))
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
