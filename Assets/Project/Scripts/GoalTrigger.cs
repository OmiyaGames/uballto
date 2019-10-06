using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GoalTrigger : MonoBehaviour
{
    [SerializeField]
    string detectTag = "Player";
    [SerializeField]
    float delaySwitchSceneSeconds = 0.5f;

    bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.CompareTag(detectTag)) && (isTriggered == false))
        {
            StartCoroutine(SwitchScene());
            isTriggered = true;
        }
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(delaySwitchSceneSeconds);
        Debug.Log("Changing scene!");
        OmiyaGames.Singleton.Get<OmiyaGames.Scenes.SceneTransitionManager>().LoadNextLevel();
    }
}
