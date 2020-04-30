using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTransparent : MonoBehaviour
{
    public Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player" || other.tag == "Lumberjack" || other.tag == "Hunter")
        {
            animator.SetBool("UnderTree", true);
        }
        
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player" || other.tag == "Lumberjack" || other.tag == "Hunter")
        {
            animator.SetBool("UnderTree", false);
        }
    }
}
