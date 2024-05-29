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
    [SerializeField] private Platform platform2;
    private float originalMoveSpeed;


    

    protected override void Start()
    {
        base.Start();
        // this.ChangeColors();
        originalMoveSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        this.Move();
    }

    // private void Test()
    // {
    //     LevelManager.Ins.SpawnObjectsWithDifferentColors(CurrentColorEnum);
    // }

    public override void ChangeColor(ColorByEnum  color)
    {
        base.ChangeColor(color);
        // CurrentColorEnum = LevelManager.Ins.ActiveColor(objectRenderer); // Save the current color enum
        // CurrentColor = objectRenderer.material.color; // Save the current color
    }

    protected override void Move()
    {
        base.Move();

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
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
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
                    if (stair.stairEnum == ColorByEnum.None || stair.stairEnum != CurrentColorEnum)
                    {
                        if (!stair.stairColorChanged)
                        {
                            stair.meshRenderer.material.color = objectRenderer.material.color;
                            RemoveBrick();
                            stair.stairColorChanged = true;
                        }
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
        // Brick newBrick = Instantiate(brickPrefab, Brickholder.position, Brickholder.rotation);
        // newBrick.transform.SetParent(transform);
        // Brickholder.transform.localPosition += new Vector3(0, 0.2f, 0);

        // stackBricks.Push(newBrick.gameObject);
        // newBrick.enabled = false;
        // newBrick.meshRenderer.material.color = CurrentColor;
    }

    protected override void RemoveBrick()
    {
        base.RemoveBrick();
        // if (stackBricks.Count <= 0) return;

        // Destroy(stackBricks.Pop());
        // Brickholder.transform.localPosition -= new Vector3(0, 0.2f, 0);
    }

    protected override void ClearAllBrick()
    {
        base.ClearAllBrick();
        // if (stackBricks.Count == 0) return;

        // while (stackBricks.Count > 0)
        // {
        //     Destroy(stackBricks.Pop());
        //     Brickholder.localPosition = startHolderPos;
        // }
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
            door.ActiveDoor();
            this.ClearAllBrick();
            // platform2.SpawnBrick2(CurrentColorEnum,objectRenderer.material.color);
        }
    }


    protected void OnDrawGizmos()
    {
        Gizmos.DrawRay(rayPos.position, Vector3.down * raycastDistance);
        Gizmos.color = Color.red;
    }
}
