using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask EnemyLayer;

    public float attackRange = 1f;
    public int attackDamage = 1;

    public float attackRate = 2f;
    private float nextAttackTime = 0.5f;

    void Start()
    {
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Attacking();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attacking()
    {


        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);

       
        foreach(Collider2D Enemy in hits)
        {
            Enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
}
