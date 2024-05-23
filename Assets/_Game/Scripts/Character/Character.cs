using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : TogaMonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected Transform holder;

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
        if (this.holder != null) return;
        this.holder = transform.Find("BrickHolder");
    }

    protected virtual void Move()
    {
        //For override
    }

    protected virtual void AddBrick()
    {
        //For override
    }

    protected virtual void RemoveBrick()
    {
        //For override
    }

    protected virtual void Animation()
    {
        //For override
    }
}
