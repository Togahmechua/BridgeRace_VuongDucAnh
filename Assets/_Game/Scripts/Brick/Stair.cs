using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : TogaMonoBehaviour
{
    [SerializeField] public MeshRenderer meshRenderer;
    public ColorByEnum stairEnum;
    public bool stairColorChanged = false;

    
    protected void Start()
    {
        stairEnum = ColorByEnum.None;
    }

    void Update()
    {
        Check();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadMeshRenderer();
    }


    protected virtual void LoadMeshRenderer()
    {
        if (this.meshRenderer != null) return;
        this.meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Check()
    {
        if (stairEnum != ColorByEnum.None)
        {
            stairColorChanged = true;
        }
    }

    // public void StairColor(Renderer other)
    // {
    //     meshRenderer.material.color = other.material.color;
    // }
}
