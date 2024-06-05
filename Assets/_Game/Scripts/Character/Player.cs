using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : Character
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FixedJoystick joyStick;
    [SerializeField] private float moveSpeed;
    // [SerializeField] private Brick brickPrefab;
    // private Stack<GameObject> stackBricks = new Stack<GameObject>();
    [SerializeField] private Transform rayPos;
    [SerializeField] private float raycastDistance;
    [SerializeField] private Platform playerPlatform;
    private float originalMoveSpeed;
    private int currentPlatformIndex = 0;
    [SerializeField] private bool isWinning = false;


    protected override void Start()
    {
        base.Start();
        playerPlatform = LevelManager.Ins.level.platformList[currentPlatformIndex];
        // this.ChangeColors();
        originalMoveSpeed = moveSpeed;
    }

    public override void OnInit()
    {
        base.OnInit();
        this.transform.position = LevelManager.Ins.playerPos.position;
        isWinning = false;
        anim.SetTrigger("Idle");
    }

    private void FixedUpdate()
    {
        this.Move();
    }

    public override void ChangeColor(ColorByEnum color)
    {
        base.ChangeColor(color);
    }

    protected override void Move()
    {
        base.Move();
        if (isWinning == true) return;
        if (joyStick.Vertical != 0)
        {
            RaycastCheck();
        }
        else
        {
            moveSpeed = originalMoveSpeed;
            rb.velocity = new Vector3(joyStick.Horizontal * moveSpeed, rb.velocity.y, joyStick.Vertical * moveSpeed);
        }

        if (joyStick.Horizontal != 0 || joyStick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z));
            ChangeAnim("IsRunning",true);
        }
        else
        {
            ChangeAnim("IsRunning",false);
        }
    }

    private void AdjustMovementOnSlope(Vector3 slopeNormal)
    {
        Vector3 moveDirection = new Vector3(joyStick.Horizontal, 0, joyStick.Vertical).normalized;
        Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, slopeNormal);
        Vector3 adjustedMoveDirection = slopeRotation * moveDirection;

        rb.velocity = new Vector3(adjustedMoveDirection.x * moveSpeed, rb.velocity.y, adjustedMoveDirection.z * moveSpeed);

        Vector3 forwardDirection = Vector3.Cross(transform.right, slopeNormal).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(forwardDirection, slopeNormal);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    private void RaycastCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayPos.position, Vector3.down, out hit, raycastDistance))
        {
            Stair stair = Cache.GetStair(hit.collider);
            if (stair != null)
            {
                if (stackBricks.Count <= 0 && joyStick.Vertical > 0)
                {
                    moveSpeed = 0;
                }
                else if (joyStick.Vertical < 0)
                {
                    moveSpeed = originalMoveSpeed;
                }

                if (stackBricks.Count != 0)
                {
                    if (stair.stairEnum == ColorByEnum.None || stair.stairEnum != this.CurrentColorEnum)
                    {
                        stair.ChangeColor(CurrentColorEnum);
                        // stair.meshRenderer.material.color = objectRenderer.material.color;
                        RemoveBrick();
                    }
                }

            }

            Vector3 hitNormal = hit.normal;
            AdjustMovementOnSlope(hitNormal);
        }
    }

    protected override void AddBrick(Color CurrentColor)
    {
        base.AddBrick(CurrentColor);
    }

    protected override void RemoveBrick()
    {
        base.RemoveBrick();
    }

    protected override void ClearAllBrick()
    {
        base.ClearAllBrick();
    }

    private void OnTriggerEnter(Collider other)
    {
        Brick otherBrick = Cache.GetBrick(other);
        if (otherBrick != null && otherBrick.BrickColorEnum == CurrentColorEnum)
        {
            this.AddBrick(objectRenderer.material.color);
            otherBrick.ActiveFalse();
        }

        Door door = Cache.GetDoor(other);
        if (door != null && joyStick.Vertical > 0)
        {
            Debug.Log(other.gameObject.name);
            // this.ClearAllBrick();
            playerPlatform = door.platformDoor;
            this.transform.position += new Vector3(0, 0, 1f);
            // playerPlatform.SpawnBrick(this, 8);
        }

        WinPlatform winPlatform = Cache.GetWinPlatform(other);
        if (winPlatform != null)
        {
            anim.SetTrigger("IsWinning");
            isWinning = true;
            Debug.Log(this .gameObject.name + "win");
            this.ClearAllBrick();
            LevelManager.Ins.MovePlayerAndBotToWinPos();
            LevelManager.Ins.CurLevel++;
            PlayerPrefs.SetInt("CurrentLevel" , LevelManager.Ins.CurLevel);
        }
    }

    // protected void OnDrawGizmos()
    // {   
    //     Gizmos.DrawRay(rayPos.position, Vector3.down * raycastDistance);
    //     Gizmos.color = Color.red;
    // }
}
