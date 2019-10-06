using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallRoller : MonoBehaviour
{
    [SerializeField]
    float stillTorque = 10f;
    [SerializeField]
    Vector2 stillForce;

    Rigidbody2D body = null;

    public Rigidbody2D Body
    {
        get => OmiyaGames.Utility.GetComponentCached(this, ref body);
    }


    void FixedUpdate()
    {
        if(Body.bodyType == RigidbodyType2D.Dynamic)
        {
            Body.AddTorque(stillTorque * Time.deltaTime, ForceMode2D.Force);
            Body.AddForce(stillForce * Time.deltaTime, ForceMode2D.Force);
        }
    }
}
