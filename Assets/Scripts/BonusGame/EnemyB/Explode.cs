using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    bool _left;
    public List<Rigidbody2D> rigidParts = new List<Rigidbody2D>();

    void Awake()
    {
        foreach (Transform part in transform.root)
        {
            if (part.GetComponent<Rigidbody2D>() != null)
            {
                Rigidbody2D rb = part.GetComponent<Rigidbody2D>();

                //eerste rigidBody in List moet altijd L zijn. Pas aan in prefab
                rigidParts.Add(rb);
            }
        }
    }
    private void Start()
    {
        Break();
    }
    void Break()
    {
        for (int i = 0; i < rigidParts.Count; i++)
        {
            rigidParts[i].velocity = new Vector2(0, 0);
            float randomForceX = 0;
            float randomTorque = 0;

            Random.InitState(System.DateTime.Now.Millisecond);
            if (i == 0) //Left
            {
                randomForceX = Random.Range(-1, -3);
                randomTorque = Random.Range(1, 15);
            }
            else if(i == 1)
            {
                randomForceX = Random.Range(1, 3);
                randomTorque = Random.Range(-1, -15);
            }


            Vector2 newforce = new Vector2(randomForceX, Random.Range(0, 3));
            rigidParts[i].AddForce(newforce, ForceMode2D.Impulse);
            rigidParts[i].AddTorque(randomTorque, ForceMode2D.Impulse);
        }


        Destroy(this.gameObject, 2);
    }
}
