using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCtrl : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.tag == "Projectile" && this.transform.tag == "HitBox")
        {
            GameManager.instance.PlayerDead();

            //bool avtivates how the object that hit should behave
            if (collision.transform.parent.GetComponent<ProjArrow>() != null)
            collision.transform.parent.GetComponent<ProjArrow>().bingo = true;

            AudioManager.instance.PlaySound("PlayerHit");

            Destroy(this);
        }
    }

}
