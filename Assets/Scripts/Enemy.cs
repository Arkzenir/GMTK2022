using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int moveSpeed;
    public int damage;
    public AIState state;
    public enum AIState
    {
        Idle,
        Dead
    }
    
    // Start is called before the first frame update
    void Start()
    {
        state = AIState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        
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
