using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : UICanvas
{
    public void Next_Level()
    {
        LevelManager.Ins.LoadLevel();
        Close(0.1f);
    }

    public void Restart_Level()
    {
        
    }
}
