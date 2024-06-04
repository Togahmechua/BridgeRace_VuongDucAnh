using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager ins;
    public static LevelManager Ins => ins;
    public ColorData colorData;
    public ColorByEnum colorByEnum;
    public Level level;

    [SerializeField] private BotCtrl botPrefab; // Prefab for the objects to be spawned
    [SerializeField] private Transform[] spawnPoints; // Spawn points for the objects
    [SerializeField] private Player player;
    
    [SerializeField] private Transform col1;
    [SerializeField] private Transform col2;
    [SerializeField] private Transform col3;

    private List<BotCtrl> spawnedBots = new List<BotCtrl>(); // List to store spawned bots

    private void Awake()
    {
        LevelManager.ins = this;

        level = FindObjectOfType<Level>();
        FindWinPlatformAndCols();

        Transform[] shuffledSpawnPoints = ShuffleTransforms(spawnPoints);
        ColorByEnum[] randomColors = RandomEnumValues(spawnPoints.Length + 1);
       
        // Currentplatform[0].SpawnBrick(randomColors);
        level.platformList[0].SpawnBrick(randomColors);

        player.ChangeColor(randomColors[0]);
        
        for (int i = 0; i < shuffledSpawnPoints.Length; i++)
        {
            BotCtrl newBot = SimplePool.Spawn<BotCtrl>(botPrefab, shuffledSpawnPoints[i].position, Quaternion.identity);
            newBot.ChangeColor(randomColors[i + 1]); // Start from the second color
            spawnedBots.Add(newBot);
        }
    }

    private void FindWinPlatformAndCols()
    {
        GameObject winPlatform = GameObject.FindGameObjectWithTag("Win");

        if (winPlatform != null)
        {
            col1 = winPlatform.transform.Find("Col1");
            col2 = winPlatform.transform.Find("Col2");
            col3 = winPlatform.transform.Find("Col3");

            if (col1 == null || col2 == null || col3 == null)
            {
                Debug.LogError("Columns Col1, Col2, and/or Col3 not found in WinPlatform.");
            }
        }
        else
        {
            Debug.LogError("WinPlatform not found in the scene.");
        }
    }

    public ColorByEnum[] RandomEnumValues(int count)
    {
        Array enumValues = Enum.GetValues(typeof(ColorByEnum));
        List<ColorByEnum> enumList = new List<ColorByEnum>();

        foreach (ColorByEnum value in enumValues)
        {
            if (value != ColorByEnum.None)
            {
                enumList.Add(value);
            }
        }

        if (count > enumList.Count)
        {
            Debug.LogError("Not enough enum values to select.");
            return null;
        }

        for (int i = 0; i < enumList.Count; i++)
        {
            ColorByEnum temp = enumList[i];
            int randomIndex = UnityEngine.Random.Range(i, enumList.Count);
            enumList[i] = enumList[randomIndex];
            enumList[randomIndex] = temp;
        }
        return enumList.GetRange(0, count).ToArray();
    }

    public Transform[] ShuffleTransforms(Transform[] transforms)
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            Transform temp = transforms[i];
            int randomIndex = UnityEngine.Random.Range(i, transforms.Length);
            transforms[i] = transforms[randomIndex];
            transforms[randomIndex] = temp;
        }
        return transforms;
    }

    public void MovePlayerAndBotToWinPos()
    {
        if (col1 == null || col2 == null || col3 == null)
        {
            Debug.LogError("Columns Col1, Col2, and/or Col3 not assigned.");
            return;
        }

        player.transform.position = col1.position;
        player.transform.rotation = Quaternion.Euler(new Vector3(0f,180f,0f));

        // Create an array with Col2 and Col3 and shuffle it
        Transform[] cols = new Transform[] { col2, col3 };
        cols = ShuffleTransforms(cols);

        // Assign the positions to the bots
        for (int i = 0; i < spawnedBots.Count && i < cols.Length; i++)
        {
            spawnedBots[i].transform.position = cols[i].position;
            spawnedBots[i].transform.rotation = Quaternion.Euler(new Vector3(0f,180f,0f));
            spawnedBots[i].TransToWinState();
        }
    }
}
