using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadInjunScript : MonoBehaviour
{
    Vector2 _force;
    float _torque;
    Rigidbody2D[] rigidbodies;

    // Start is called before the first frame update
    private void Start()
    {
         rigidbodies = GetComponentsInChildren<Rigidbody2D>();

        foreach (Rigidbody2D rb in rigidbodies)
        {
            rb.simulated = true;
            rb.gameObject.layer = 8;
            
            Random.InitState(System.DateTime.Now.Millisecond);
            float y = Random.Range(2f, 3f);
            Random.InitState(System.DateTime.Now.Millisecond);
            float x = Random.Range(-2, 2f);
            _force = new Vector2(x, y);

            rb.velocity = new Vector2(0,0);

            rb.AddForce(_force * 100);

            Random.InitState(System.Environment.TickCount);
            _torque = Random.Range(-20f, 20f);
            rb.AddTorque(_torque);

 
            StartCoroutine(DestroyIfMove(rb));
        }

    }
    //Destroy Objects OffScreen (or still moving)
    IEnumerator DestroyIfMove(Rigidbody2D _rb)
    {
        yield return new WaitForSeconds(3);
        if (_rb.velocity != new Vector2(0, 0))
        {
            Destroy(_rb.gameObject);
        }

    }
}
