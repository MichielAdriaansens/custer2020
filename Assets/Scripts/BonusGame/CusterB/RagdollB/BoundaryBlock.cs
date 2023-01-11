using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryBlock : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject head;
    public BoxCollider2D floorCol;

    int forceMultiplier = 5;
    bool isClipping = false;
 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        floorCol = GameObject.FindWithTag("Floor").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CusterCtrl.instance.dead)
        {
            if (transform.position.x < -3) //Left block
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(5, 2), ForceMode2D.Impulse);
            }
            else if (transform.position.x > 3) //right block
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(-5, 2), ForceMode2D.Impulse);
            }

            if(head.transform.position.y <= -0.2) //Y Block
            {
                isClipping = true;
                Rigidbody2D headRb = head.GetComponent<Rigidbody2D>();

                headRb.velocity = Vector2.zero;
                headRb.AddForce(new Vector2(0, forceMultiplier), ForceMode2D.Impulse);
                forceMultiplier++;

                floorCol.enabled = false;
            }
            else
            {
                if (isClipping)
                {
                    floorCol.enabled = true;
                    forceMultiplier = 5;
                    isClipping = false;
                }
            }

        }
    }
}
