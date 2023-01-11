using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    Rigidbody2D rb;

    public GameObject trail;
    GameObject newTrail;
    public TrailRenderer _tr;

    public CircleCollider2D _eCol; //collider that responds with enemy
    public CircleCollider2D _cCol; //Collider for picking up custer Custer


    float speed;
    Vector2 prevPos;

    public bool lineDown;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        _eCol.enabled = false;
        _cCol.enabled = false;
        
    }

    void LineStart()
    {

        if (Input.GetMouseButtonDown(0))
        {
            newTrail = Instantiate(trail);
            newTrail.transform.position = this.transform.position;
           // prevPos = rb.transform.position;

            lineDown = true;

            speed = 0;
            _eCol.enabled = false;
            _cCol.enabled = false;
        }
        else if (Input.GetMouseButtonUp(0) && lineDown)
        {
            newTrail.transform.parent = null;
            newTrail.GetComponent<TrailRenderer>().Clear();
            Destroy(newTrail, 0.1f);

            lineDown = false;
         //   prevPos = Vector2.zero;
            speed = 0;
        }


    }
    void LineHold()
    {
        if (lineDown)
        {
            Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            if (newTrail != null)
            {
                newTrail.transform.position = newPos;
                Vector2 curPos = rb.transform.position;

                if(prevPos != curPos)
                    speed = (prevPos - curPos).magnitude;

                prevPos = curPos;
            }
            _cCol.enabled = true;

            if (speed > 0.1f) //0.0001 for enemy's.
            {
                _eCol.enabled = true;
            }
            else
            {
                _eCol.enabled = false;
            }
        }
        else
        {
            _cCol.enabled = false;
            _eCol.enabled = false;
        }
       
    }

    void TouchCtrl()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                lineDown = true;
                Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                rb.position = newPos;

                prevPos = newPos;

                speed = 0;
                newTrail = Instantiate(trail);
                newTrail.transform.position = rb.position;

                //setColliders
            }
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                rb.position = newPos;
                if(newTrail != null)
                    newTrail.transform.position = rb.position;

                //set Colliders
                Vector2 curPos = rb.transform.position;

                if (prevPos != curPos)
                    speed = (prevPos - curPos).magnitude;

                prevPos = curPos;
                
                _cCol.enabled = true;

                if (speed > 0.1f) //0.0001 for enemy's.
                {
                    _eCol.enabled = true;
                }
                else
                {
                    _eCol.enabled = false;
                }

            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {

                if (newTrail != null)
                {
                    newTrail.transform.parent = null;
                    newTrail.GetComponent<TrailRenderer>().Clear();
                    Destroy(newTrail, 0.1f);
                }

                lineDown = false;
                _cCol.enabled = false;
                _eCol.enabled = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            //  LineStart();
            //  LineHold();
            TouchCtrl();
        }

    }
}
