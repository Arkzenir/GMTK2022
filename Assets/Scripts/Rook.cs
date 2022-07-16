using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Enemy
{
    private float TOLERANCE = 0.01f;
    
    private GameObject attackIndicator;
    private GameObject dashDirection;
    
    public float attackRange = 4f;
    public float attackLinger;
    private float attackLingerCount;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = 120;
        moveSpeed = 2;
        damage = 20;
        attackInitiateRange = 3f;
        attackLinger = 0.4f;
        attackCooldown = 1f;

        attackLingerCount = attackLinger;
        attackIndicator = transform.Find("AttackIndicator").gameObject;
        dashDirection = transform.Find("DashDirection").gameObject;
        attackIndicator.SetActive(false);
        dashDirection.gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        disregardPath = attacking;
        base.Update();
        attackIndicator.SetActive(attacking);
        
        if (distanceToTarget <= attackInitiateRange && seePlayer)
        {
            attacking = true;
            dashDirection.gameObject.SetActive(true);
            attackLingerCount -= Time.deltaTime;
            if (attackLingerCount <= 0)
            {
                dashDirection.gameObject.SetActive(false);
                //DashToPosition();
            }
        }
    }

    private void DashToPosition(float spd, Vector2 pos)
    {
        if (Vector2.Distance(transform.position, pos) > 0.4f)
        {
            Vector2 dir = (pos - (Vector2) transform.position).normalized;
            transform.Translate(dir * Time.deltaTime * spd);
        }
        else
        {
            attackCount -= Time.deltaTime;
            if (attackCount <= 0)
            {
                attackLingerCount = attackLinger;
                attackCount = attackCooldown;
                attacking = false;
            }
        }
    }
}
