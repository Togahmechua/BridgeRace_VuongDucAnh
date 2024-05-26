using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager ins;
    public static LevelManager Ins => ins;
    public ColorData colorData;
    [SerializeField] private BotCtrl objectPrefab; // Prefab for the objects to be spawned
    [SerializeField] private Transform[] spawnPoints; // Spawn points for the objects

    private void Awake()
    {
        LevelManager.ins = this;
        
    }

    public EColor.ColorByEnum ActiveColor(Renderer objectRenderer)
    {
        EColor.ColorByEnum randomColorEnum = GetRandomEnumValue<EColor.ColorByEnum>();
        Color newColor = colorData.GetColorByEnum((int)randomColorEnum);
        if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor;
        }
        return randomColorEnum;
    }
    
    private T GetRandomEnumValue<T>()
    {
        System.Array values = System.Enum.GetValues(typeof(T));
        int randomIndex = Random.Range(1, values.Length);
        return (T)values.GetValue(randomIndex);
    }

    public void SetColor(Renderer objectRenderer, int x)
    {
        Color newColor = colorData.GetColorByEnum(x);
        if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor;
        }
    }

    public void SpawnObjectsWithDifferentColors(EColor.ColorByEnum playerColor)
    {
        HashSet<EColor.ColorByEnum> usedColors = new HashSet<EColor.ColorByEnum>();
        usedColors.Add(playerColor);

        for (int i = 0; i < 2; i++)
        {
            EColor.ColorByEnum newColorEnum = GetRandomEnumValue<EColor.ColorByEnum>();
            while (usedColors.Contains(newColorEnum))
            {
                newColorEnum = GetRandomEnumValue<EColor.ColorByEnum>();
            }
            usedColors.Add(newColorEnum);

            BotCtrl newObject = Instantiate(objectPrefab, spawnPoints[i].position, Quaternion.identity);
            SetColor(newObject.objectRenderer, (int)newColorEnum);
        }
    }
}
