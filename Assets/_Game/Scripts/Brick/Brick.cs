using System.Collections;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] protected BoxCollider boxCollider;
    [SerializeField] public MeshRenderer meshRenderer;
    [SerializeField] ColorData colorData;
    public ColorByEnum BrickColorEnum { get; private set; }

    public virtual void ChangeColor(ColorByEnum color)
    {
        BrickColorEnum = color;
        meshRenderer.material = colorData.GetMaterial(color);
    }

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

    public bool isActiveBrick()
    {
        return meshRenderer != null && boxCollider != null && meshRenderer.enabled && boxCollider.enabled;
    }
}
