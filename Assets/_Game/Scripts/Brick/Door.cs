using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Platform platformDoor;

    // public void ActiveDoor()
    // {
    //     boxCollider.isTrigger = true;
    //     StartCoroutine(Wait());
    // }

    // private IEnumerator Wait()
    // {
    //     yield return new WaitForSeconds(0.2f);
    //     boxCollider.isTrigger = false;
    // }

    public void Compare(Platform otherPlatform)
    {
        if (platformDoor != otherPlatform)
        {
            boxCollider.isTrigger = false;
            Debug.Log("a");
        }
        else
        {
            boxCollider.isTrigger = true;
            Debug.Log("b");
        }
    }
}
