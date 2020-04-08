using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioController : MonoBehaviour
{
    int counter = 0;
    void FixedUpdate()
    {        
        if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)  //if Otso is on the move in either direction
        {
            if (FindObjectOfType<PlayerMovement>().Running)                     //is Otso sprinting
            {
                FindObjectOfType<AudioManager>().Play("DualStep_run");
                ++counter;

                if(counter%20 == 0)
                {
                    if (FindObjectOfType<AudioManager>().isPlaying("OtsoRoar_run1") == false && FindObjectOfType<AudioManager>().isPlaying("OtsoRoar_run2") == false)
                    {
                        if (Random.Range(0, 2) == 0)
                            FindObjectOfType<AudioManager>().Play("OtsoRoar_run1");
                        else
                            FindObjectOfType<AudioManager>().Play("OtsoRoar_run2");
                    }
                }                               
            }

            if(FindObjectOfType<PlayerMovement>().Running == false)
            {
                //walking       
                FindObjectOfType<AudioManager>().Play("DualStep_walk");                                
            }

        }

        

        if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            FindObjectOfType<AudioManager>().Stop("OtsoRoar_run1");
            FindObjectOfType<AudioManager>().Stop("OtsoRoar_run2");
            FindObjectOfType<AudioManager>().Stop("DualStep_walk");
            FindObjectOfType<AudioManager>().Stop("DualStep_run");
        }
    }

    private void OnCollisionEnter(Collision2D collision)
    {
        FindObjectOfType<AudioManager>().Stop("OtsoRoar_run1");
        FindObjectOfType<AudioManager>().Stop("OtsoRoar_run2");
        FindObjectOfType<AudioManager>().Stop("DualStep_walk");
        FindObjectOfType<AudioManager>().Stop("DualStep_run");
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        FindObjectOfType<AudioManager>().Stop("OtsoRoar_run1");
        FindObjectOfType<AudioManager>().Stop("OtsoRoar_run2");
        FindObjectOfType<AudioManager>().Stop("DualStep_walk");
        FindObjectOfType<AudioManager>().Stop("DualStep_run");
    }
}
