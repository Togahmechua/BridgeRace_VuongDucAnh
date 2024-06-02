using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    public Stack<GameObject> stackBricks = new Stack<GameObject>();
    public ColorByEnum CurrentColorEnum { get; private set; }

    [SerializeField] protected Animator anim;
    [SerializeField] public Transform Brickholder;
    [SerializeField] private Brick brickPrefab;
    [SerializeField] public Renderer objectRenderer;
    [SerializeField] ColorData colorData;

    protected Vector3 startHolderPos;
    protected string currentAnimName;


    protected virtual void Start()
    {
        startHolderPos = Brickholder.localPosition;
    }

    public virtual void ChangeColor(ColorByEnum  color)
    {
        CurrentColorEnum = color;
        objectRenderer.material = colorData.GetMaterial(color);
    }

    protected virtual void Move()
    {
        //For override
    }

    public virtual void ChangeAnim(string nameAnim, bool isActive)
    {
        anim.SetBool(nameAnim,isActive);
    }

    protected virtual void AddBrick(Color CurrentColor)
    {
        Brick newBrick = SimplePool.Spawn<Brick>(PoolType.Brick, Brickholder.position, Brickholder.rotation);
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