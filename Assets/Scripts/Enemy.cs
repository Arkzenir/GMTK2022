using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Enemy : MonoBehaviour
{
    public int health = 1;
    public float moveSpeed = 1;
    public int damage = 0;
    public AIState state;
    
    
    public float detectRange = 9f;
    public float attackInitiateRange = 1f;
    public float attackCooldown = 0.8f;
    protected float attackCount;
    
    protected bool seePlayer;
    protected bool attacking;
    protected bool disregardPath;
    protected float distanceToTarget;
    
    protected float pathRefreshFrameDelay = 0.25f;
    private float pathRefreshCount;

    
    public Transform target;
    public LayerMask wallLayer;
    private float nextWaypointDist = 0.05f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    
    public enum AIState
    {
        Idle,
        Dead,
        MoveTowardsPlayer,
    }
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        seePlayer = false;
        attacking = false;
        attackCount = attackCooldown;
        pathRefreshCount = pathRefreshFrameDelay;
        target = GameObject.FindWithTag("Player").transform;
        distanceToTarget = Vector2.Distance(transform.position, target.position);
        state = AIState.Idle;
        Debug.Log("before target");
        Debug.Log("target pos: " + target.position);
        wallLayer = LayerMask.NameToLayer("Wall");
        seeker = GetComponent<Seeker>();
        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    protected virtual void Update()
    {
        if(state == AIState.Dead)
            return;
        distanceToTarget = Vector2.Distance(transform.position, target.position);
    }

    protected virtual void FixedUpdate()
    {
        if(state == AIState.Dead)
            return;
        
        SetMoveTowardsPlayer();
        FollowPath();
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void UpdatePath()
    {
        if (path.IsDone())
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }
    
    private void FollowPath()
    {
        float spd = moveSpeed;
        if (path == null)
            return;
        if (disregardPath)
            return;
        
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        if (state == AIState.MoveTowardsPlayer)
        {
            Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - (Vector2) transform.position).normalized;
            transform.Translate(direction * Time.deltaTime * spd, Space.World);
            
            float distanceToWaypoint = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDist)
                currentWaypoint++;
        }
    }

    private bool TargetDetected()
    {
        if (distanceToTarget > detectRange)
            return false;
        else
        {
            RaycastHit2D hitInfo;
            hitInfo = Physics2D.Raycast(transform.position, (target.position - transform.position), distanceToTarget,~wallLayer);
            Debug.DrawLine(transform.position, target.position, Color.red);
            seePlayer = (hitInfo.collider == null || !hitInfo.collider.gameObject.CompareTag("Wall"));
            return seePlayer;
        }
    }
    private void PerformAttack()
    {
        Attack();
    }

    private void Attack()
    {
        
    }
    
    private void SetMoveTowardsPlayer()
    {
        if (TargetDetected())
        {
            if (pathRefreshCount > 0)
            {
                if (pathRefreshCount == pathRefreshFrameDelay)
                    UpdatePath();
                
                pathRefreshCount -= Time.deltaTime;
            }
            else
            {
                pathRefreshCount = pathRefreshFrameDelay;
            }
            state = AIState.MoveTowardsPlayer;
            Debug.DrawLine(transform.position, target.position, Color.blue);
            
        }
        else
        {
            state = AIState.Idle;
        }
    }
    
    public int TakeDamage(int val)
    {
        if (health > 0 || state != AIState.Dead || health - val > 0) {
            health = health - val;
        }
        else {
            health = 0;
            Die();
        }
        return health;
    }

    private void Die()
    {
        state = AIState.Dead;
    }
}
