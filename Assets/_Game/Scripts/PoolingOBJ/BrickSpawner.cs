using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : Pool
{
    private static Pool instance;
    public static Pool Instance { get => instance;}
    public static string brick1 = "BrickPrefab";

    protected override void Awake()
    {
        base.Awake();
        if (BrickSpawner.instance != null)
        {
            Debug.LogError("There are 2 Pool exist");
        }
        BrickSpawner.instance = this;
    }
}
