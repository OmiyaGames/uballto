using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParent : MonoBehaviour
{
    [SerializeField]
    RectTransform objectToMove;
    [SerializeField]
    RectTransform objectToFollow;
    [SerializeField]
    Canvas parent;

    private void Start()
    {
        objectToMove.transform.SetParent(parent.transform, true);
    }

    // Update is called once per frame
    void Update()
    {
        objectToMove.position = objectToFollow.position;
    }
}
