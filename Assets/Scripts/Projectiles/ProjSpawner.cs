using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjSpawner : MonoBehaviour
{
    public static ProjSpawner instance;


    public bool Classic;
    public float spawnStart = 0f;
    public float spawnRepeatSeconds = 2;

    Vector2 pSpawnPos;
    float boundarieL = -1.5F;
    float boundarieR = 3.5F;

    float spawnY = 3.3F;

    public List<float> spawnX = new List<float>();

    public GameObject projectile;

    public bool spawnmode2 = false;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (projectile == null)
        {
            Debug.Log("Projectile not set");
        }
       
    }

    private void Start()
    {
        for (float i = boundarieL; i <= boundarieR; i += 0.5F)
        {
            spawnX.Add(i);
        }
    }

    public void SpawnStart()
    {
        if (GameManager.instance.currentLevel >= 10)
            spawnmode2 = true;

        if (!spawnmode2)
        {
            InvokeRepeating("Spawn", spawnStart, spawnRepeatSeconds);
        }
        else
        {
            InvokeRepeating("AfterLevelTen", spawnStart, spawnRepeatSeconds);
        }
    }

    private void Spawn()
    {
        int selectX = Random.Range(0,spawnX.Count);
        int oldX = selectX;
        int selectXDist = Random.Range(1, 4);


        if (!Classic)
        {
            Instantiate(projectile, new Vector3(spawnX[selectX], spawnY, 0), Quaternion.identity);
        }
        else
        {

                //bereken hoeveel pijlen tussen 1 en 3
                int arrowAmount = Random.Range(0, 3);

                for (int i = 0; i <= arrowAmount; i++)
                {

                    //meer dan 1 pijl
                    if (i > 0)
                    {

                        //2 Pijlen
                        if (i == 1)
                        {
                            // -
                            selectX = oldX - selectXDist;
                        }
                        //3 pijlen
                        if (i == 2)
                        {
                            //+
                            selectX = oldX + selectXDist;
                        }

                        //controleer out of boundaries
                        if (selectX >= spawnX.Count)
                        {
                            selectX = oldX - (selectXDist * 2);
                        }
                        else if (selectX <= 0)
                        {
                            selectX = oldX + (selectXDist * 2);
                        }
                    }


                    Instantiate(projectile, new Vector3(spawnX[selectX], spawnY, 0), Quaternion.identity);
                }
        
        }
    }
    void AfterLevelTen()
    {
        int hole = Random.Range(0, spawnX.Count);

        int hole2 = hole;
        if (hole - 1 > 0)
            hole2 = hole - 1;
        else
            hole2 = hole + 2;

        int hole3 = hole;
        if (hole + 1 < spawnX.Count)
            hole3 = hole + 1;
        else
            hole3 = hole - 2;
        
        for (int i = 0 ; i < spawnX.Count; i++)
        {
            if(i != hole && i != hole2 && i != hole3)
                Instantiate(projectile, new Vector3(spawnX[i], spawnY, 0),Quaternion.identity);
           
        }
    }

    public void SpawnStop()
    {
        if (!spawnmode2)
            CancelInvoke("Spawn");
        else
            CancelInvoke("AfterLevelTen");
    }

}
