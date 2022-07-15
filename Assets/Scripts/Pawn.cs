using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Enemy
{
    
    // Start is called before the first frame update
    void Start()
    {
        health = 50;
        moveSpeed = 5;
        damage = 15;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
