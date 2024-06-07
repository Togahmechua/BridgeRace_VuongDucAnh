using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : UICanvas
{
    public void Next_Level()
    {
        LevelManager.Ins.NextLevel();
        Close(0.1f);
    }

    public void Restart_Level()
    {
        LevelManager.Ins.ResetMap();
        Close(0.1f);
    }
}
