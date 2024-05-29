using System.Collections.Generic;
using UnityEngine;

public class Character : TogaMonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] public Transform Brickholder;
    [SerializeField] private Brick brickPrefab;
    [SerializeField] public Renderer objectRenderer;
    public Stack<GameObject> stackBricks = new Stack<GameObject>();
    protected Vector3 startHolderPos;
    // protected Color CurrentColor { get; set; }

    [SerializeField] ColorData colorData;
    public ColorByEnum CurrentColorEnum { get; private set; }

    protected virtual void Start()
    {
        startHolderPos = Brickholder.localPosition;
    }

    public virtual void ChangeColor(ColorByEnum  color)
    {
        CurrentColorEnum = color;
        objectRenderer.material = colorData.GetMaterial(color);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAnim();
        this.LoadHolder();
    }

    protected virtual void LoadAnim()
    {
        if (this.anim != null) return;
        this.anim = transform.GetComponentInChildren<Animator>();
    }

    protected virtual void LoadHolder()
    {
        if (this.Brickholder != null) return;
        this.Brickholder = transform.Find("Holder");
    }

    protected virtual void Move()
    {
        //For override
    }

    protected virtual void AddBrick(Color CurrentColor)
    {
        Brick newBrick = Instantiate(brickPrefab, Brickholder.position, Brickholder.rotation);
        newBrick.transform.SetParent(transform);
        Brickholder.transform.localPosition += new Vector3(0, 0.2f, 0);
        stackBricks.Push(newBrick.gameObject);
        newBrick.enabled = false;
        newBrick.meshRenderer.material.color = CurrentColor;
    }

    protected virtual void RemoveBrick()
    {
        if (stackBricks.Count <= 0) return;

        Destroy(stackBricks.Pop());
        Brickholder.transform.localPosition -= new Vector3(0, 0.2f, 0);
    }

    protected virtual void ClearAllBrick()
    {
        if (stackBricks.Count == 0) return;

        while (stackBricks.Count > 0)
        {
            Destroy(stackBricks.Pop());
            Brickholder.localPosition = startHolderPos;
        }
    }
}