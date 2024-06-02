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
    [SerializeField] private Platform BotPlatform;
    [SerializeField] private float raycastDistance;
    private float originalMoveSpeed;
    private int currentPlatformIndex = 0;
    [SerializeField] private bool isWinning = false;

    protected override void Start()
    {
        base.Start();
        
        if (LevelManager.Ins.Currentplatform == null || LevelManager.Ins.Currentplatform.Length == 0)
        {
            Debug.LogError("Platform list is not assigned or is empty.");
            return;
        }

        BotPlatform = LevelManager.Ins.Currentplatform[currentPlatformIndex];
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
    }

    private void Update()
    {
        // RaycastCheck();
        // if (agent.remainingDistance < 0.5f && agent.destination != finishPos.position)
        // {
        //     if (isWinning == false)
        //     {
        //         MoveToNextBrick();
        //     }
        // }
    }

    private Brick FindNearestBrick()
    {
        List<Brick> bricks = BotPlatform.GetBricksByColor(botColorEnum);
        Brick nearestBrick = null;
        float minDistance = float.MaxValue;

        foreach (Brick brick in bricks)
        {
            if (brick.isActiveBrick() == true)
            {
                float distance = Vector3.Distance(transform.position, brick.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestBrick = brick;
                }
            }
        }

        return nearestBrick;
    }

    protected void MoveToNextBrick()
    {
        if (stackBricks.Count < RandomBrickCount())
        {
            Brick nearestBrick = FindNearestBrick();
            if (nearestBrick != null)
            {
                MoveToBrickWithSameColor(nearestBrick.transform);
            }
            else
            {
                agent.SetDestination(transform.position);
                ChangeAnim("IsRunning",false);
            }
        }
        else
        {
            agent.SetDestination(finishPos.position);
            ChangeAnim("IsRunning",true);
        }
    }

    protected void MoveToBrickWithSameColor(Transform pos)
    {
        agent.SetDestination(pos.position);
        ChangeAnim("IsRunning",true);
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

                        if (stackBricks.Count == 0)
                        {
                            MoveToNextBrick();
                        }
                    }
                }
            }
        }
    }

    private int RandomBrickCount()
    {
        return Random.Range(5, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        Brick otherBrick = Cache.GetBrick(other);
        if (otherBrick != null && otherBrick.BrickColorEnum == botColorEnum)
        {
            this.AddBrick(objectRenderer.material.color);
            otherBrick.ActiveFalse();

            if (stackBricks.Count < 5)
            {
                MoveToNextBrick();
            }
        }

        Door door = Cache.GetDoor(other);
        if (door != null && agent.velocity.z > 0)
        {
            Debug.Log(other.gameObject.name);
            this.ClearAllBrick();
            // BotPlatform = LevelManager.Ins.Currentplatform[currentPlatformIndex++];
            BotPlatform = door.platformDoor;
            this.transform.position += new Vector3(0, 0, 1f);
            BotPlatform.SpawnBrick2(this, 5);
            bricksByColor = BotPlatform.GetBricksByColor(botColorEnum);
            MoveToNextBrick();
        }

        WinPlatform winPlatform = Cache.GetWinPlatform(other);
        if (winPlatform != null && agent.velocity.z > 0)
        {
            anim.SetTrigger("IsWinning");
            isWinning = true;
            Debug.Log(this .gameObject.name + "win");
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawRay(rayPos.position, Vector3.down * raycastDistance);
        Gizmos.color = Color.red;
    }
}
