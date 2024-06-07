using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameCanvas : UICanvas
{
    public void ReturnToMenu()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0.1f);
    }

    public void RestartLevel()
    {
        LevelManager.Ins.ResetMap();
        Close(0.1f);
    }
}
