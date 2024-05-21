using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : TogaMonoBehaviour
{
    [SerializeField] protected Animator anim;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAnim();
    }

    protected virtual void LoadAnim()
    {
        if (this.anim != null) return;
        this.anim = transform.GetComponentInChildren<Animator>();
    }

    protected virtual void Move()
    {
        //For override
    }

    protected virtual void TakeBrick()
    {
        //For override
    }

    protected virtual void BuildBrick()
    {
        //For override
    }

    protected virtual void Animation()
    {
        //For override
    }
}
