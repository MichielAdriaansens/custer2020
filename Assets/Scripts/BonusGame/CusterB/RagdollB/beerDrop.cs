using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beerDrop : MonoBehaviour
{
    private void Awake()
    {
        if (!CusterCtrl.instance.drinkForRag)
        {
            Destroy(this.gameObject);
        }

    }
    void Start()
    {
 
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

        Random.InitState(System.DateTime.Now.Millisecond);
        int randomX = Random.Range(0, 150);
        if (CusterAnim.instance.faceLeft)
        {
            randomX = randomX * -1;
        }

        float randomY = Random.Range(1, 3) * 100;

        Vector2 _force = new Vector2(randomX, randomY);
        rb.AddForce(_force, ForceMode2D.Force);

        int randomT = Random.Range(0, 4);
        rb.AddTorque(1, ForceMode2D.Force);

        StartCoroutine(DestroyObj());
    }
    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

}
