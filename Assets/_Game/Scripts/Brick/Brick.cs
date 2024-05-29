using System.Collections;
using UnityEngine;

public class Brick : TogaMonoBehaviour
{
    [SerializeField] protected BoxCollider boxCollider;
    [SerializeField] public MeshRenderer meshRenderer;
    [SerializeField] ColorData colorData;
    public ColorByEnum BrickColorEnum { get; private set; }

    // private void Start()
    // {
    //     this.ChangeColor();
    // }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBox();
        this.LoadMeshRenderer();
    }

    protected virtual void LoadBox()
    {
        if (this.boxCollider != null) return;
        this.boxCollider = GetComponent<BoxCollider>();
    }

    protected virtual void LoadMeshRenderer()
    {
        if (this.meshRenderer != null) return;
        this.meshRenderer = GetComponent<MeshRenderer>();
    }

    public virtual void ChangeColor(ColorByEnum  color)
    {
        BrickColorEnum = color;
        meshRenderer.material = colorData.GetMaterial(color);
    }

    public void BrickColor()
    {
        // eColor = LevelManager.Ins.ActiveColor(meshRenderer); // Get and save the color enum
    }

    // public void SetColor(ColorByEnum colorEnum, Color color)
    // {
    //     eColor = colorEnum;
    //     meshRenderer.material.color = color;
    // }

    public void ActiveFalse()
    {
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        StartCoroutine(Wait2Sec());
    }

    private IEnumerator Wait2Sec()
    {
        yield return new WaitForSeconds(2f);
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    public bool IsSameColorAs(ColorByEnum color)
    {
        return BrickColorEnum == color;
    }
}
