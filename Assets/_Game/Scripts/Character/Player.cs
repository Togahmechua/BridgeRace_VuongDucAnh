using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : Character
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FixedJoystick joyStick;
    [SerializeField] private float moveSpeed;
    private int i = 1;


    private void FixedUpdate()
    {
        this.Move();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.TAG_Brick))
        {
            // Brick brick = Instantiate(brickPrefab, holder.position , holder.rotation);
            Transform brick = BrickSpawner.Instance.Spawn(BrickSpawner.brick1, holder.position, holder.rotation);
            brick.transform.localPosition += new Vector3(0,0.2f * i,0);
            i++;
            brick.gameObject.SetActive(true);
            brick.transform.SetParent(holder);
        }
    }
}
