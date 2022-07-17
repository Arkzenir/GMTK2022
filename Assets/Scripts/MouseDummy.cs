using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDummy : MonoBehaviour
{
    public Vector2 MousePos;
    private Transform transform;

    public void Awake()
    {
        transform = GetComponent<Transform>();
    }

    public void Update()
    {
        transform.position = MousePos;
    }
}
