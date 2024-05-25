using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : Character
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FixedJoystick joyStick;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Brick brickPrefab;
    private Stack<GameObject> stackBricks = new Stack<GameObject>();
    [SerializeField] private Transform rayPos;
    [SerializeField] private float raycastDistance;
    [SerializeField] private Renderer objectRenderer;


    public Color CurrentColor { get; private set; }
    public EColor.ColorByEnum CurrentColorEnum { get; private set; }

    protected override void Start()
    {
        base.Start();
        this.ChangeColors();
    }

    private void FixedUpdate()
    {
        this.Move();
    }

    private void ChangeColors()
    {
        CurrentColorEnum = LevelManager.Ins.ActiveColor(objectRenderer); // Save the current color enum
        CurrentColor = objectRenderer.material.color; // Save the current color
    }

    protected override void Move()
    {
        base.Move();
        if (joyStick.Vertical > 0)
        {
            RaycastCheck();
        }
        else
        {
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
        // Adjust movement based on the slope angle
        Vector3 moveDirection = new Vector3(joyStick.Horizontal, 0, joyStick.Vertical).normalized;
        Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, slopeNormal);
        Vector3 adjustedMoveDirection = slopeRotation * moveDirection;

        rb.velocity = new Vector3(adjustedMoveDirection.x * moveSpeed, rb.velocity.y, adjustedMoveDirection.z * moveSpeed);

        // Adjust the character's rotation to align with the slope normal
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
                if (stackBricks.Count <= 0) return;
                if  (stair.eColor == EColor.ColorByEnum.None || stair.eColor != CurrentColorEnum)
                {
                    if (stair.stairColorChanged == false)
                    {
                        Debug.Log("A");
                        stair.meshRenderer.material.color = CurrentColor;
                        RemoveBrick();
                        stair.stairColorChanged = true;
                    }
                } 
            }

            Vector3 hitNormal = hit.normal;
            AdjustMovementOnSlope(hitNormal);
        }
    }

    protected override void AddBrick()
    {
        base.AddBrick();
        Brick newBrick = Instantiate(brickPrefab, Brickholder.position, Brickholder.rotation);
        newBrick.transform.SetParent(transform);
        Brickholder.transform.localPosition += new Vector3(0, 0.2f, 0);

        stackBricks.Push(newBrick.gameObject);
        newBrick.enabled = false;
        newBrick.meshRenderer.material.color = CurrentColor;
    }

    protected override void RemoveBrick()
    {
        base.RemoveBrick();
        if (stackBricks.Count <= 0) return;

        Destroy(stackBricks.Pop());
        Brickholder.transform.localPosition -= new Vector3(0, 0.2f, 0);
    }

    protected override void ClearAllBrick()
    {
        base.ClearAllBrick();
        if (stackBricks.Count == 0) return;

        while (stackBricks.Count > 0)
        {
            Destroy(stackBricks.Pop());
            Brickholder.localPosition = startHolderPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Brick otherBrick = Cache.GetBrick(other);
        if (otherBrick != null && otherBrick.eColor == CurrentColorEnum) // Check color before collecting
        {
            this.AddBrick();
            otherBrick.ActiveFalse();
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawRay(rayPos.position, Vector3.down * raycastDistance);
        Gizmos.color = Color.red;
    }
}
