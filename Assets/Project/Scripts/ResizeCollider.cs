using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ResizeCollider : MonoBehaviour
{
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    BoxCollider2D boxCollider;

#if UNITY_EDITOR
    void Update()
    {
        if (rectTransform && boxCollider)
        {
            boxCollider.size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        }
    }
#endif
}
