using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    AudioSource otsoWalkSource;
    AudioSource otsoRunSource;
    AudioSource roarRun1Source;
    AudioSource roarRun2Source;
    AudioSource roarHurtSource;
    AudioSource attackSwingSource;
    AudioSource attackHeavySource;
    AudioSource otsoFeastSource;

    private Rigidbody2D rb;
    public float MoveHorz = 0f;
    public float MoveVert = 0f;    

    [SerializeField]
    [Header("Movement")]
    public int MoveSpeed = 5;
    private int RunSpeed = 1;
    public bool Running = false;

    [SerializeField]
    [Header("Health")]
    public float currentHealth;
    public float MaxHealth = 25;
    public Slider HealthBar;
    public Text HealthCount;


    [SerializeField]
    [Header("Stamina")]
    public float Stamina;
    public float MaxStamina = 50;
    public Slider StaminaBar;
    public Text StaminaCount;


    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Stamina = MaxStamina;
        currentHealth = MaxHealth;

        AudioSource[] allMyOtsoSources = this.GetComponentsInChildren<AudioSource>();
        otsoWalkSource = allMyOtsoSources[0];
        otsoRunSource = allMyOtsoSources[1];
        roarRun1Source = allMyOtsoSources[2];
        roarRun2Source = allMyOtsoSources[3];
        roarHurtSource = allMyOtsoSources[4];
        attackSwingSource = allMyOtsoSources[5];
        attackHeavySource = allMyOtsoSources[6];
        otsoFeastSource = allMyOtsoSources[7];
    }

    // Update is called once per frame
    void Update()
    {
        MoveHorz = Input.GetAxis("Horizontal") * MoveSpeed;
        MoveVert = Input.GetAxis("Vertical") * MoveSpeed;

        animator.SetFloat("Horizontal", Mathf.Abs(MoveHorz));


        Vector3 characterScale = transform.localScale;

        if(Input.GetAxis("Horizontal") < 0)
        {
            characterScale.x = 1;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = -1;
        }
        transform.localScale = characterScale;


        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Stamina >= 0)
            {
                RunSpeed = 2;
                Running = true;
                animator.SetBool("Running", true);
                Stamina -= Time.deltaTime;
            }
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
                RunSpeed = 1;
                Running = false;
                animator.SetBool("Running", false);
        }

        StaminaBar.value = Stamina;
        StaminaCount.text = Stamina.ToString("0") + " / " + MaxStamina;

        /*
        HealthBar.value = currentHealth;
        HealthCount.text = currentHealth.ToString("0") + " / " + MaxHealth; */
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(MoveHorz * RunSpeed, MoveVert * RunSpeed);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        roarHurtSource.Play();
    }
}
