using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailCanvas : UICanvas
{
    public void Restart_Level()
    {

    }

    public void ReturnToMainMenu()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0.1f);
    }
}
