using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UICanvas
{
    public void Play()
    {
        Time.timeScale = 1;
        UIManager.Ins.OpenUI<SettingCanvas>();
        Close(0.1f);
    }

    public void Quit()
    {
        LevelManager.Ins.Quit();
    }
}
