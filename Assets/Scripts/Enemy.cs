using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum EnemyStates
    {
       idleState,
       MoveToTree,
       MoveToOtso

    }

    void setCurrentState(EnemyStates state)
    {
        currentState = state;
    }

    [Header("Health")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Attack")]
    public int AttackDamage = 1;
    public float AttackRate = 2f;
    public float NextAttackTime = 2f;

    [SerializeField]
    [Header("AI")]
    GameObject[] Trees;
    int index;
    public GameObject CurrentTree;
    public GameObject Otso;
    public float Speed = 2;

    private bool attacked = false;
    private bool decision = false;
    private EnemyStates currentState;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        FindTree();
        setCurrentState(EnemyStates.idleState);
    }

    private void FixedUpdate()
    {
        switch(currentState)
        {
            case EnemyStates.idleState:
                break;
            case EnemyStates.MoveToTree:
                MoveToTree();
                break;
            case EnemyStates.MoveToOtso:
                MoveToOtso();
                break;
        }
    }

    void Update()
    {
        if(currentHealth == maxHealth)
        {
            setCurrentState(EnemyStates.MoveToTree);
        }
        if(currentHealth < maxHealth && decision == false)
        {
            attacked = true;
            setCurrentState(EnemyStates.idleState);
        }

        if(attacked)
        {
            GetFunction();
            decision = true;
            attacked = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Attack()
    {

    }

    public void FindTree()
    {
        Trees = GameObject.FindGameObjectsWithTag("Tree");
        index = Random.Range(0, Trees.Length);
        CurrentTree = Trees[index];
    }

    GameObject FindClosestTree(GameObject[] Trees)
    {
        CurrentTree = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject tree in Trees)
        {
            Vector3 directionToTarget = tree.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                CurrentTree = tree;
            }
        }

        return CurrentTree;
    }

    public void GetFunction()
    {
       int FuncToChoose = Random.Range(0, 2);

       switch(FuncToChoose)
        {
            case 0:
                setCurrentState(EnemyStates.MoveToTree);
                break;
            case 1:
                setCurrentState(EnemyStates.MoveToOtso);
                break;
        }
    }

    public void MoveToTree()
    { 
        float Dist = Vector3.Distance(CurrentTree.transform.position, transform.position);
        if (Dist > 1.5)
        { 
            transform.position = Vector3.MoveTowards(transform.position, CurrentTree.transform.position, Speed * Time.deltaTime);
        }
        if (Dist == 1.5)
        {

            Attack();

        }
    }

    public void MoveToOtso()
    {
        float Dist2 = Vector3.Distance(Otso.transform.position, transform.position);
        if (Dist2 > 1.5)
        {
            transform.position = Vector3.MoveTowards(transform.position, Otso.transform.position, Speed * 2 * Time.deltaTime);
        }
        if (Dist2 == 1.5)
        {
            Attack();
        }

        if(Dist2 > 10)
        {
            FindClosestTree(Trees);
            setCurrentState(EnemyStates.MoveToTree);
        }
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }
}
