using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    public static BrickSpawner Instance;
    public List<Transform> poolObjs = new List<Transform>();

    private void Awake()
    {
        Instance = this;
    }

    public Transform GetObjectFromPool(GameObject prefab, Vector3 spawnPos, Quaternion rotation)
    {
        if (poolObjs.Count > 0)
        {
            Transform obj = poolObjs[0];
            poolObjs.RemoveAt(0);
            obj.SetPositionAndRotation(spawnPos, rotation);
            obj.gameObject.SetActive(true);
            Debug.Log("A");
            return obj;
        }
        else
        {
            Debug.Log("B");
            return Instantiate(prefab, spawnPos, rotation).transform;
        }
    }

    public int GetIndexOfBrickInPool(Transform brick)
    {
        return poolObjs.IndexOf(brick);
    }

    public void RemoveBrickFromPool(int index)
    {
        if (index >= 0 && index < poolObjs.Count)
        {
            poolObjs.RemoveAt(index);
        }
    }

    public void ReturnObjectToPool(Transform obj)
    {
        obj.gameObject.SetActive(false);
        poolObjs.Add(obj);
    }
}
