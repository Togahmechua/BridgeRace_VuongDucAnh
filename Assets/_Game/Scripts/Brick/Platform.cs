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
        if (brickList.Count > 0) {
            foreach (Brick brick in brickList)
            {
                SimplePool.Despawn(brick);
            }
            brickList.Clear();
        }

        List<Vector3> positions = new List<Vector3>();
        int index = 0;

        for (int x = 0; x < 10; x++) 
        {
            x++;
            for (int y = 0; y < 10; y++)
            {
                y++;
                positions.Add(startPos.position + new Vector3(x * 1.1f, 0, -y * 1.1f));
            }
        }

        // Shuffle the positions
        Shuffle(positions);

        foreach (Vector3 position in positions)
        {
            Brick brick = SimplePool.Spawn<Brick>(brickPrefab, transform.position, Quaternion.identity);
            brick.ChangeColor(colors[index]);
            brick.transform.position = position;
            brickList.Add(brick);
            brick.transform.SetParent(transform);
            index = (index + 1) % colors.Length;
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
                Brick brick = SimplePool.Spawn<Brick>(brickPrefab, transform.position, Quaternion.identity);
                brick.ChangeColor(character.CurrentColorEnum);
                brick.transform.position = position;
                brickList.Add(brick);
                occupiedPositions.Add(position);
                spawnedCount++;
                brick.transform.SetParent(transform);
            }
        }
    }

    // Fisher-Yates Shuffle
    private void Shuffle(List<Vector3> positions)
    {
        for (int i = positions.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector3 temp = positions[i];
            positions[i] = positions[j];
            positions[j] = temp;
        }
    }
}
