using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeath : MonoBehaviour
{
    float speed = 1.5f;
    public GameObject Target;
    Vector2 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void ChaseTarget()
    {
        Vector2 dir = Vector2.zero;
        if (!CusterCtrl.instance._passOut)
        {
            dir = Target.transform.position - transform.position;
        }
        else
        {
            if (GameObject.FindWithTag("Ragdoll") != null)
            {
                dir = GameObject.FindWithTag("Ragdoll").transform.Find("torso_ragdoll_Player").position - transform.position;
            }
        }


        if (!CusterCtrl.instance.dead)
        {
            moveDir = dir;
        }


        transform.Translate((moveDir.normalized * speed) * Time.deltaTime);


    }
    // Update is called once per frame
    void Update()
    {
        if (!CusterCtrl.instance.dead)
        {
            ChaseTarget();
        }
        else
        {
            transform.position = transform.position + (Vector3.up * speed) * Time.deltaTime;
        }
    }
}
