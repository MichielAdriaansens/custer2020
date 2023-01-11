using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CusterAnim : MonoBehaviour
{
    public static CusterAnim instance;

    Animator anim;
    SpriteRenderer sRenderer;

    Vector2 prevPos;
    public int _moveDir = 0;
    public bool isMoving = false;
    public bool faceLeft = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        anim = GetComponent<Animator>();
        sRenderer = GetComponent<SpriteRenderer>();
    }

    //Called from CusterCtrl Script
    public void DrinkAnim()
    {
        anim.SetBool("Drink", true);
        
    }

    void Walk()
    {
        if (_moveDir != 0)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        if (_moveDir == -1)
        {
            Flip(true);
        }
        else if(_moveDir == 1)
        {
            Flip(false);
        }
    }

   public void Flip(bool active)
    {
        if (active)
        {
            sRenderer.flipX = true;
            faceLeft = true;
        }
        else
        {
            sRenderer.flipX = false;
            faceLeft = false;
        }
    }

    private void Update()
    {
        _moveDir = CusterCtrl.instance.moveDir;
        Walk();

    }
}
