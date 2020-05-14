using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{
    AudioSource treeFallSource;

    [Header("Health")]
    public float maxHealth = 20f;
    public float currentHealth;
    public Slider treeHealth;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        treeFallSource = this.GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth == maxHealth)
        {
            treeHealth.gameObject.SetActive(false);
        }
        if (currentHealth != maxHealth)
        {
            treeHealth.gameObject.SetActive(true);
        }
        treeHealth.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Fall();
        }
    }

    public void Fall()
    {
        treeFallSource.Play();
        gameManager.GetComponent<GameManager>().TreesDestroyed++;
        Destroy(this.gameObject);
    }
}
