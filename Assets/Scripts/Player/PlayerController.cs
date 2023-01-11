using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    PlayerAnimation playerAnim;

    public float moveSpeed;
    
    float moveSpeedBU;
    [HideInInspector]
    public Vector3 moveDir;
    bool keyboard = false;
    public int mobileCtrlType = 1; //movement> 0 = screen tap ctrl. 1 = button Ctrl
  
    public bool controlActive = false;
    [HideInInspector]
    public bool buttonMoveActive = false;
    bool buttonMoveLeft = false;

    bool touchOverUI;

    private void Start()
    {
        GameManager.instance.player = this.gameObject;

        playerAnim = GetComponentInChildren<PlayerAnimation>();
        
        //movespeedBackUp voor boundaries 
        moveSpeedBU = moveSpeed;

        if (!GameManager.instance.mobileMode) { keyboard = true; }
    }

    void Movement()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0f,0f);

        transform.position = transform.position + (moveDir * moveSpeed) * Time.deltaTime;

        
    }

    void TouchMove()
    {

        if (Input.GetMouseButton(0) && !touchOverUI)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePos.x > 0)
            {
                float newX = transform.position.x + moveSpeed * Time.deltaTime;
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);

                moveDir.x = 1;
            }
            if (mousePos.x < 0)
            {
                float newX = transform.position.x + (moveSpeed * -1) * Time.deltaTime;
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);

                moveDir.x = -1;
            }
        }
        else
        {
            moveDir.x = 0;
        }
    }

    public void ButtonCtrl(bool goLeft)
    {
        buttonMoveActive = true;
        buttonMoveLeft = goLeft;


    }
    public void ButtonMoveStop()
    {
        buttonMoveActive = false;
        moveDir.x = 0;
    }

    //checks if mouse/touch is not over UI button
    void TouchOverUI() 
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            touchOverUI = true;
        }
        else
        {
            touchOverUI = false;
        }
    }

    public void SetPlayerSpeed(float speed)
    {
        moveSpeed = speed;
        moveSpeedBU = speed;
    }

    void Boundaries()
    {
        //Left X Boundary
        if (moveDir.x < 0.0f) {
            if (transform.position.x <= -2.8f)
            {
                if (moveSpeed != 0)
                    moveSpeedBU = moveSpeed;  

                moveSpeed = 0;
            }
            else
            {
                moveSpeed = moveSpeedBU;
            }
        }

        //Right X Boundary
        if (moveDir.x > 0.0f) {
            if (transform.position.x >= 2.0f)
            {
                if (moveSpeed != 0)
                    moveSpeedBU = moveSpeed;

                moveSpeed = 0;
            }
            else
            {
                moveSpeed = moveSpeedBU;
            }
        }
    }

    public void Attack01()
    {
        if(!GameManager.instance.playerDied)
             playerAnim.Attack01Anim();
    }

    void Update()
    {
        if (controlActive)
        {
            if (keyboard)
            {
                //Register Input Movekeys
                if (Input.GetAxis("Horizontal") != 0)
                {
                    Movement();
                }
                else
                {
                    moveDir.x = 0;
                }
                //Register Input Attack key
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Attack01();
                }
            }
            else
            {
                if (mobileCtrlType == 0)
                {
                    //Check if mouse/touch over a UI button
                    TouchOverUI();
                    //register Input for touchscreen
                    TouchMove();
                }
                else if (mobileCtrlType ==1)
                {
                    if (buttonMoveActive)
                    {
                        if (!buttonMoveLeft)
                        {
                            float newX = transform.position.x + moveSpeed * Time.deltaTime;
                            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

                            moveDir.x = 1;
                        }
                        else
                        {
                            float newX = transform.position.x + (moveSpeed * -1) * Time.deltaTime;
                            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

                            moveDir.x = -1;
                        }
                    }
                }
            }

            //prevents player walking off screen
            Boundaries();
        }
    }
}
