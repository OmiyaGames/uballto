using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveVertically : MonoBehaviour
{
    [SerializeField]
    float range = 2f;
    [SerializeField]
    [Range(0f, 1f)]
    float startAt = 0f;
    [SerializeField]
    float speed = 0.5f;

    [Header("Debug")]
    [SerializeField]
    BoxCollider2D box;

    Rigidbody2D body = null;
    Vector2 initialPosition, moveTo = Vector2.zero;
    float timePassed = 0f;

    public Rigidbody2D Body
    {
        get => OmiyaGames.Utility.GetComponentCached(this, ref body);
    }

    public float StartAtRadians
    {
        get => Mathf.Lerp(0, (Mathf.PI * 2f), startAt);
    }

    private void Awake()
    {
        initialPosition = Body.position;
        timePassed = 0f;
        MoveBody();
    }

    private void FixedUpdate()
    {
        timePassed += Time.deltaTime;
        MoveBody();
    }

    private void MoveBody()
    {
        moveTo.y = GetOffsetPosition(timePassed);
        Body.MovePosition(initialPosition + moveTo);
    }

    private float GetOffsetPosition(float time)
    {
        return Mathf.Sin((time * speed) + StartAtRadians) * range;
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 lastMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        Vector2 startPoint = box.offset;
        startPoint.y -= GetOffsetPosition(0);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(startPoint, box.size);

        startPoint = box.offset;
        startPoint.y -= range;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(startPoint, box.size);

        Vector2 endPoint = box.offset;
        endPoint.y += range;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(endPoint, box.size);

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(startPoint, endPoint);

        Gizmos.matrix = lastMatrix;
    }
}
