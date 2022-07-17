using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        maxHealth = 60;
        moveSpeed = 7;
        damage = 20;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
