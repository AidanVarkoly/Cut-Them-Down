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
    public AudioSource[] allMyOtsoSources;

    private Rigidbody2D rb;
    public float MoveHorz = 0f;
    public float MoveVert = 0f;
    public float SpeedHorz = 0f;
    public float SpeedVert = 0f;
    

    [SerializeField]
    [Header("Movement")]
    public int MoveSpeed = 5;
    private int RunSpeed = 1;
    public bool Running = false;
    public bool Moving = false;

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

        SpeedHorz = Mathf.Abs(MoveHorz);
        SpeedVert = Mathf.Abs(MoveVert);

        if(SpeedHorz > 0 || SpeedVert > 0)
        {
            Moving = true;
            animator.SetBool("Moving", true);
        }
        else
        {
            Moving = false;
            animator.SetBool("Moving", false);
        }


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
            if (Stamina > 15)
            {
                RunSpeed = 2;
                Running = true;
                animator.SetBool("Running", true);
                Stamina -= Time.deltaTime;
            }
        }
        if (!Input.GetKey(KeyCode.LeftShift) || Stamina <= 15)
        {
                RunSpeed = 1;
                Running = false;
                animator.SetBool("Running", false);
        }

        StaminaBar.value = Stamina;
        if(Stamina <= 30)
        {
            StaminaBar.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        if (Stamina <= 15)
        {
            StaminaBar.fillRect.GetComponent<Image>().color = Color.red;
        }
        if(Stamina <= 0)
        {
            StaminaBar.fillRect.GetComponent<Image>().color = Color.clear;
        }
        StaminaCount.text = Stamina.ToString("0") + " / " + MaxStamina; 
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(MoveHorz * RunSpeed, MoveVert * RunSpeed);
    }

    public void TakeDamage(int damage)
    {
        Stamina -= damage;
        roarHurtSource.Play();
    }
}
