using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadStop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StopRoll());
    }
    IEnumerator StopRoll()
    {
        yield return new WaitForSeconds(3);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        Destroy(this);
    }

}
