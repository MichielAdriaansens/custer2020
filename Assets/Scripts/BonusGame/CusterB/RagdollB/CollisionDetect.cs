using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Ccol")
        {
            if(!CusterCtrl.instance.TimerIsActive && !CusterCtrl.instance.callTimer)
            {
              //  Debug.LogWarning("TimerCalled");
                CusterCtrl.instance.callTimer = true;
            }
          
        }

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            if(!CusterCtrl.instance.dead)
              CusterCtrl.instance.Death();
        }
    }
    
}
