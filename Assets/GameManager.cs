using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum SpawnStates
    {
        Idle,
        Wave1,
        Wave2,
        Wave3,
        Wave4,
        EndGame
    }

    void setCurrentState(SpawnStates state)
    {
        currentState = state;
    }

    private SpawnStates currentState;

    // Start is called before the first frame update
    void Start()
    {
        setCurrentState(SpawnStates.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case SpawnStates.Idle:


                break;
            case SpawnStates.Wave1:


                break;
            case SpawnStates.Wave2:


                break;
            case SpawnStates.Wave3:


                break;
            case SpawnStates.Wave4:


                break;
            case SpawnStates.EndGame:


                break;
        }
    }
}
