using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Enemy
{
    private GameObject attackIndicator;
    private GameObject pawnAttack;
    
    public float attackRange = 1.5f;
    public float attackLinger;
    private float attackLingerCount;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = 50;
        moveSpeed = 2;
        damage = 15;
        attackLinger = 0.2f;
        attackCooldown = 0.8f;
        attackInitiateRange = 0.8f;
        
        attackLingerCount = attackLinger;
        attackIndicator = transform.Find("AttackIndicator").gameObject;
        pawnAttack = transform.Find("PawnAttack").gameObject;
        attackIndicator.SetActive(false);
        pawnAttack.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        attackIndicator.SetActive(attacking);
        if (distanceToTarget <= attackInitiateRange && seePlayer)
        {
            attacking = true;
            attackCount -= Time.deltaTime;
            if (attackCount <= 0)
            {
                disregardPath = true;
                if (distanceToTarget <= attackRange)
                {
                    //Deal damage to enemy once
                    pawnAttack.gameObject.SetActive(true);
                }
                attackLingerCount -= Time.deltaTime;
                if (attackLingerCount <= 0)
                {
                    pawnAttack.gameObject.SetActive(false);
                    disregardPath = false;
                    attackLingerCount = attackLinger;
                    attackCount = attackCooldown;
                }
            }
        }
        else
            attacking = false;
    }
    
}