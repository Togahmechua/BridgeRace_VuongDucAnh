using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform startPos;
    [SerializeField] private Brick brickPrefab;
    [SerializeField] public List<Brick> brickList = new List<Brick>();
    private List<Vector3> occupiedPositions = new List<Vector3>();

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
                // Brick brick = SimplePool.Spawn<Brick>(PoolType.Brick);
                // Brick brick = SimplePool.Spawn<Brick>(PoolType.Brick, transform.position, Quaternion.identity);
                brick.ChangeColor(colors[index]);
                brick.transform.position = startPos.position + new Vector3(x * 1.1f, 0, -y * 1.1f);
                // brick.BrickColor();  
                brickList.Add(brick);
                brick.transform.SetParent(transform);
                index = (index + 1) % colors.Length;
            }
        }
    }

    public List<Brick> GetBricksByColor(ColorByEnum colorEnum)
    {
        return brickList.FindAll(brick => brick.BrickColorEnum == colorEnum);
    }

    public void SpawnBrick2(Character character, int brickCount)
    {
        int spawnedCount = 0;
        while (spawnedCount < brickCount)
        {
            int x = Random.Range(0, 10);
            int y = Random.Range(0, 10);
            Vector3 position = startPos.position + new Vector3(x * 1.1f, 0, -y * 1.1f);

            if (!occupiedPositions.Contains(position))
            {
                Brick brick = Instantiate(brickPrefab);
                brick.ChangeColor(character.CurrentColorEnum);
                brick.transform.position = position;
                brickList.Add(brick);
                occupiedPositions.Add(position);
                spawnedCount++;
            }
        }
    }
}
