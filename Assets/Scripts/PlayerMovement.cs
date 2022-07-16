using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D RB;
    private float stepSize = 1f;
    [SerializeField] private float moveSpeed = 10f;
    
    public Vector2 MovementInput { get; set; }

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        
    }

    private void FixedUpdate()
    {
        stepSize = 1f;
        if (MovementInput.x != 0 && MovementInput.y != 0)
        {
            stepSize = stepSize / MathF.Sqrt(2);
        }
        RB.MovePosition(RB.position + (MovementInput * stepSize * moveSpeed * Time.deltaTime));
    }
}
