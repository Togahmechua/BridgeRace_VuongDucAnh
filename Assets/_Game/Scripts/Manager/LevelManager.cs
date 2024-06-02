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
    [SerializeField] public Platform[] Currentplatform;
    [SerializeField] private BotCtrl botPrefab; // Prefab for the objects to be spawned
    [SerializeField] private Transform[] spawnPoints; // Spawn points for the objects
    [SerializeField] private Player player;

    public int currentPlatformIndex = 0; // Make currentPlatformIndex global

    private void Awake()
    {
        LevelManager.ins = this;
        
        Transform[] shuffledSpawnPoints = ShuffleTransforms(spawnPoints);
        ColorByEnum[] randomColors = RandomEnumValues(spawnPoints.Length + 1);
       
        Currentplatform[0].SpawnBrick(randomColors);

        player.ChangeColor(randomColors[0]);
        
        for (int i = 0; i < shuffledSpawnPoints.Length; i++)
        {
            BotCtrl newBot = Instantiate(botPrefab, shuffledSpawnPoints[i].position, Quaternion.identity);
            newBot.ChangeColor(randomColors[i + 1]); // Start from the second color
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

    public Platform GetNextPlatform(ref int currentIndex)
    {
        currentIndex++;
        if (currentIndex >= Currentplatform.Length)
        {
            currentIndex = 0; // Wrap around if at the end of the array
        }

        return Currentplatform[currentIndex];
    }
}
