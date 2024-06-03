using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class BotCtrl : Character
{
    public NavMeshAgent agent;
    public Transform finishPos;
    public ColorByEnum botColorEnum;

    private List<Brick> bricksByColor;
    private Player player;
    private float originalMoveSpeed;
    private int currentPlatformIndex = 0;
    public IState<BotCtrl> currentState;
    public IdleState idleState;
    public FindBrickState findBrickState;
    public BuildBridgeState buildBridgeState;
    [SerializeField] private Transform rayPos;
    [SerializeField] private Platform BotPlatform;
    [SerializeField] private float raycastDistance;
    [SerializeField] private bool isWinning;
    public bool IsHavingNearestBrick;
    public int BricksToFind;

    private Brick targetBrick;

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

        // Initialize states
        idleState = new IdleState();
        findBrickState = new FindBrickState();
        buildBridgeState = new BuildBridgeState();
        // Start in Idle state
        TransitionToState(idleState);

        BricksToFind = RandomBrickCount();
    }

    private void Update()
    {
        RaycastCheck();
        currentState?.OnExecute(this);
    }

    public void TransitionToState(IState<BotCtrl> newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }

    public Brick FindNearestBrick()
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

    public void MoveToNextBrick()
    {
        targetBrick = FindNearestBrick();
        if (targetBrick != null)
        {
            MoveToBrickWithSameColor(targetBrick.transform);
        }
        else
        {
            agent.SetDestination(transform.position);
            ChangeAnim("IsRunning", false);
        }
    }

    public void CheckArrival()
    {
        if (targetBrick != null && Vector3.Distance(transform.position, targetBrick.transform.position) < 0.5f)
        {
            targetBrick = null;
            TransitionToState(findBrickState);
        }
    }

    public void GoToFinishPoint()
    {
        agent.SetDestination(finishPos.position);
        ChangeAnim("IsRunning", true);
    }

    protected void MoveToBrickWithSameColor(Transform pos)
    {
        if (isWinning) return;
        agent.SetDestination(pos.position);
        ChangeAnim("IsRunning", true);
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
                            // MoveToNextBrick();
                            TransitionToState(findBrickState);
                        }
                    }
                }
            }
        }
    }

    public int RandomBrickCount()
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
                TransitionToState(findBrickState);
            }
        }

        Door door = Cache.GetDoor(other);
        if (door != null && agent.velocity.z > 0)
        {
            Debug.Log(other.gameObject.name);
            this.ClearAllBrick();
            BotPlatform = door.platformDoor;
            this.transform.position += new Vector3(0, 0, 1f);
            BotPlatform.SpawnBrick2(this, 5);
            bricksByColor = BotPlatform.GetBricksByColor(botColorEnum);
            TransitionToState(findBrickState);
        }

        WinPlatform winPlatform = Cache.GetWinPlatform(other);
        if (winPlatform != null && agent.velocity.z > 0)
        {
            anim.SetTrigger("IsWinning");
            isWinning = true;
            Debug.Log(this.gameObject.name + " win");
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawRay(rayPos.position, Vector3.down * raycastDistance);
        Gizmos.color = Color.red;
    }

    internal bool IsEnoughBrickToBuild() => stackBricks.Count >= BricksToFind;
}
