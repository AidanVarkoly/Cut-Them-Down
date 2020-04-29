using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lumberjack : MonoBehaviour
{
    AudioSource decFightSource;
    AudioSource decFlightSource;
    AudioSource attackTree1Source;
    AudioSource attackTree2Source;
    AudioSource attackOtsoSource;
    AudioSource lumbDamageSource;
    AudioSource lumbDeathSource;
    AudioSource lumbFirstDamSource;
    

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
    public GameManager manager;

    
  

    // Start is called before the first frame update
    void Start()
    {
        manager = (GameManager)FindObjectOfType(typeof (GameManager));
        Otso = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
        FindTree();
        setCurrentState(EnemyStates.idleState);

        AudioSource[] allMyLumbSources = this.GetComponents<AudioSource>();
        decFightSource = allMyLumbSources[0];
        decFlightSource = allMyLumbSources[1];
        attackTree1Source = allMyLumbSources[2];
        attackTree2Source = allMyLumbSources[3];
        attackOtsoSource = allMyLumbSources[4];
        lumbDamageSource = allMyLumbSources[5];
        lumbDeathSource = allMyLumbSources[6];
        lumbFirstDamSource = allMyLumbSources[7];
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

        if (!decision)
        {
            lumbFirstDamSource.Play();
        }
        else
        {
            lumbDamageSource.Play();
        }        

        if (currentHealth <= 0)
        {
            lumbDeathSource.Play();
            StartCoroutine(Die());
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
                decFlightSource.Play();
                break;
            case 1:
                setCurrentState(EnemyStates.MoveToOtso);
                decFightSource.Play();
                break;
        }
    }

    public void MoveToTree()
    {
        CurrentTree = FindClosestTree(Trees);
        float Dist = Vector3.Distance(CurrentTree.transform.position, transform.position);
        if (Dist > 1.5)
        { 
            transform.position = Vector3.MoveTowards(transform.position, CurrentTree.transform.position, Speed * Time.deltaTime);
        }
        else
        {
            StartCoroutine("Chop");
        }
    }

    public IEnumerator Chop()
    {
        if (attackTree1Source.isPlaying == false && attackTree2Source.isPlaying == false)
        {
            CurrentTree = FindClosestTree(Trees);

            if (Random.Range(0, 2) == 1)
            {
                attackTree1Source.Play();
                CurrentTree.GetComponent<Tree>().TakeDamage(AttackDamage);
                yield return new WaitForSeconds(AttackRate);
            }
            else
            {
                attackTree2Source.Play();
                CurrentTree.GetComponent<Tree>().TakeDamage(AttackDamage);
                yield return new WaitForSeconds(AttackRate);
            }
        }
        
    }
    public IEnumerator Attack()
    {
        if(attackOtsoSource.isPlaying == false)
        {
            attackOtsoSource.Play();
            Otso.GetComponent<PlayerMovement>().TakeDamage(AttackDamage);
        }
        yield return new WaitForSeconds(AttackRate);

    }

    public void MoveToOtso()
    {
        float Dist2 = Vector3.Distance(Otso.transform.position, transform.position);
        if (Dist2 > 1.5)
        {
            transform.position = Vector3.MoveTowards(transform.position, Otso.transform.position, Speed * 2 * Time.deltaTime);
        }
        else
        {
            StartCoroutine("Attack");
        }

        if(Dist2 > 10)
        {
            FindClosestTree(Trees);
            setCurrentState(EnemyStates.MoveToTree);
        }
    }
    public IEnumerator Die()
    {
        manager.EnemiesAlive--;
        GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        GetComponent<Collider2D>().enabled = !GetComponent<Collider2D>().enabled;
        yield return new WaitUntil(() => !lumbDeathSource.isPlaying);
        if(lumbDeathSource.isPlaying == false)
        {
            Destroy(this.gameObject);
        }    
    }
}

