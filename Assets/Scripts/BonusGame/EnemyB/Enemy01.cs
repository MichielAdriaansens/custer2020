using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01 : MonoBehaviour
{
    public GameObject _dead;
    public GameObject _hitFx;
    public GameObject Target;

    public bool changeColor = true;

    [Header("AI ctrl")]
    public bool _fall = false;
    public bool _zigZag = false;
    public bool _target = false;

    public float speed = 2;
    public float zigZagSpeed = 2;

    float borderL = -2.8f;
    float borderR = 2.8f;

    public bool _goLeft = false;
    bool goLeft()
    {
        int rng = Random.Range(0, 2);
      
        if(rng == 1)
        {
            return true;
        }

        return false;
    }

    bool showChaseMode = false;

    //for Target and keep moving after custer died
    Vector2 moveDir;

    private void Start()
    {
        Target = GameObject.FindWithTag("Player");
       // StartCoroutine(DestoryAfterTime(5));
        _goLeft = goLeft();

        ColorSwitch(0, 0, 0);

        if (_zigZag)
        {
            speed = 1.75f;

            if(changeColor)
                ColorSwitch(181, 28, 150);
        }
        if (_target)
        {
            //red
        }

    }
    void ColorSwitch(byte r,byte g, byte b)
    {
        Color32 newColor = GetComponent<SpriteRenderer>().color;

        newColor.r = r;
        newColor.g = g;
        newColor.b = b;

        GetComponent<SpriteRenderer>().color = newColor;
        _dead.transform.Find("breakL").GetComponent<SpriteRenderer>().color = newColor;
        _dead.transform.Find("breakR").GetComponent<SpriteRenderer>().color = newColor;
    }

    void Update()
    {
        OffScreen();
        if (BonusGameManager.instance.gameIsPlaying)
        {
            if (_fall)
                Fall();

            if (_zigZag)
                ZigZag();
            if(_target)
            {
                ChaseTarget();
            }
        }
    }

    void ChaseTarget()
    {
        Vector2 dir = Vector2.zero;
        if (!CusterCtrl.instance._passOut)
        {
            dir = Target.transform.position - transform.position;
        }
        else
        {
            if(GameObject.FindWithTag("Ragdoll") != null)
            {
                dir = GameObject.FindWithTag("Ragdoll").transform.Find("torso_ragdoll_Player").position - transform.position;
            }
        }


        if (!CusterCtrl.instance.dead)
        {
            moveDir = dir;
        }


        transform.Translate((moveDir.normalized * speed) * Time.deltaTime);

        if (!showChaseMode)
        {
            showChaseMode = true;
            ColorSwitch(210,51,36);
        }
    }
    void Fall()
    {
        transform.position = transform.position + (Vector3.down * speed) * Time.deltaTime;
        
    }
    void ZigZag()
    {
        if(_goLeft == true)
        {
            if (transform.position.x > borderL)
            {
                transform.position = transform.position + (Vector3.left * zigZagSpeed) * Time.deltaTime;
            }
            else
            {
                _goLeft = false;
            }
        }
        else
        {
            if(transform.position.x < borderR)
            {
                transform.position = transform.position + (Vector3.right * zigZagSpeed) * Time.deltaTime;
            }
            else
            {
                _goLeft = true;
            }
        }
    }

    void OffScreen()
    {
        if(transform.position.y < -1.2)
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator DestoryAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }


    void DeadColor(GameObject obj)
    {
        Color32 newColor = GetComponent<SpriteRenderer>().color;

        obj.transform.Find("breakL").GetComponent<SpriteRenderer>().color = newColor;
        obj.transform.Find("breakR").GetComponent<SpriteRenderer>().color = newColor;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag == "Ecol" && !CusterCtrl.instance.dead)
        {
            GameObject die = Instantiate(_dead);
            DeadColor(die);
            die.transform.position = transform.position;
       
            GameObject fx = Instantiate(_hitFx);
            Vector3 newPos = this.transform.position;
            newPos.z = 0;

            fx.transform.position = newPos;


            Destroy(this.gameObject);

            BonusGameManager.instance.killScore++;
            AudioManager.instance.KillSoundB();
        }
        
    }
}
