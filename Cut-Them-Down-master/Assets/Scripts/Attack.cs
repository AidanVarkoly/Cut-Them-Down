using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    AudioSource otsoWalkSource;
    AudioSource otsoRunSource;
    AudioSource roarRun1Source;
    AudioSource roarRun2Source;
    AudioSource roarHurtSource;
    AudioSource attackSwingSource;
    AudioSource attackHeavySource;
    AudioSource otsoFeastSource;
    public AudioSource[] allMyOtsoSources;


    public Transform attackPoint;
    public LayerMask LumberjackLayer;
    public LayerMask HunterLayer;
    public Lumberjack lumberjack;
    public Hunter hunter;
    public PlayerMovement Move;
    public Rigidbody2D lj;
    public Rigidbody2D h;

    bool lightAttack = false;
    bool heavyAttack = false;
    public float lightAttackRange = 1f;
    public int lightAttackDamage = 1;
    public float heavyAttackRange = 1.5f;
    public int heavyAttackDamage = 3;

    public float lightAttackRate = 2f;
    public float heavyAttackRate = 1f;
    private float nextAttackTime = 0.5f;
    private float nextHeavyAttackTime = 3f;
    public Animator animator;

    void Start()
    {
        allMyOtsoSources = this.GetComponentsInChildren<AudioSource>();
        otsoWalkSource = allMyOtsoSources[0];
        otsoRunSource = allMyOtsoSources[1];
        roarRun1Source = allMyOtsoSources[2];
        roarRun2Source = allMyOtsoSources[3];
        roarHurtSource = allMyOtsoSources[4];
        attackSwingSource = allMyOtsoSources[5];
        attackHeavySource = allMyOtsoSources[6];
        otsoFeastSource = allMyOtsoSources[7];
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1") && heavyAttack == false)
            {
                animator.SetBool("IsLightAttacking", true);
                lightAttack = true;
                LightAttack();
                Move.Stamina -= 1;
                StartCoroutine("EndLightAttack");
                nextAttackTime = Time.time + 1f / lightAttackRate;
            }
        }
        if (Time.time >= nextHeavyAttackTime)
        {
            if (Input.GetButtonDown("Fire2") && lightAttack == false)
            {
                animator.SetBool("IsHeavyAttacking", true);
                heavyAttack = true;
                HeavyAttack();
                Move.Stamina -= 2;
                StartCoroutine("EndHeavyAttack");
                nextHeavyAttackTime = Time.time + 1f / heavyAttackRate;
            }
        }
    }

    void LightAttack()
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, lightAttackRange, LumberjackLayer);
        attackSwingSource.Play();

        foreach (Collider2D Lumberjack in hits)
        {
            Lumberjack.GetComponent<Lumberjack>().TakeDamage(lightAttackDamage);
        }

        Collider2D[] hits2 = Physics2D.OverlapCircleAll(attackPoint.position, lightAttackRange, HunterLayer);


        foreach (Collider2D Hunter in hits2)
        {
            Hunter.GetComponent<Hunter>().TakeDamage(lightAttackDamage);
        }
    }

    void HeavyAttack()
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, heavyAttackRange, LumberjackLayer);
        attackHeavySource.Play();

        foreach (Collider2D Lumberjack in hits)
        {
            Lumberjack.GetComponent<Lumberjack>().TakeDamage(heavyAttackDamage);
        }

        Collider2D[] hits2 = Physics2D.OverlapCircleAll(attackPoint.position, heavyAttackRange, HunterLayer);


        foreach (Collider2D Hunter in hits2)
        {
            Hunter.GetComponent<Hunter>().TakeDamage(heavyAttackDamage);
        }
    }
        public IEnumerator EndLightAttack()
    {
        yield return new WaitForSeconds(0.3f);
        lightAttack = false;
        animator.SetBool("IsLightAttacking", false);
    }
    public IEnumerator EndHeavyAttack()
    {
        yield return new WaitForSeconds(0.5f);
        heavyAttack = false;
        animator.SetBool("IsHeavyAttacking", false);
    }

    public void Eating()
    {
        otsoFeastSource.Play();
    }
}
