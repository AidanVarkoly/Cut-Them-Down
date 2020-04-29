using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public enum HunterStates
    {
        idleState,
        MoveToOtso

    }

    void setCurrentState(HunterStates state)
    {
        currentState = state;
    }

    AudioSource takeDamageSource;
    AudioSource huntDeathSource;
    AudioSource huntAttackSource;

    [Header("Health")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Attack")]
    public int AttackDamage = 1;
    public float AttackRate = 2f;
    public float NextAttackTime = 2f;

    [SerializeField]
    [Header("AI")]
    public GameObject Otso;
    public float Speed = 2;

    public GameManager manager;
    private HunterStates currentState;

    // Start is called before the first frame update
    void Start()
    {
        manager = (GameManager)FindObjectOfType(typeof(GameManager));
        Otso = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
        setCurrentState(HunterStates.MoveToOtso);

        AudioSource[] allMyHuntSources = this.GetComponents<AudioSource>();
        takeDamageSource = allMyHuntSources[0];
        huntDeathSource = allMyHuntSources[1];
        huntAttackSource = allMyHuntSources[2];
    }

    public void Update()
    {
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case HunterStates.idleState:
                break;
            case HunterStates.MoveToOtso:
                MoveToOtso();
                break;
        }
    }

    public void Attack()
    {
        if (huntAttackSource.isPlaying == false)
        {
            huntAttackSource.Play();
                //put otso damage code here
        }
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
        if (Dist2 > 4)
        {
            transform.position = Vector3.MoveTowards(transform.position, Otso.transform.position, Speed * 2 * Time.deltaTime);
        }
        else
        {
            Attack();
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

