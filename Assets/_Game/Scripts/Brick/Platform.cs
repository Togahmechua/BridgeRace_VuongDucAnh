using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform startPos;
    [SerializeField] private Brick brickPrefab;
    [SerializeField] public List<Brick> brickList = new List<Brick>();

    private void Start()
    {
        brickList.AddRange(FindObjectsOfType<Brick>());
    }

    // Clear and Spawn Bricks
    public void SpawnBrick(ColorByEnum[] colors)
    {
        foreach (Brick brick in brickList)
        {
            Destroy(brick.gameObject);
        }
        brickList.Clear();

        int index = 0;
    
        for (int x = 0; x < 10; x++) 
        {
            x++;
            for (int y = 0; y < 10; y++)
            {
                y++;
                Brick brick = Instantiate(brickPrefab);
                brick.ChangeColor(colors[index]);
                brick.transform.position = startPos.position + new Vector3(x * 1.1f, 0, -y * 1.1f); // Adjust spacing as needed
                brick.BrickColor();  
                brickList.Add(brick);

                index = (index + 1) % colors.Length;
            }
        }
    }

    // public List<Brick> GetBricksByColor(ColorByEnum color)
    // {
    //     return brickList.FindAll(brick => brick.BrickColorEnum == color);
    // }

    public void SpawnBrick2(ColorByEnum playerColor,Color playercolor)
    {
        
        Debug.Log("SpawnBrick2");

        
        foreach (Brick brick in brickList)
        {
            if (brick.BrickColorEnum == playerColor)
            {
                brick.gameObject.SetActive(true);
                // brick.SetColor(playerColor,playercolor);
            }
            else
            {
                brick.gameObject.SetActive(false);
            }
        }
    }
}
