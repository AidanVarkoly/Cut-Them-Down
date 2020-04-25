using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    AudioSource treeFallSource;

    [Header("Health")]
    public int maxHealth = 12;
    public int currentHealth;

    void Start()
    {
        treeFallSource = this.GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {               
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Fall();
        }
    }

    IEnumerator Fall()
    {
        treeFallSource.Play();
        yield return new WaitUntil(() => !treeFallSource.isPlaying);
        Destroy(this.gameObject);                                     //tree sprite change / animation goes here                                     
    }
}
