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

    IEnumerator Fall()
    {
        treeFallSource.Play();
        yield return new WaitUntil(() => !treeFallSource.isPlaying);
        Destroy(this.gameObject);
    }
}
