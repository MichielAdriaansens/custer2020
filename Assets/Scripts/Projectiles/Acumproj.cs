using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acumproj : MonoBehaviour
{
    public GameObject spark;
    float moveSpeed = 1.5f;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Ecol" && !CusterCtrl.instance.dead)
        {
            AudioManager.instance.PlaySound("Ehit01");
            GameObject _spark = Instantiate(spark);
            _spark.transform.position = this.transform.position;

            Destroy(this.gameObject);
        }
    }
    void Move()
    {
        transform.position = transform.position + (Vector3.down * moveSpeed) * Time.deltaTime;
    }
    // Update is called once per frame
    void Update()
    {
        Move();

        if(transform.position.y < -0.4f)
        {
            Destroy(this.gameObject);
        }
    }
}
