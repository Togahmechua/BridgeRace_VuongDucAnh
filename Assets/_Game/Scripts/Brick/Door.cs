using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : TogaMonoBehaviour
{
    
    [SerializeField] private BoxCollider boxCollider;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBoxCollider();
    }

    protected virtual void LoadBoxCollider()
    {
        if (this.boxCollider != null) return;
        this.boxCollider = GetComponent<BoxCollider>();
    }
    public void ActiveDoor()
    {
        boxCollider.isTrigger = true;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        boxCollider.isTrigger = false;
    }
}
