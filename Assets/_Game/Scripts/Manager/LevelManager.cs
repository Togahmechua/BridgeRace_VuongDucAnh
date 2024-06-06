using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager ins;
    public static LevelManager Ins => ins;
    public ColorData colorData;
    public ColorByEnum colorByEnum;
    public Level level;
    public List<Level> levelList = new List<Level>();
    public int CurLevel;

    [SerializeField] private BotCtrl botPrefab; 
    [SerializeField] private Transform[] spawnPoints; 
    [SerializeField] private Player player;
    
   
    public Transform playerPos;

    private List<BotCtrl> spawnedBots = new List<BotCtrl>(); 

    private void Awake()
    {
        LevelManager.ins = this;
        Time.timeScale = 0;
        CurLevel = PlayerPrefs.GetInt("CurrentLevel", 0); 
        LoadLevel();
        StartLevel();
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
        if (level.col1 == null || level.col2 == null || level. col3 == null)
        {
            Debug.LogError("Columns Col1, Col2, and/or Col3 not assigned.");
            return;
        }

        player.transform.position = level.col1.position;
        player.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

        Transform[] cols = new Transform[] { level.col2, level.col3 };
        cols = ShuffleTransforms(cols);

        for (int i = 0; i < spawnedBots.Count && i < cols.Length; i++)
        {
            spawnedBots[i].transform.position = cols[i].position;
            spawnedBots[i].transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            spawnedBots[i].rb.useGravity = true;
            spawnedBots[i].TransToWinState();
        }
    }

    private void StartLevel()
    {
        Transform[] shuffledSpawnPoints = ShuffleTransforms(spawnPoints);
        ColorByEnum[] randomColors = RandomEnumValues(spawnPoints.Length + 1);
       
        level.platformList[0].SpawnBrick(randomColors);

        player.ChangeColor(randomColors[0]);
        
        spawnedBots.Clear();

        for (int i = 0; i < shuffledSpawnPoints.Length; i++)
        {
            BotCtrl newBot = SimplePool.Spawn<BotCtrl>(botPrefab, shuffledSpawnPoints[i].position, Quaternion.identity);
            newBot.ChangeColor(randomColors[i + 1]); // Start from the second color
            newBot.OnInit();
            spawnedBots.Add(newBot);
        }
    }

    public void LoadLevel()
    {
        if (level != null)
        {
            Destroy(level.gameObject);
        }
        
        level = Instantiate(levelList[CurLevel], transform);
        level = FindObjectOfType<Level>();
        player.OnInit();

        for (int i = 0; i < spawnedBots.Count; i++)
        {
            spawnedBots[i].OnInit();
        }
    }

    public void NextLevel()
    {
        if (level != null)
        {
            Destroy(level.gameObject);
        }

        for (int i = 0; i < spawnedBots.Count; i++)
        {
            SimplePool.Despawn(spawnedBots[i]);
        }
        // spawnedBots.Clear();

        level = Instantiate(levelList[CurLevel], transform);
        level = FindObjectOfType<Level>();
        player.OnInit();
        StartLevel();
    }

    public void NewGame()
    {
        CurLevel = 1;
    }

    public void ResetMap()
    {
        CurLevel--;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
