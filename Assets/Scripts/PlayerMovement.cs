using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float MoveHorz;
    private float MoveVert;

    [SerializeField]
    [Header("Movement")]
    public int MoveSpeed = 5;
    private int RunSpeed = 1;
    private bool Running = false;


    [SerializeField]
    [Header("Stamina")]
    public float Stamina;
    public float MaxStamina = 5;
    public Slider StaminaBar;
    private bool StartReloadingStamina = true;
    public Text StaminaCount;

    IEnumerator CoolDown()
    {
        StartReloadingStamina = false;

        yield return StartReloadingStamina = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Stamina = MaxStamina;
    }

    // Update is called once per frame
    void Update()
    {
     MoveHorz = Input.GetAxis("Horizontal");
     MoveVert = Input.GetAxis("Vertical");


        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Stamina > 0)
            {
                RunSpeed = 2;
                Running = true;
                Stamina -= Time.deltaTime;
            }
            if (Stamina <= 0 || StartReloadingStamina == true)
            {
                RunSpeed = 1;
                Running = false;
                StartCoroutine(CoolDown());
            }
        }

        if(!Input.GetKey(KeyCode.LeftShift))
        {
                RunSpeed = 1;
                Running = false;
                StartCoroutine(CoolDown());
        }


        if(StartReloadingStamina == true)
        {
            Stamina += Time.deltaTime;
            if (Stamina > 5)
            {
                Stamina = 5;
                StartReloadingStamina = false;
            }
        }
        if (StartReloadingStamina == true && Input.GetKey(KeyCode.LeftShift))
        {
            RunSpeed = 1;
            Stamina += Time.deltaTime;
            Running = false;
        }
        StaminaBar.value = Stamina;
        StaminaCount.text = Stamina.ToString("0" + " / " + MaxStamina); 
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(MoveHorz * MoveSpeed * RunSpeed, MoveVert * MoveSpeed * RunSpeed);
    }
}
