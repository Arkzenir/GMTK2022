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
    
    public int pathRefreshFrameDelay = 8;
    private int pathRefreshCount;

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
        Attack,
        MoveTowardsPlayer,
        Special
    }
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        pathRefreshCount = pathRefreshFrameDelay;
        state = AIState.Idle;
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        wallLayer = LayerMask.NameToLayer("Wall");
        seeker = GetComponent<Seeker>();
        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    protected virtual void Update()
    {
        if(state == AIState.Dead)
            return;
    }

    protected virtual void FixedUpdate()
    {
        if(state == AIState.Dead)
            return;
        
        SetAggro();
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
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        if (state == AIState.MoveTowardsPlayer )
        {
            Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - (Vector2) transform.position).normalized;
            transform.Translate(direction * Time.deltaTime * spd, Space.World);
            
            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDist)
                currentWaypoint++;
        }
    }

    private bool TargetDetected()
    {
        if (Vector2.Distance(transform.position, target.position) > detectRange)
            return false;
        else
            return true;
    }
    private void PerformAttack()
    {
        
    }
    private void SetAggro()
    {
        if (TargetDetected())
        {
            if (pathRefreshCount > 0)
            {
                if (pathRefreshCount == pathRefreshFrameDelay)
                    UpdatePath();
                
                pathRefreshCount--;
            }
            else
            {
                pathRefreshCount = pathRefreshFrameDelay;
            }

            float dist = Vector2.Distance(target.position, transform.position);
            RaycastHit2D hitInfo;
            hitInfo = Physics2D.Raycast(transform.position, (target.position - transform.position), dist,~wallLayer);
            Debug.DrawLine(transform.position, target.position, Color.red);
            bool seePlayer = false;
            seePlayer = hitInfo.collider == null || !hitInfo.collider.gameObject.CompareTag("Wall");
            if (seePlayer)
            {
                state = AIState.MoveTowardsPlayer;
                Debug.DrawLine(transform.position, target.position, Color.blue);
            }
            else
            {
                state = AIState.Idle;
            }
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
