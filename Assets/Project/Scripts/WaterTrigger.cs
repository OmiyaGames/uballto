using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WaterTrigger : MonoBehaviour
{
    readonly HashSet<ISoundMaker> uniqueSoundMakers = new HashSet<ISoundMaker>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ISoundMaker soundScript = collision.GetComponent<ISoundMaker>();
        if((soundScript != null) && (uniqueSoundMakers.Contains(soundScript) == false))
        {
            soundScript.OnEnterWater(this);
            uniqueSoundMakers.Add(soundScript);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ISoundMaker soundScript = collision.GetComponent<ISoundMaker>();
        if (soundScript != null)
        {
            uniqueSoundMakers.Remove(soundScript);
        }
    }
}
