using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    Vector3 originalPos;

    public void Shake(float time,float force)
    {
        originalPos = transform.localPosition;
        StartCoroutine(ShakeStart(time,force));
    }
    IEnumerator ShakeStart(float t, float f)
    {
        while(t > 0)
        {
            float _x = Random.Range( -1, 1) * (f / 10);
            float _y = Random.Range(-1, 1) * (f / 10);

            transform.localPosition = new Vector3(_x, _y, originalPos.z);

            t -= 1 * Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;

    }
  
    
}
