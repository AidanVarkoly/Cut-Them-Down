using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    AudioSource huntAttackSource;
    AudioSource takeDamageSource;
    AudioSource huntDeathSource;

    public Animator animator;
    public Rigidbody2D rb;

    public GameObject Arrow;
    public GameObject firePoint;
    Vector3 lookDir;

    public enum HunterStates
    {
        idleState,
        MoveToOtso

    }

    void setCurrentState(HunterStates state)
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
    public float ArrowForce = 4f;

    [SerializeField]
    [Header("AI")]
    public GameObject Otso;
    public float Speed = 2;

    public GameManager manager;
    private HunterStates currentState;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] allMyHuntSources = this.GetComponents<AudioSource>();
        takeDamageSource = allMyHuntSources[0];
        huntDeathSource = allMyHuntSources[1];
        huntAttackSource = allMyHuntSources[2];

        manager = (GameManager)FindObjectOfType(typeof(GameManager));
        Otso = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
        setCurrentState(HunterStates.MoveToOtso);
        rb = GetComponent<Rigidbody2D>();
     }


    private void FixedUpdate()
    {
        Vector3 currentPos = new Vector3 (rb.position.x, rb.position.y, 0f);
        lookDir = (Otso.transform.position - currentPos);
        Quaternion quaternion = Quaternion.Euler(lookDir.x, lookDir.y, lookDir.z);
        firePoint.transform.rotation = quaternion;
        switch (currentState)
        {
            case HunterStates.idleState:
                break;
            case HunterStates.MoveToOtso:
                MoveToOtso();
                break;
        }
    }

    public void Update()
    {
    }

    public IEnumerator Attack()
    {
        if (huntAttackSource.isPlaying == false)
        {
            huntAttackSource.Play();
            animator.SetTrigger("Attack");
            Fire();
        }        
        yield return new WaitForSeconds(1.9f);
    }

    public void Fire()
    {        
        Instantiate(Arrow, firePoint.transform.position, firePoint.transform.rotation);
    }

    

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        takeDamageSource.Play();

        if (currentHealth <= 0)
        {
            huntDeathSource.Play();
            StartCoroutine(Die());
        }
    }

    public void MoveToOtso()
    {
        float Dist2 = Vector3.Distance(Otso.transform.position, transform.position);
        if (Dist2 > 6)
        {
            transform.position = Vector3.MoveTowards(transform.position, Otso.transform.position, Speed * 2 * Time.deltaTime);
        }
        else
        {
            StartCoroutine(Attack());
        }

    }
    public IEnumerator Die()
    {
        manager.EnemiesAlive--;
        GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        GetComponent<Collider2D>().enabled = !GetComponent<Collider2D>().enabled;
        yield return new WaitUntil(() => !huntDeathSource.isPlaying);
        if (huntDeathSource.isPlaying == false)
        {
            Destroy(this.gameObject);
        }
    }
}

