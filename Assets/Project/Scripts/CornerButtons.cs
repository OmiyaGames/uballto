using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OmiyaGames;
using OmiyaGames.Scenes;
using OmiyaGames.Menus;

public class CornerButtons : MonoBehaviour
{
    public void OnRestartClicked()
    {
        Singleton.Get<SceneTransitionManager>().ReloadCurrentScene();
    }

    public void OnMenuClicked()
    {
        Singleton.Get<MenuManager>().PauseMenu.Show();
    }

    public void OnPointerEnter()
    {
        CursorManager.SetClickCursor(true);
    }

    public void OnPointerExit()
    {
        CursorManager.SetClickCursor(false);
    }
}
