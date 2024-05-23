using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] protected Player player;
    [SerializeField] protected float speed;

    private Vector3 offset;

    protected virtual void Awake()
    {
        this.LoadPlayer();
    }

    void Start()
    {
        // Tính offset giữa vị trí của camera và vị trí của nhân vật
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        // Tính toán vị trí mới của camera dựa trên vị trí của nhân vật và offset
        Vector3 targetPosition = player.transform.position + offset;

        // Di chuyển camera đến vị trí mới
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    protected virtual void LoadPlayer()
    {
        if (this.player != null) return;
        this.player = GameObject.FindObjectOfType<Player>();
    }
}