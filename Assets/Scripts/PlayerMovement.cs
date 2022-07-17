using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D RB;
    private float stepSize = 1f;
    [SerializeField] private float moveSpeed = 10f;
    public Vector2 direction;
    public Transform transform;
    [SerializeField] Player player;
    public bool roll;
    public float rollTimerCount;

    public Vector2 MovementInput { get; set; }

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        rollTimerCount = player.rollDuration;

    }
    
    private void FixedUpdate()
    {
        stepSize = 1f;
        if (MovementInput.x != 0 && MovementInput.y != 0)
        {
            stepSize = stepSize / MathF.Sqrt(2);
        }

        if (!player.rolling)
        {
            RB.MovePosition(RB.position + (MovementInput * (stepSize * moveSpeed * Time.deltaTime)));
        }

        if (roll)
        {
            if (rollTimerCount > 0)
            {
                rollTimerCount -= Time.deltaTime;
            }
            else
            {
                roll = false;
                rollTimerCount = player.rollDuration;
            }

            transform.Translate(direction * (Time.deltaTime * (moveSpeed * 2)));
        }
    }

    public Vector2 Roll(Vector2 mousePos)
    {
        Vector3 mouse3 = mousePos;
        mouse3.z = 0;
        Vector2 mouseDir= (mouse3 - transform.position).normalized;
        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
        mouseDir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        return mouseDir;
    }
}
