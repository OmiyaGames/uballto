using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParent : MonoBehaviour
{
    [System.Serializable]
    public struct ParentTransformPair
    {
        [SerializeField]
        Canvas parent;
        [SerializeField]
        RectTransform objectToMove;

        public void Setup()
        {
            objectToMove.transform.SetParent(parent.transform, true);
        }

        public void MoveTo(Vector3 position)
        {
            objectToMove.position = position;
        }
    }
    [SerializeField]
    DragDrop dragScript;
    [SerializeField]
    RectTransform objectToFollow;
    [SerializeField]
    ParentTransformPair[] objectsToMove;

    private void Start()
    {
        foreach(ParentTransformPair pair in objectsToMove)
        {
            pair.Setup();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (ParentTransformPair pair in objectsToMove)
        {
            pair.MoveTo(objectToFollow.position);
        }
    }
}
