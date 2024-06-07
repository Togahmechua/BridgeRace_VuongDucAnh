using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UICanvas
{
    public void Play()
    {
        if (LevelManager.Ins.CurLevel >= LevelManager.Ins.levelList.Count)
        {
            LevelManager.Ins.CurLevel--;
        }
        Time.timeScale = 1;
        UIManager.Ins.OpenUI<SettingCanvas>();
        Close(0.1f);
        LevelManager.Ins.StartLevel();
    }

    public void Quit()
    {
        LevelManager.Ins.Quit();
    }
}
