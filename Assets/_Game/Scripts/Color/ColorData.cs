using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Color", order = 1)]
public class ColorData : ScriptableObject
{
    public List<Color> colors;

    public Color GetColorByEnum(int index)
    {
        for (int i = 0; i < colors.Count; i++)
        {
            if (i == index)
            {
                return colors[i];
            }
        }
        return Color.white;
    }
}
