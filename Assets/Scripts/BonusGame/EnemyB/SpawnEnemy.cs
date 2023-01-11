using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public static SpawnEnemy instance;

    [Header("Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Enemy Types")]
    public GameObject[] Enemies;

    public float setSpeed = 2;
    public float setZigZag = 2;
    [Header("Boss Mode")]
    public bool bossMode = false;
    public int bossLevel = 1;
    [SerializeField]
    float spawnRepeat;
    int spawnSpeedMode = 0;
    List<int> usedPoints = new List<int>();

    //automate next level
    [Header("Auto Next Level")]
    [SerializeField]
    bool automateLevel = false;
    public int nextLevelCounter = 100;
    int drinkTracker;
    [SerializeField]
    float autoRepeat = 1.8f;
    [Header("End Game")]
    public bool badEnding = false;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        foreach (Transform child in transform)
        { spawnPoints.Add(child); }
    }
    
    public void StartSpawn(float startT, float repeatT)
    {
        InvokeRepeating("Spawn", startT, repeatT);
    }
    public void StopSpawn()
    {
        CancelInvoke();
    }

    void Spawn()
    {
        int drinkp = CusterCtrl.instance.drinkPoints;
        int killp = BonusGameManager.instance.killScore;

        //make level voids
        //increase difficulty with drink points.............
        //use usedpoints list for fucking whatever.. om niet die shit te doen spawnen op zelfde plek
        //Fuck dit spel maak het alsjeblieft af deze week!!

        if (!badEnding)
        {
            if (!bossMode)
            {
                //BossLevels
                if (killp > 49 && bossLevel == 1)
                {
                    bossMode = true;
                    Boss01(0);
                    return;
                }
                else if (killp > 149 && bossLevel == 2)
                {
                    bossMode = true;
                    Boss01(1);
                    return;
                }
                else if (killp > 249 && bossLevel == 3)
                {
                    bossMode = true;
                    Boss02();
                    return;
                }
                else if (killp > 349 && bossLevel == 4)
                {
                    bossMode = true;
                    Boss01(2);
                    return;
                }
                else if (killp > 499 && bossLevel == 5)
                {
                    bossMode = true;
                    Boss03();
                }

                //Drink Levels
                if (drinkp < 40 && drinkp >= 0)
                {
                    Level0();

                    return;
                }
                else if (drinkp > 40 && drinkp < 100)
                {
                    Level1();
                }
                else if (drinkp > 100 && drinkp < 200)
                {
                    SpawnSpeedUp(0, 2.5f, 2.5f);

                    Level2();

                }
                else if (drinkp > 200 && drinkp <= 235)
                {
                    // introduce multiple enemies at once
                    Level3();

                }
                else if (drinkp > 235 && drinkp <= 274)
                {
                    // Delay
                    Level4();
                }
                else if (drinkp > 274 && drinkp <= 299)
                {
                    Level5();
                }
                else if (drinkp >= 300 && drinkp <= 349)
                {
                    SpawnSpeedUp(1, 2, 2);

                    Level5();
                }
                else if (drinkp > 349 && drinkp < 449)
                {
                    SpawnSpeedUp(2, 2, 2.5f);

                    Level6();
                }
                else if (drinkp > 449 && drinkp < 499)
                {
                    SpawnSpeedUp(3, 2, 2f);
                    Level7();
                }
                else if (drinkp > 499 && drinkp < 549)
                {
                    SpawnSpeedUp(4, 2, 1.9f);
                    Level8();
                }
                else if (drinkp > 549 && drinkp < 665)
                {
                    SpawnSpeedUp(5, 2, 1.8f);
                    Level8();
                }
                else if (drinkp > 665)
                {
                    if (!automateLevel)
                        automateLevel = true;

                    Level8();
                    //iedere keer als drinkp omhoog gaat.. gaat er een punt van nextlevel counter af
                }
            }
        }
        else
        {
            //Spawn Death
            DeathSpawn();
        }
    }

    void SpawnSpeedUp(int sMode,float startT, float repeatT) //sMode is to prevent from running everytime Spawn is called
    {
        if (spawnSpeedMode == sMode)
        {
            spawnRepeat = repeatT;
            spawnSpeedMode++;
            StopSpawn();
            StartSpawn(startT, repeatT);
        }
    }
    void AutoSpeedUp(float startT, float repeatT) // no sMode
    {
        spawnRepeat = repeatT;
      //spawnSpeedMode++;
        StopSpawn();
        StartSpawn(startT, repeatT);
    }
    #region EnemieSubTypes
    void E1Loader(int type)
    {
        if (type == 0)
            LoadTypeA();
        else if (type == 1)
            LoadTypeB();
        else if (type == 2)
            LoadTypeC();
    }
    void LoadTypeA()
    {

        Enemies[0].GetComponent<Enemy01>()._fall = true;
        Enemies[0].GetComponent<Enemy01>()._zigZag = false;
        Enemies[0].GetComponent<Enemy01>()._target = false;
        Enemies[0].GetComponent<Enemy01>().speed = setSpeed;
       // Enemies[0].GetComponent<Enemy01>().zigZagSpeed = setZigZag;
    }

    void LoadTypeB()
    {

        Enemies[0].GetComponent<Enemy01>()._target = false;
        Enemies[0].GetComponent<Enemy01>()._zigZag = true;
        Enemies[0].GetComponent<Enemy01>()._fall = true;
        setSpeed = 2f;
        Enemies[0].GetComponent<Enemy01>().speed = setSpeed;
        setZigZag = 2;
        Enemies[0].GetComponent<Enemy01>().zigZagSpeed = setZigZag;
    }

    void LoadTypeC()
    {

        Enemies[0].GetComponent<Enemy01>()._zigZag = false;
        Enemies[0].GetComponent<Enemy01>()._fall = false;
        Enemies[0].GetComponent<Enemy01>()._target = true;
        Enemies[0].GetComponent<Enemy01>().speed = setSpeed;
        
    }
    #endregion

    #region DifficultyLevels
    //dont forget to clean usedSpawnPoints when you need to

    void Level0()
    {
        usedPoints.Clear();
        int rng = 0;

        if (rng == 0)
        {
            LoadTypeA();
        }

        GameObject enemy = Instantiate(Enemies[0]);
        enemy.transform.position = spawnPoints[RandomSpawnPoint()].position;
    }

    void Level1()
    {
        usedPoints.Clear();
        int rng = Random.Range(0, 2);

        if (rng == 0)
        {
            LoadTypeA();
        }
        else if (rng == 1)
        {
            LoadTypeB();
        }

        GameObject enemy = Instantiate(Enemies[0]);
        enemy.transform.position = spawnPoints[RandomSpawnPoint()].position;
    }

    void Level2()
    {
        usedPoints.Clear();

        int rng = Random.Range(0, 3);

        if (rng == 0)
        {
            LoadTypeA();
        }
        else if (rng == 1)
        {
            LoadTypeB();
        }
        else if (rng == 2)
        {
            LoadTypeC();
        }

        GameObject enemy = Instantiate(Enemies[0]);
        enemy.transform.position = spawnPoints[RandomSpawnPoint()].position;
    }

    void Level3()
    {
        usedPoints.Clear();
        //spawn 1 or 2 enemies a,b,c
 
        int amount = Random.Range(0, 2);
        
        for (int _i = 0; _i <= amount; _i++)
        {
            int rng = Random.Range(0, 3);

            if (rng == 0)
            {
                LoadTypeA();
            }
            else if (rng == 1)
            {
                LoadTypeB();
            }
            else if (rng == 2)
            {
                LoadTypeC();
            }

            GameObject enemy = Instantiate(Enemies[0]);
            int newSpawnPoint = RandomSpawnPoint();

            enemy.transform.position = spawnPoints[newSpawnPoint].position;
        }
    }

    void Level4()
    {
        usedPoints.Clear();
        //spawn 1 or 2 enemies and add minor delay

        int amount = Random.Range(0, 2);

        for (int _i = 0; _i <= amount; _i++)
        {
            int rng = Random.Range(0, 3);

            if (rng == 0)
            {
                LoadTypeA();
            }
            else if (rng == 1)
            {
                LoadTypeB();
            }
            else if (rng == 2)
            {
                LoadTypeC();
            }

            
            int newSpawnPoint = RandomSpawnPoint();

            int delayRng = Random.Range(0, 3);

            if (delayRng == 2)
            {
                float delayTime = Random.Range(0, 2);
             //   print(delayTime);
                StartCoroutine(SpawnDelay(newSpawnPoint,delayTime));
            }
            else
            {
                GameObject enemy = Instantiate(Enemies[0]);
                enemy.transform.position = spawnPoints[newSpawnPoint].position;
            }
        }
    }

    IEnumerator SpawnDelay(int spoint, float _time)
    {
        yield return new WaitForSeconds(_time);

        GameObject enemy = Instantiate(Enemies[0]);
        int newSpawnPoint = RandomSpawnPoint();

        enemy.transform.position = spawnPoints[spoint].position;
    }
    
    //add birds
    void Level5()
    {
        usedPoints.Clear();
        //spawn 1 or 2 enemies and add minor delay

        int amount = Random.Range(0, 2);

        for (int _i = 0; _i <= amount; _i++)
        {
            int rng = Random.Range(0, 6);

            if (rng == 0)
            {
                LoadTypeA();
            }
            else if (rng == 1)
            {
                LoadTypeB();
            }
            else if (rng == 2)
            {
                LoadTypeC();
            }


            int newSpawnPoint = RandomSpawnPoint();

            int delayRng = Random.Range(0, 3);

            if (delayRng >= 1)
            {
                float delayTime = Random.Range(0.3f, 1);
               // print(delayTime);
                StartCoroutine(SpawnDelay(newSpawnPoint, delayTime));
            }
            else
            {
                int type = 0;
                if(rng > 2)
                {
                    type = 2;
                }
                
                GameObject enemy = Instantiate(Enemies[type]);
                enemy.transform.position = spawnPoints[newSpawnPoint].position;
                
            }
        }
    }

    void Level6()
    {
        int amount = Random.Range(0, 3);
        for(int i = 0; i <= amount; i++)
        {
            int Etype = Random.Range(0, 2);
            int newSpawnPoint = RandomSpawnPoint();
            
            GameObject enemy = Instantiate(Enemies[2]);
            enemy.GetComponent<Enemy02>().unitType = Etype;

            if (Etype == 0)
            {
                enemy.transform.position = spawnPoints[newSpawnPoint].position;
            }
            else
            {
                Vector3 newPos = spawnPoints[newSpawnPoint].position;
                int r = Random.Range(0, 2);
                if (r == 0)
                    newPos.y = 2.8f;
                else
                    newPos.y = 2.5f;

                enemy.transform.position = newPos;
            }
        }

    }

    void Level7()
    {
        usedPoints.Clear();
        //spawn 1 or 2 enemies and add minor delay

        int amount = Random.Range(0, 3);

        for (int _i = 0; _i <= amount; _i++)
        {
            int rng = Random.Range(0, 9);

            if (rng < 3)
            {
                if (rng == 0)
                {
                    LoadTypeA();
                }
                else if (rng == 1)
                {
                    LoadTypeB();
                }
                else if (rng == 2)
                {
                    LoadTypeC();
                }
            }

            int newSpawnPoint = RandomSpawnPoint();

            int delayRng = Random.Range(0, 3);

            if (delayRng >= 1)
            {
                float delayTime = Random.Range(0.3f, 1);
                // print(delayTime);
                StartCoroutine(SpawnDelay(newSpawnPoint, delayTime));
            }
            else
            {
                int type = 0;
                if (rng > 2)
                {
                    type = 2;
                }

                GameObject enemy = Instantiate(Enemies[type]);
                enemy.transform.position = spawnPoints[newSpawnPoint].position;

                if (type == 2)
                {
                    int subtype = 0;
                    subtype = Random.Range(0, 2);
                    enemy.GetComponent<Enemy02>().unitType = subtype;

                    if(subtype == 1)
                    {
                        Vector3 newpos = enemy.transform.position;
                        newpos.y = 2.8f;

                        enemy.transform.position = newpos;
                    }
                }
            }
        }

    }

    //add robo
    void Level8()
    {
        Level7();
        RoboSpawn();
    }
    #endregion

    void RoboSpawn()
    {
        int rngSpawn = Random.Range(0,4); 

        if(rngSpawn == 0)
        {
            GameObject _robo = Instantiate(Enemies[4]);

            Vector3 spawnPos = new Vector3(-3.47f, 0, 0); //spawn left of screen by default
            int rngPos = Random.Range(0, 2);
            if (rngPos == 1) //spawn Right of screen
            {
                spawnPos.x = 3.47f;

                _robo.GetComponent<Robo01>().left = true;
            }


            _robo.transform.position = spawnPos;


        }
    }

    void DeathSpawn()
    {
        GameObject _death = Instantiate(Enemies[6]);
        _death.transform.position = new Vector3(0, 3.2f, 0);
    }

    #region Bosses
    void Boss01(int type)
    {
        GameObject boss = Instantiate(Enemies[1]);
        
        boss.GetComponent<Boss01>().difficultyLvl = type;

        float _x = 2.75f;
        int r = Random.Range(0, 2);
        if(r == 1)
        {
            _x = -2.75f;
        }

        boss.transform.position = new Vector3(_x, 2.46f, 0);
        //startpos Y 2.46
    }

    void Boss02()
    {
        GameObject boss = Instantiate(Enemies[3]);
        Vector3 newpos = new Vector3(0, 2.8f, 0);
        boss.transform.position = newpos;
    }

    void Boss03()
    {
        GameObject boss03 = Instantiate(Enemies[5]);
        boss03.transform.position = new Vector3(0, 2.55f, 0);
    }

    #endregion
    int RandomSpawnPoint()
    {
        int rng = 0;

        rng = Random.Range(0, spawnPoints.Count);
        if (usedPoints.Count != 0)
        {
            for (int i = 0; i < usedPoints.Count; i++)
            {
                if (usedPoints[i] == rng)
                {
                    int newRng = Random.Range(1, 3);
                    if(newRng + rng < spawnPoints.Count)
                    {
                        rng += newRng;
                    }
                    else
                    {
                        rng -= newRng;
                    }

                }
            }
        }
        usedPoints.Add(rng);

        return rng;
    }

    private void Update()
    {
        if (automateLevel)
        {
            if(CusterCtrl.instance.drinkPoints > drinkTracker)
            {
                drinkTracker = CusterCtrl.instance.drinkPoints;

                if (nextLevelCounter != 0)
                    nextLevelCounter--;
                else
                {
                    if(autoRepeat > 0.1f)
                        autoRepeat -= 0.1f;

                    AutoSpeedUp(2, autoRepeat);
                   
                    nextLevelCounter = 100;
                }
            }
        }
    }
}
