using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ChangeBallDirection : MonoBehaviour
{
    [SerializeField]
    string ballTag = "Player";
    [SerializeField]
    bool moveRight = true;

    bool? lastMoveRight = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(ballTag))
        {
            BallRoller roller = collision.GetComponent<BallRoller>();
            if(roller)
            {
                lastMoveRight = roller.MoveRight;
                roller.MoveRight = moveRight;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag(ballTag)) && (lastMoveRight.HasValue))
        {
            BallRoller roller = collision.GetComponent<BallRoller>();
            if (roller)
            {
                roller.MoveRight = lastMoveRight.Value;
                lastMoveRight = null;
            }
        }
    }
}
