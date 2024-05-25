using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager ins;
    public static LevelManager Ins => ins;
    public ColorData colorData;

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

    public void SetColor(Renderer objectRenderer,int x)
    {
        Color newColor = colorData.GetColorByEnum(x);
        if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor;
        }
    }
}
