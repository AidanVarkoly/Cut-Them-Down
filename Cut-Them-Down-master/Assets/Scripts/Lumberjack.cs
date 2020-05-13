using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
       MoveToOtso,
       FleeToTree,
       Dead

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
    public BoxCollider2D box;
    int index;
    public GameObject CurrentTree;
    public GameObject Otso;
    public GameObject StartingPoint;
    public float Speed = 2;

    [SerializeField]
    [Header("Eaten")]
    public Slider EatBar;
    public float timeDead;
    public float EatTime = 25;
    public float MaxEatTime = 50;


    private bool attacked = false;
    private bool decision = false;
    private bool eaten = false;
    private bool beingEaten = false;
    private EnemyStates currentState;
    public GameManager manager;
    public ParticleSystem Blood;
    public PlayerMovement move;
    public TextMeshProUGUI FText;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        manager = (GameManager)FindObjectOfType(typeof (GameManager));
        Otso = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
        EatBar.value = EatTime;
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
                animator.SetTrigger("IsIdle");
                break;
            case EnemyStates.MoveToTree:
                MoveToTree();
                break;
            case EnemyStates.MoveToOtso:
                MoveToOtso();
                break;
            case EnemyStates.FleeToTree:
                FleeToTree();
                break;
            case EnemyStates.Dead:
                Die();
                break;
        }

        //CheckRaycast();
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
         EatBar.value = EatTime;
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
            manager.EnemiesAlive--;
            lumbDeathSource.Play();
            setCurrentState(EnemyStates.Dead);

        }
    }

    public IEnumerator Chop()
    {
        if (attackTree1Source.isPlaying == false && attackTree2Source.isPlaying == false)
        {
            CurrentTree = FindClosestTree(Trees);

            if (Random.Range(0, 2) == 1)
            {
                animator.SetTrigger("IsChopping");
                attackTree1Source.Play();
                CurrentTree.GetComponent<Tree>().TakeDamage(AttackDamage);
                yield return new WaitForSeconds(AttackRate);
            }
            else
            {
                animator.SetTrigger("IsChopping");
                attackTree2Source.Play();
                CurrentTree.GetComponent<Tree>().TakeDamage(AttackDamage);
                yield return new WaitForSeconds(AttackRate);
            }
        }
    }

        public IEnumerator Attack()
    {
        if (attackOtsoSource.isPlaying == false)
        {
            animator.SetTrigger("IsChopping");
            attackOtsoSource.Play();
            Otso.GetComponent<PlayerMovement>().TakeDamage(AttackDamage);
        }
        yield return new WaitForSeconds(AttackRate);
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
                setCurrentState(EnemyStates.FleeToTree);
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
        float Dist = Vector3.Distance(CurrentTree.transform.position, transform.position);

        //RaycastHit2D hit = CheckRaycast();

        if (Dist > 1.5)
        {
            transform.position = Vector3.MoveTowards(transform.position, CurrentTree.transform.position, Speed * Time.deltaTime);
            animator.SetTrigger("IsMoving");
        ////if (hit.collider.Equals(index) && hit.collider.Equals(!CurrentTree))
        //{
        //    GameObject tree = hit.collider.gameObject;
        //    gameObject.transform.RotateAround(tree.transform.position, Vector2.right, Speed * Time.deltaTime);
        //}
        }
        else
        {

            StartCoroutine("Chop");
        }
    }

    public void MoveToOtso()
    {
        float Dist2 = Vector3.Distance(Otso.transform.position, transform.position);
        if (Dist2 > 1.5)
        {
            transform.position = Vector3.MoveTowards(transform.position, Otso.transform.position, Speed * 2 * Time.deltaTime);
            animator.SetTrigger("IsMoving");
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

    public void FleeToTree()
    {
        float Dist = Vector3.Distance(CurrentTree.transform.position, transform.position);

        if (Dist > 1.5)
        {
            transform.position = Vector3.MoveTowards(transform.position, CurrentTree.transform.position, Speed * 2 * Time.deltaTime);
            animator.SetTrigger("IsRunning");
        }
        else
        {

            StartCoroutine("Chop");
        }
    }

    public void Die()
    {
        box.enabled = false;
        animator.Play("LumberJackDead");
        timeDead += Time.deltaTime;
        float Dist3 = Vector3.Distance(Otso.transform.position, transform.position);
        if (Dist3 < 4 && Input.GetKeyDown(KeyCode.F) && !beingEaten)
        {
            FText.gameObject.SetActive(false);
            Blood.gameObject.SetActive(false);
            beingEaten = true;
        }
        if (beingEaten)
        {
            EatBar.gameObject.SetActive(true);
            EatTime -= Time.deltaTime * 5;

            if (Input.GetKeyDown(KeyCode.F))
            {
                EatTime += 4;
                Otso.GetComponent<Animator>().Play("Eating");
            }
            if (EatTime >= MaxEatTime)
            {
                eaten = true;
            }

            if (eaten)
            {
                EatBar.gameObject.SetActive(false);
                Otso.GetComponent<PlayerMovement>().Stamina += 8;
                Destroy();
                eaten = false;
                beingEaten = false;
                Otso.GetComponent<Animator>().SetBool("Eating", false);
            }
        }
        if (!beingEaten && timeDead >= 10 || Dist3 > 6)
        {
            FText.gameObject.SetActive(false);
            Blood.gameObject.SetActive(false);
            Destroy();
        }
        if(beingEaten && EatTime <= 0)
        {
            EatBar.gameObject.SetActive(false);
            Destroy();
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
        
    }

    //public RaycastHit2D CheckRaycast()
    //{
    //    Debug.DrawLine(StartingPoint.transform.position, CurrentTree.transform.position, Color.red);
    //    return Physics2D.Raycast(StartingPoint.transform.position, CurrentTree.transform.position);
    //}

}

