using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class BotCtrl : Character
{
    public NavMeshAgent agent;
    public Transform finishPos;
    public ColorByEnum botColorEnum;
    private List<Brick> bricksByColor;
    [SerializeField] private Transform rayPos;
    private Player player;
    [SerializeField] private Platform currentPlatform;
    [SerializeField] private float raycastDistance;
    private float originalMoveSpeed;

    protected override void Start()
    {
        base.Start();

        if (LevelManager.Ins.Currentplatform == null || LevelManager.Ins.Currentplatform.Length == 0)
        {
            Debug.LogError("Platform list is not assigned or is empty.");
            return;
        }

        // Gán currentPlatform từ platformList
        currentPlatform = LevelManager.Ins.Currentplatform[0];
        rayPos = transform.Find("rayPos");
        player = FindObjectOfType<Player>();

        GameObject finishPosObj = GameObject.Find("FinishPos");
        if (finishPosObj != null)
        {
            finishPos = finishPosObj.transform;
        }
        else
        {
            Debug.LogError("Game object with the name 'FinishPos' not found.");
        }
        botColorEnum = CurrentColorEnum;
        bricksByColor = currentPlatform.GetBricksByColor(botColorEnum);

        MoveToNextBrick();
    }

    private void Update()
    {
        RaycastCheck();
        if (agent.remainingDistance < 0.5f && agent.destination != finishPos.position)
        {
            MoveToNextBrick();
        }
    }

    protected void MoveToNextBrick()
    {
        if (stackBricks.Count < RandomBrickCount())
        {
            if (bricksByColor.Count == 0)
            {
                bricksByColor = currentPlatform.GetBricksByColor(botColorEnum);
            }

            if (bricksByColor.Count > 0)
            {
                Brick randomBrick = bricksByColor[Random.Range(0, bricksByColor.Count)];
                bricksByColor.Remove(randomBrick);
                MoveToBrickWithSameColor(randomBrick.transform);
            }
            else
            {
                // No bricks left to collect, stay in place
                agent.SetDestination(transform.position);
                anim.SetBool("IsRunning", false);
            }
        }
        else
        {
            agent.SetDestination(finishPos.position);
            anim.SetBool("IsRunning", true);
        }
    }

    protected void MoveToBrickWithSameColor(Transform pos)
    {
        agent.SetDestination(pos.position);
        anim.SetBool("IsRunning", true);
    }

    private void RaycastCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayPos.position, Vector3.down, out hit, raycastDistance))
        {
            Stair stair = Cache.GetStair(hit.collider);
            if (stair != null)
            {
                if (agent.velocity.z > 0)
                {
                    if (stair.stairEnum == ColorByEnum.None || stair.stairEnum != this.CurrentColorEnum)
                    {
                        stair.ChangeColor(CurrentColorEnum);
                        RemoveBrick();

                        // If stack is empty, find the next brick
                        if (stackBricks.Count == 0)
                        {
                            MoveToNextBrick();
                        }
                    }
                }
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Brick otherBrick = Cache.GetBrick(other);
        if (otherBrick != null && otherBrick.BrickColorEnum == botColorEnum)
        {
            this.AddBrick(objectRenderer.material.color);
            otherBrick.ActiveFalse();

            // Check if stack is less than 5 and we need to continue collecting bricks
            if (stackBricks.Count < 5)
            {
                MoveToNextBrick();
            }
        }
    }

    private int RandomBrickCount()
    {
        int randBrick = Random.Range(5,8);
        return randBrick;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawRay(rayPos.position, Vector3.down * raycastDistance);
        Gizmos.color = Color.red;
    }
}
