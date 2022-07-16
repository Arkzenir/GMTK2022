using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = 30;
        moveSpeed = 7;
        damage = 50;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
