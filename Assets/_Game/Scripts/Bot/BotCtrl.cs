using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCtrl : TogaMonoBehaviour
{
    [SerializeField] public Renderer objectRenderer;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRenderer();
    }

    private void LoadRenderer()
    {
        if (this.objectRenderer != null) return;
        this.objectRenderer = GetComponentInChildren<Renderer>();
    }
}
