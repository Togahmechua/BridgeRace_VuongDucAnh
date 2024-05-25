using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : Character
{
    private static Player ins;
    public static Player Ins => ins;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FixedJoystick joyStick;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private List<Transform> brickList;
    [SerializeField] private Transform rayPos;
    [SerializeField] private float raycastDistance;
    // [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Renderer objectRenderer;

    public int i = 1;
    public Color CurrentColor { get; private set; } // Add this line

    protected override void Awake()
    {
        Player.ins = this;
        this.ChangeColors();
    }

    private void FixedUpdate()
    {
        this.Move();
    }

    private void ChangeColors()
    {
        LevelManager.Ins.ActiveColor(objectRenderer);
        CurrentColor = objectRenderer.material.color; // Save the current color
    }

    protected override void Move()
    {
        base.Move();
        rb.velocity = new Vector3(joyStick.Horizontal * moveSpeed, rb.velocity.y, joyStick.Vertical * moveSpeed);

        if (joyStick.Horizontal != 0 || joyStick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            anim.SetBool("IsRunning", true);
        }
        else anim.SetBool("IsRunning", false);

        if (joyStick.Vertical > 0)
        {
            RaycastCheck();
        }
    }

    private void RaycastCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayPos.position, Vector3.down, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag(Constants.TAG_Ground))
            {
                Debug.Log("Ground");
            }
            else if (hit.collider.CompareTag(Constants.TAG_Stair))
            {
                // RemoveBrick();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.TAG_Brick))
        {
            GameObject newBrick = Instantiate(brickPrefab, Brickholder.position, Brickholder.rotation);
            newBrick.transform.SetParent(Brickholder);
            newBrick.transform.localPosition = new Vector3(0, 0.2f * i, 0); 
            i++;
            brickList.Add(newBrick.transform);

            // Get the color of the last brick in the list
            Color lastBrickColor = GetLastBrickColor();
            Debug.Log("Last brick color: " + lastBrickColor);
        }
        else if (other.gameObject.CompareTag(Constants.TAG_Stair))
        {
            // this.RemoveBrick();
        }
    }

    public Color GetLastBrickColor()
    {
        if (brickList.Count > 0)
        {
            Transform lastBrick = brickList[brickList.Count - 1];
            Renderer lastBrickRenderer = lastBrick.GetComponent<Renderer>();
            if (lastBrickRenderer != null)
            {
                return lastBrickRenderer.material.color;
            }
        }
        return Color.white; // Return a default color if there is no brick or renderer
    }
}
