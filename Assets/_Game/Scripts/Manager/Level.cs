using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Platform[] platformList;
    public Transform col1;
    public Transform col2;
    public Transform col3;
    public Transform finishPos;


    // private void Start()
    // {
    //     FindWinPlatformAndCols();
    //     LoadFinishPos();
    // }

    // private void FindWinPlatformAndCols()
    // {
    //     GameObject winPlatform = GameObject.FindGameObjectWithTag("Win");

    //     if (winPlatform != null)
    //     {
    //         col1 = winPlatform.transform.Find("Col1");
    //         col2 = winPlatform.transform.Find("Col2");
    //         col3 = winPlatform.transform.Find("Col3");

    //         if (col1 == null || col2 == null || col3 == null)
    //         {
    //             Debug.LogError("Columns Col1, Col2, and/or Col3 not found in WinPlatform.");
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError("WinPlatform not found in the scene.");
    //     }
    // }

    // private void LoadFinishPos()
    // {
    //     GameObject finishPosObj = GameObject.Find("FinishPos");
    //     if (finishPosObj != null)
    //     {
    //         finishPos = finishPosObj.transform;
    //     }
    //     else
    //     {
    //         Debug.LogError("Game object with the name 'FinishPos' not found.");
    //     }
    // }
}
