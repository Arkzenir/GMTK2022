using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Enemy
{
    //private float TOLERANCE = 0.01f;
    
    private GameObject attackIndicator;
    private GameObject dashDirection;

    private bool targetLocked;
    private Vector2 dashTarget;
    
    public float attackRange = 4f;
    public float hitboxSize = 0.4f;
    public float attackLinger;
    private float attackLingerCount;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        maxHealth = 120;
        moveSpeed = 2;
        damage = 20;
        targetLocked = false;
        attacking = false;
        
        attackLingerCount = attackLinger;
        attackIndicator = transform.Find("AttackIndicator").gameObject;
        dashDirection = transform.Find("DashDirection").gameObject;
        attackIndicator.SetActive(false);
        dashDirection.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        disregardPath = attacking;
        
        base.FixedUpdate();
        if (state == AIState.Dead)
            return;
        
        attackIndicator.SetActive(attacking);
        if (distanceToTarget <= attackInitiateRange && seePlayer)
        {
            if (!targetLocked)
            {
                RaycastHit2D hitInfo;
                hitInfo = Physics2D.Raycast(transform.position, (target.position - transform.position), attackRange,~wallLayer);
                Debug.DrawLine(transform.position, target.position, Color.magenta);
                if (hitInfo.collider == null)
                {
                    dashTarget = transform.position + ((target.position - transform.position).normalized) * attackRange;
                }
                else
                {
                    dashTarget = hitInfo.point;
                }
                dashDirection.transform.right = (Vector3)dashTarget - transform.position;
                targetLocked = true;
                attacking = true;
            }
            dashDirection.gameObject.SetActive(true);
            attackLingerCount -= Time.deltaTime;
            if (attackLingerCount <= 0)
            {
                dashDirection.gameObject.SetActive(false);
                DashToPosition(moveSpeed * 3, dashTarget);
            }
        }
        
        if (attackLingerCount <= 0)
        {
            attackCount -= Time.deltaTime;
            if (attackCount <= 0)
            {
                attackLingerCount = attackLinger;
                attackCount = attackCooldown;
                attacking = false;
                targetLocked = false;
            }
        }

        if (attackLingerCount > 0 && (distanceToTarget > attackInitiateRange || !seePlayer))
        {
            dashDirection.gameObject.SetActive(false);
            attackLingerCount = attackLinger;
            attackCount = attackCooldown;
            attacking = false;
            targetLocked = false;
        }
    }

    private void DashToPosition(float spd, Vector2 pos)
    {
        if (Vector2.Distance(transform.position, pos) > 0.4f)
        {
            Vector2 dir = (pos - (Vector2) transform.position).normalized;
            transform.Translate(dir * Time.deltaTime * spd);
            if (distanceToTarget <= hitboxSize)
            {
                //damageable.TakeDamage(damage);
            }
        }
    }

    public override void Die()
    {
        base.Die();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
