using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioController : MonoBehaviour
{

    AudioSource otsoWalkSource;
    AudioSource otsoRunSource;
    AudioSource roarRun1Source;
    AudioSource roarRun2Source;
    AudioSource roarHurtSource; 
    AudioSource attackSwingSource; 
    AudioSource attackHeavySource;
    AudioSource otsoFeastSource;

    void Start()
    {
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
    int counter = 0;
    void FixedUpdate()
    {        
        if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)  //if Otso is on the move in either direction
        {
            if (FindObjectOfType<PlayerMovement>().Running && otsoRunSource.isPlaying == false)                     //is Otso sprinting
            {
                otsoRunSource.Play();
                ++counter;

                if(counter%3 == 0)
                {
                    if (roarRun1Source.isPlaying == false && roarRun2Source.isPlaying == false)
                    {
                        if (Random.Range(0, 2) == 0)
                            roarRun1Source.Play();
                        else
                            roarRun2Source.Play();
                    }
                }                               
            }

            if(FindObjectOfType<PlayerMovement>().Running == false && otsoWalkSource.isPlaying == false)
            {
                //walking       
                otsoWalkSource.Play();                                
            }

        }

        

        if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            otsoWalkSource.Stop();
            otsoRunSource.Stop();
            roarRun1Source.Stop();
            roarRun2Source.Stop();
        }
    }
}
