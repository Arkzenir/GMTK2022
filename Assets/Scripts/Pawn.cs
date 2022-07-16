using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Enemy
{
    private GameObject attackIndicator;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = 50;
        moveSpeed = 2;
        damage = 15;
        
        attackIndicator = transform.Find("AttackIndicator").gameObject;
        attackIndicator.SetActive(false);
    }
    
}