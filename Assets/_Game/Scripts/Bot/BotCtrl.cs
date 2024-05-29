using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class BotCtrl : Character
{
    public NavMeshAgent agent;
    public Transform finishPos;
    public ColorByEnum botColorEnum;
    private List<Brick> bricksByColor;
    private Platform currentPlatform;
    private Player player;

    protected override void Start()
    {
        base.Start();

        currentPlatform = FindObjectOfType<Platform>();
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

        // botColorEnum = LevelManager.Ins.ActiveColor(null);
        // bricksByColor = currentPlatform.GetBricksByColor(botColorEnum);
        

        // MoveToNextBrick();
    }

    // private void Update()
    // {
    //     if (agent.remainingDistance < 0.5f)
    //     {
    //         MoveToNextBrick();
    //     }
    // }
    
    // private void Check()
    // {
    //     botColorEnum = LevelManager.Ins.ActiveColor(this.objectRenderer);
    //     if (botColorEnum == player.CurrentColorEnum)
    //     {
    //         Check();
    //     }
    // }

    // protected void MoveToNextBrick()
    // {
    //     if (bricksByColor.Count == 0)
    //     {
    //         agent.SetDestination(transform.position);
    //         anim.SetBool("IsRunning", false);
    //         return;
    //     }

    //     Brick randomBrick = bricksByColor[Random.Range(0, bricksByColor.Count)];
    //     bricksByColor.Remove(randomBrick);

    //     MoveToBrickWithSameColor(randomBrick.transform);
    // }

    // protected void MoveToBrickWithSameColor(Transform pos)
    // {
    //     agent.SetDestination(pos.position);
    //     anim.SetBool("IsRunning", true);
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     Brick otherBrick = Cache.GetBrick(other);
    //     if (otherBrick != null && otherBrick.BrickColorEnum == botColorEnum)
    //     {
    //         this.AddBrick(objectRenderer.material.color);
    //         otherBrick.ActiveFalse();

    //         MoveToNextBrick();
    //     }
    // }
}
