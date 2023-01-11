using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDetect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.parent.tag == "Ragdoll")
        {
            // Debug.Log(collision.name);
            Transform thisArrow = transform.parent.transform;

            if (GameManager.instance.levelMode == 0)
            {
                if (collision.transform.name != "Hat_ragdoll_Player")
                {

                    thisArrow.parent = collision.transform;

                    collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    Random.InitState(System.DateTime.Now.Millisecond);
                    int rng = Random.Range(0, 2);


                    if (rng == 0)
                    {
                        collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(-20, 50));
                    }
                    else if (rng == 1)
                    {
                        collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(-30, 75));
                    }


                    Destroy(this);

                }
                else
                {
                    collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    GameObject _parent = collision.transform.parent.gameObject;

                    thisArrow.parent = _parent.transform.Find("Head_ragdoll_Player").transform;

                    Destroy(this);
                }
            }
            else if(GameManager.instance.levelMode == 1)
            {
                thisArrow.parent = collision.transform;

                Rigidbody2D colRigid = collision.GetComponent<Rigidbody2D>();
                colRigid.velocity = Vector2.zero;

                if (collision.transform.name != "Head_ragdoll_Player")
                {
                    Random.InitState(System.DateTime.Now.Millisecond);
                    int rng = Random.Range(0, 2);


                    if (rng == 0)
                    {
                        colRigid.AddForce(new Vector2(-20, 50));
                    }
                    else if (rng == 1)
                    {
                        colRigid.AddForce(new Vector2(-30, 75));
                    }

                }
                else
                {
                    

                    Random.InitState(System.DateTime.Now.Millisecond);
                    int rngX = Random.Range(0, -9);
                    int rngY = Random.Range(0, 11);

                    colRigid.AddForce(new Vector2(rngX, rngY));

                    float rngT = Random.Range(0, 0.5f);

                    colRigid.AddTorque(rngT);
                }
                Destroy(this);

            }
        }
    }
}
