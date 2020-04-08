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

    public IEnumerator Attack(int target)
    {
        if (target == 0) //tree
        {

            if (FindObjectOfType<AudioManager>().isPlaying("LumbAttack_tree2") == false && FindObjectOfType<AudioManager>().isPlaying("LumbAttack_tree1") == false)      // if we aren't playing a chop sound
            {
                if (Random.Range(0,2) == 1) //pick a random chop noise and play it
                {
                    FindObjectOfType<AudioManager>().Play("LumbAttack_tree1");
                    yield return new WaitForSeconds(.1f);
                    if(Random.Range(0,2) == 1)
                    {
                        FindObjectOfType<AudioManager>().Play("LumbAttack_grunt1");
                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().Play("LumbAttack_grunt2");
                    }
                    yield return new WaitForSeconds(AttackRate);
                }
                else
                {
                    FindObjectOfType<AudioManager>().Play("LumbAttack_tree2");
                    yield return new WaitForSeconds(.1f);
                    if (Random.Range(0, 2) == 1)
                    {
                        FindObjectOfType<AudioManager>().Play("LumbAttack_grunt1");
                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().Play("LumbAttack_grunt2");
                    }
                    yield return new WaitForSeconds(AttackRate);
                }
            }

            else if (target == 1) //otso
            {
                FindObjectOfType<AudioManager>().Play("LumbAttack_otso");
                yield return new WaitForSeconds(.1f);
                if (Random.Range(0, 2) == 1)
                {
                    FindObjectOfType<AudioManager>().Play("LumbAttack_grunt1");
                }
                else
                {
                    FindObjectOfType<AudioManager>().Play("LumbAttack_grunt2");
                }
                yield return new WaitForSeconds(AttackRate);
            }
        }
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
                FindObjectOfType<AudioManager>().Play("LumbDec_flight");
                break;
            case 1:
                setCurrentState(EnemyStates.MoveToOtso);
                FindObjectOfType<AudioManager>().Play("LumbDec_fight");
                break;
        }
    }

    public void MoveToTree()
    { 
        float Dist = Vector3.Distance(CurrentTree.transform.position, transform.position);
        if (Dist > 1.49)
        { 
            transform.position = Vector3.MoveTowards(transform.position, CurrentTree.transform.position, Speed * Time.deltaTime);
        }
        if (Dist <= 1.5)
        {
            StartCoroutine(Attack(0)); //0 is tree
        }
    }

    public void MoveToOtso()
    {
        float Dist2 = Vector3.Distance(Otso.transform.position, transform.position);
        if (Dist2 > 1.49)
        {
            transform.position = Vector3.MoveTowards(transform.position, Otso.transform.position, Speed * 2 * Time.deltaTime);
        }
        if (Dist2 <= 1.5)
        {
            StartCoroutine(Attack(1)); //1 is Otso
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
        FindObjectOfType<AudioManager>().Play("LumbDeath");
    }
}
