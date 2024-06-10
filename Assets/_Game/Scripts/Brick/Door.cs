using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
    public Platform platformDoor;

    private void OnTriggerEnter(Collider other)
    {
        Character character = Cache.GetCharacter(other);
        if (character != null)
        {
            platformDoor.SpawnBrick2(character, 8);
            StartCoroutine(DeActiveDoor());
        }
    }

    private IEnumerator DeActiveDoor()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(true);
    }
}
