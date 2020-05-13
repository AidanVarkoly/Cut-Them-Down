using System.Collections;
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
        Dead
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
    int RandomSpawn;

    [Header("Waves")]
    public int RandomEnemy;
    public SpawnStates currentState;
    private Scene Level1;
    public float MaxGracePeriod = 10;
    private float GracePeriod;
    public int EnemiesInWaves;
    private int MaxEnemiesInWaves = 10;
    public float SpawnTime = 0;
    public float MaxSpawnTime = 5;
    public int EnemiesAlive = 0;
    public int WaveCounter = 0;
    public bool Wave1Finished = false;
    public bool Wave2Finished = false;

    [Header("Text")]
    public Text WaveNumber;
    public Text EnemyNumber;
    public Text InBetweenRoundTime;
    public Text Dead;
    public Text EndGame;

    

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
                    WaveCounter++;
                }
                if (Wave2Finished && GracePeriod <= 0)
                {
                    setCurrentState(SpawnStates.Wave3);
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
                
                if(Enemies.Count < 15)
                {
                    Enemies.Add(hunter);
                    Enemies.Add(lumberjack);
                    Enemies.Add(lumberjack);
                    Enemies.Add(lumberjack);
                    Enemies.Add(hunter);
                    MaxEnemiesInWaves = 15;
                    EnemiesAlive = Enemies.Count;
                    EnemiesInWaves = 0;
                }

                if (EnemiesInWaves < MaxEnemiesInWaves)
                {
                    if (SpawnTime <= 0)
                    {
                        SpawnEnemies();
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

                setCurrentState(SpawnStates.EndGame);

                break;
            case SpawnStates.Wave4:

                break;
            case SpawnStates.EndGame:
                EndGame.gameObject.SetActive(true);
                StartCoroutine("RestartLevel");

                break;
            case SpawnStates.Dead:
                Dead.gameObject.SetActive(true);
                StartCoroutine("RestartLevel");
                break;
        }
    }

    public IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Level 1");
     }


    public void SpawnEnemies()
    {
        RandomSpawn = Random.Range(0, SpawnPoints.Length);
        RandomEnemy = Random.Range(0, Enemies.Count);
        Instantiate(Enemies[RandomEnemy], SpawnPoints[RandomSpawn].position, Quaternion.identity);
        EnemiesInWaves++;
    }

   }
