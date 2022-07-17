using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform Playertransform;
    private Transform CameraTransform;
    private void Awake()
    {
        CameraTransform = GetComponent<Transform>();
    }

    void Update()
    {
        CameraTransform.position = new Vector3(Playertransform.position.x , Playertransform.position.y , -10);
    }
}
