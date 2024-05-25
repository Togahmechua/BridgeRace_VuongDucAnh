using System.Collections;
using UnityEngine;

public class Brick : TogaMonoBehaviour
{
    // [SerializeField] public Renderer objectRenderer;
    [SerializeField] protected BoxCollider boxCollider;
    [SerializeField] public MeshRenderer meshRenderer;
    public EColor.ColorByEnum eColor;

    private void Start()
    {
        this.BrickColor();
    }

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

    public void BrickColor()
    {
        eColor = LevelManager.Ins.ActiveColor(meshRenderer); // Get and save the color enum
    }

    public void ActiveFalse()
    {
        // gameObject.SetActive(false);
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        StartCoroutine(Wait2Sec());
    }

    private IEnumerator Wait2Sec()
    {
        yield return new WaitForSeconds(2f);
        // gameObject.SetActive(true);
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
    }
}
