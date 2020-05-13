using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public BoxCollider2D col;
    public Rigidbody2D rb;
    public GameObject Proj;
    public GameObject Otso;
    public Vector2 dir;
    public float angle;
    public bool soaring;
    public bool dealtDamage;
    

    [SerializeField]
    public float Velocity = 3f;
    public int damage = 5;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        Proj = this.gameObject;
        Otso = GameObject.FindGameObjectWithTag("Player");
        dir = Otso.transform.position - Proj.transform.position;
        soaring = true;
        dealtDamage = false;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg -44;
        rb.MoveRotation(angle);
        
    }

    void Update()
    {
        if (soaring)
        {
            rb.MovePosition(rb.position + dir * Time.fixedDeltaTime * Velocity);
        }
        else
        {
            StartCoroutine(Despawn());
        }
    }
      

    void OnCollisionEnter2D(Collision2D collision)
    {
        soaring = false;

        if (collision.collider.gameObject.CompareTag("Tree"))
        {
            Proj = this.gameObject;
            Proj.transform.parent = collision.collider.gameObject.transform;
            Destroy(rb);
        }
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Proj = this.gameObject;
            if (!dealtDamage)
            {
                collision.collider.gameObject.GetComponent<PlayerMovement>().TakeDamage(damage);
                dealtDamage = true;
            }
            Proj.transform.parent = Otso.transform;
            Destroy(rb);
        }
        if (collision.collider.gameObject.CompareTag("Arrow"))
        {
            Proj = this.gameObject;
            Proj.transform.parent = collision.collider.gameObject.transform;
            Destroy(rb);
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
