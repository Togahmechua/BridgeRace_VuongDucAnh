using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailCanvas : UICanvas
{
    public void Restart_Level()
    {
        Time.timeScale = 1;
        LevelManager.Ins.StartLevel();
        Close(0.1f);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("ReturnToMainMenu");
        Time.timeScale = 1;
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0.1f);
    }
}
