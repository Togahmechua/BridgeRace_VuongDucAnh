using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.TAG_PLayer))
        {
            BrickSpawner.Instance.Despawn(transform);
        }
    }
}
