using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 mousePosition { get; set; }
    public Animator animator;
    public float delay = 0.3f;
    private bool attackBLocked;
    [SerializeField] Collider2D collider;
    public int damageVal = 20;
    public bool IsAttacking { get; private set; }

    private void Start()
    {
        collider.enabled = false;
    }

    private void Update()
    {
        if (IsAttacking)
        {
            return;
        }
        Vector2 direction = (mousePosition - (Vector2) transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
        }

        transform.localScale = scale;
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }

        
    }

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }
    public void Attack()
    {
        if (attackBLocked)
        {
            return;
        }
        animator.SetTrigger("Attack");
        IsAttacking = true;
        attackBLocked = true;
        collider.enabled = true;
        StartCoroutine(DelayAttack());
        StartCoroutine((DelayFollow()));
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBLocked = false;
    }

    private IEnumerator DelayFollow()
    {
        yield return new WaitForSeconds(0.75f);
        IsAttacking = false;
        collider.enabled = false;
    }

}
