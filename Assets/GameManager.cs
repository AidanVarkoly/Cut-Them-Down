﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum SpawnStates
    {
        Idle,
        Wave1,
        Wave2,
        Wave3,
        Wave4,
        EndGame,
        Dead,
        LumberJacksWin
    }

    void setCurrentState(SpawnStates state)
    {
        currentState = state;
    }
    [Header("List of Enemies")]
    public List<GameObject> Enemies;

    [Header("Prefabs")]
    public GameObject hunter;
    public GameObject lumberjack;
    public PlayerMovement Otso;

    [Header("SpawnPoints")]
    public Transform[] SpawnPoints;
    public Transform[] SpawnPointsWave2;
    public Transform[] SpawnPointsWave3;
    public Transform[] SpawnPointsWave4;
    public Transform Spawn2;
    public Transform Spawn3;
    public Transform Spawn4;
    int RandomSpawn;

    [Header("Waves")]
    public int RandomEnemy;
    public SpawnStates currentState;
    private Scene Level1;
    public float MaxGracePeriod = 10;
    private float GracePeriod;
    public int EnemiesInWaves;
    public int TreesDestroyed = 0;
    private int MaxEnemiesInWaves = 20;
    public float SpawnTime = 0;
    public float MaxSpawnTime = 7;
    public int EnemiesAlive = 0;
    public int WaveCounter = 0;
    public bool Wave1Finished = false;
    public bool Wave2Finished = false;
    public bool Wave3Finished = false;

    [Header("Text")]
    public Text WaveNumber;
    public Text EnemyNumber;
    public Text InBetweenRoundTime;
    public Text Dead;
    public Text EndGame;
    public Text Lost;
    

    // Start is called before the first frame update
    void Start()
    {
        EnemiesAlive = Enemies.Count;
        SpawnTime = MaxSpawnTime;
        GracePeriod = MaxGracePeriod;
        setCurrentState(SpawnStates.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        WaveNumber.text = WaveCounter.ToString("");
        EnemyNumber.text = EnemiesAlive.ToString("");

        if(Otso.Stamina <= 0)
        {
            setCurrentState(SpawnStates.Dead);
        }
        if(!Wave1Finished && TreesDestroyed == 19)
        {
            setCurrentState(SpawnStates.LumberJacksWin);
        }
        if (Wave1Finished && TreesDestroyed == 21)
        {
            setCurrentState(SpawnStates.LumberJacksWin);
        }
        if (Wave2Finished && TreesDestroyed == 15)
        {
            setCurrentState(SpawnStates.LumberJacksWin);
        }
        if (Wave3Finished && TreesDestroyed == 11)
        {
            setCurrentState(SpawnStates.LumberJacksWin);
        }

        switch (currentState)
        {
            case SpawnStates.Idle:

                GracePeriod -= Time.deltaTime;
                InBetweenRoundTime.gameObject.SetActive(true);
                InBetweenRoundTime.text = GracePeriod.ToString("0") + " / " + MaxGracePeriod;

                if (GracePeriod <= 0)
                {
                    InBetweenRoundTime.gameObject.SetActive(false);
                    setCurrentState(SpawnStates.Wave1);
                }
                if (!Wave1Finished && GracePeriod <= 0)
                {
                    WaveCounter++;
                }

                if (Wave1Finished && GracePeriod <= 0)
                {
                    setCurrentState(SpawnStates.Wave2);
                    TreesDestroyed = 0;
                    TeleportWave2();
                    WaveCounter++;
                }
                if (Wave2Finished && GracePeriod <= 0)
                {
                    setCurrentState(SpawnStates.Wave3);
                    TreesDestroyed = 0;
                    TeleportWave3();
                    WaveCounter++;
                }
                if (Wave3Finished && GracePeriod <= 0)
                {
                    setCurrentState(SpawnStates.Wave4);
                    TreesDestroyed = 0;
                    TeleportWave4();
                    WaveCounter++;
                }
                break;
            case SpawnStates.Wave1:

                SpawnTime -= Time.deltaTime;

                if (EnemiesInWaves < MaxEnemiesInWaves)
                {
                    if(SpawnTime <= 0)
                    {
                        SpawnEnemies();
                        SpawnTime = MaxSpawnTime;
                    }
                    
                }

                if (EnemiesAlive == 0)
                {
                    Wave1Finished = true;
                    GracePeriod = 10;
                    setCurrentState(SpawnStates.Idle);
                }


                break;
            case SpawnStates.Wave2:

                SpawnTime -= Time.deltaTime;
                
                if(Enemies.Count < 25)
                {
                    Enemies.Add(hunter);
                    Enemies.Add(lumberjack);
                    Enemies.Add(lumberjack);
                    Enemies.Add(lumberjack);
                    Enemies.Add(hunter);
                    MaxEnemiesInWaves = 25;
                    EnemiesAlive = Enemies.Count;
                    EnemiesInWaves = 0;
                }

                if (EnemiesInWaves < MaxEnemiesInWaves)
                {
                    if (SpawnTime <= 0)
                    {
                        SpawnEnemiesWave2();
                        SpawnTime = MaxSpawnTime;
                    }

                }

                if (EnemiesAlive == 0)
                {
                    Wave2Finished = true;
                    GracePeriod = 10;
                    setCurrentState(SpawnStates.Idle);
                }

                break;
            case SpawnStates.Wave3:

                SpawnTime -= Time.deltaTime;

                if (Enemies.Count < 30)
                {
                    Enemies.Add(hunter);
                    Enemies.Add(lumberjack);
                    Enemies.Add(lumberjack);
                    Enemies.Add(lumberjack);
                    Enemies.Add(hunter);
                    MaxEnemiesInWaves = 30;
                    EnemiesAlive = Enemies.Count;
                    EnemiesInWaves = 0;
                }

                if (EnemiesInWaves < MaxEnemiesInWaves)
                {
                    if (SpawnTime <= 0)
                    {
                        SpawnEnemiesWave3();
                        SpawnTime = MaxSpawnTime;
                    }

                }

                if (EnemiesAlive == 0)
                {
                    Wave2Finished = true;
                    GracePeriod = 10;
                    setCurrentState(SpawnStates.Idle);
                }
      

                break;
            case SpawnStates.Wave4:

                SpawnTime -= Time.deltaTime;

                if (Enemies.Count < 35)
                {
                    Enemies.Add(hunter);
                    Enemies.Add(lumberjack);
                    Enemies.Add(lumberjack);
                    Enemies.Add(lumberjack);
                    Enemies.Add(hunter);
                    MaxEnemiesInWaves = 35;
                    EnemiesAlive = Enemies.Count;
                    EnemiesInWaves = 0;
                }

                if (EnemiesInWaves < MaxEnemiesInWaves)
                {
                    if (SpawnTime <= 0)
                    {
                        SpawnEnemiesWave4();
                        SpawnTime = MaxSpawnTime;
                    }

                }

                if (EnemiesAlive == 0)
                {
                    Wave2Finished = true;
                    GracePeriod = 10;
                    setCurrentState(SpawnStates.EndGame);
                }
                break;
            case SpawnStates.EndGame:
                EndGame.gameObject.SetActive(true);
                StartCoroutine("LoadMenu");

                break;
            case SpawnStates.Dead:
                Dead.gameObject.SetActive(true);
                StartCoroutine("RestartLevel");
                break;
            case SpawnStates.LumberJacksWin:
                Lost.gameObject.SetActive(true);
                StartCoroutine("RestartLevel");
                break;
        }
    }

    public IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Level 1");
     }

    public IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Menu");
    }

    public void SpawnEnemies()
    {
        RandomSpawn = Random.Range(0, SpawnPoints.Length);
        RandomEnemy = Random.Range(0, Enemies.Count);
        Instantiate(Enemies[RandomEnemy], SpawnPoints[RandomSpawn].position, Quaternion.identity);
        EnemiesInWaves++;
    }

    public void SpawnEnemiesWave2()
    {
        RandomSpawn = Random.Range(0, SpawnPointsWave2.Length);
        RandomEnemy = Random.Range(0, Enemies.Count);
        Instantiate(Enemies[RandomEnemy], SpawnPointsWave2[RandomSpawn].position, Quaternion.identity);
        EnemiesInWaves++;
    }
    public void SpawnEnemiesWave3()
    {
        RandomSpawn = Random.Range(0, SpawnPointsWave3.Length);
        RandomEnemy = Random.Range(0, Enemies.Count);
        Instantiate(Enemies[RandomEnemy], SpawnPointsWave3[RandomSpawn].position, Quaternion.identity);
        EnemiesInWaves++;
    }
    public void SpawnEnemiesWave4()
    {
        RandomSpawn = Random.Range(0, SpawnPointsWave4.Length);
        RandomEnemy = Random.Range(0, Enemies.Count);
        Instantiate(Enemies[RandomEnemy], SpawnPointsWave4[RandomSpawn].position, Quaternion.identity);
        EnemiesInWaves++;
    }

    public void TeleportWave2()
    {
        if(Wave1Finished)
        {
            Otso.gameObject.transform.position = Spawn2.gameObject.transform.position;
        }
    }
    public void TeleportWave3()
    {
        if (Wave2Finished)
        {
            Otso.gameObject.transform.position = Spawn3.gameObject.transform.position;
        }
    }
    public void TeleportWave4()
    {
        if (Wave3Finished)
        {
            Otso.gameObject.transform.position = Spawn4.gameObject.transform.position;
        }
    }
}
