using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjArrow : MonoBehaviour
{

    public GameObject hitFX;
    public float moveSpeed = 1;
    public Vector3 moveDir = new Vector3(-1,-1.5f,0);
    public bool bingo = false;

    

    void Awake()
    {
        if (hitFX == null)
            Debug.Log("No FX obj on Arrow");
    }

    void Move()
    {
        transform.position = transform.position + (moveDir * moveSpeed) * Time.deltaTime;
    }

    private void DestroyObj()
    {
        //over de grens destroy
        if (transform.position.y <= -0.2)
        {
            Destroy(gameObject);
        }

        //player lose Destroy behalve de pijl die raak is
        if (GameManager.instance.playerDied)
        {
            if (!bingo)
                Destroy(gameObject);
            else
                StartCoroutine(ArrowPenetrate()); 
        }

        //Player won round
        if (GameManager.instance.playerLevelUp)
        {
            Destroy(gameObject);
        }


    }

    IEnumerator ArrowPenetrate()
    {

        yield return new WaitForSeconds(0.01f); 

        var gore = Instantiate(hitFX, transform.Find("FXspawn").transform);
        gore.transform.parent = transform.Find("FXspawn");

        Destroy(this);
      
    }

    void Update()
    {
        
        Move();

        DestroyObj();
    }
}
