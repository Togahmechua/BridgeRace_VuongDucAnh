using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager ins;
    public static LevelManager Ins => ins;
    public ColorData colorData;
    // public Renderer objectRenderer;

    private void Awake()
    {
        LevelManager.ins = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveColor(Renderer objectRenderer)
    {
        EColor.ColorByEnum randomColorEnum = GetRandomEnumValue<EColor.ColorByEnum>();
        Color newColor = colorData.GetColorByEnum((int)randomColorEnum);
        if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor;
        }
    }

    private T GetRandomEnumValue<T>()
    {
        System.Array values = System.Enum.GetValues(typeof(T));
        int randomIndex = Random.Range(1, values.Length);
        return (T)values.GetValue(randomIndex);
    }
}
