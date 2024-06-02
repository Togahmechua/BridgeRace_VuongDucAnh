using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] public Renderer meshRenderer;
    [SerializeField] ColorData colorData;
    public ColorByEnum stairEnum;
    // public bool stairColorChanged = false;

    
    protected void Start()
    {
        stairEnum = ColorByEnum.None;
    }

    public virtual void ChangeColor(ColorByEnum  color)
    {
        
        stairEnum = color;
        meshRenderer.material = colorData.GetMaterial(color);
    }
}
