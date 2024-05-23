using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform startPos;
    [SerializeField] private Brick brickPrefab;

    // Start is called before the first frame update
    void Start()
    {
        this.SpawnBrick();
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    private void SpawnBrick()
    {
        for (int x = 0 ; x<10 ;x++)
        {
            x++;
            for (int y = 0; y <10; y++)
            {
                y++;
                Brick brick = Instantiate(brickPrefab);
                brick.transform.position = startPos.position + new Vector3(x,0,-y);
            }
        }
    }
}
