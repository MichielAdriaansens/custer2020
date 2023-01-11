using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitDestroy());
    }
    IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(this.gameObject);
    }
}
