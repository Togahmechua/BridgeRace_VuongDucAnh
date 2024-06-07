using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCanvas : UICanvas
{
    public void SettingButton()
    {
        Time.timeScale = 0;
    }

    public void ContinueButton()
    {
        Time.timeScale = 1;
    }

    public void RestartButton()
    {
        Time.timeScale = 1;
        LevelManager.Ins.StartLevel();
    }
}
