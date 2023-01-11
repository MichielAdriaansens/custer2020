using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;

    //Playa
    public RuntimeAnimatorController playerAnimCtrl2;
    public Sprite playerSprite2;
    public GameObject playerRagdoll2;
    //Indjun
    public RuntimeAnimatorController indjunAnimCtrl2;
    public Sprite indjunSprite2;
    public GameObject indjunDeath2;
    //Background
    public Sprite bg1;
    public Sprite bg2;
    public Sprite ufo;
    //Projectile
    public GameObject arrow2;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Start()
    {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        GameObject _indjun = GameObject.FindGameObjectWithTag("Enemy");
        if (GameManager.instance.levelMode == 1)
        {
            //player change Animator Controller
            _player.GetComponentInChildren<Animator>().runtimeAnimatorController = playerAnimCtrl2;
            //player change renderer.sprite
            _player.GetComponentInChildren<SpriteRenderer>().sprite = playerSprite2;
            //player change ragdoll
            GameManager.instance.playerRagdoll = playerRagdoll2;

            //indjun change Animator Ctrl
            _indjun.GetComponentInChildren<Animator>().runtimeAnimatorController = indjunAnimCtrl2;
            //indjun change renderer.Sprite
            _indjun.GetComponentInChildren<SpriteRenderer>().sprite = indjunSprite2;
            //indjun change Death
            _indjun.GetComponent<IndjunScript>().corpse = indjunDeath2;

            //Background change Cloud
            GameObject.FindWithTag("AnimBG").GetComponent<SpriteRenderer>().sprite = ufo;
            //Background change main background
            GameObject.FindWithTag("BG").transform.Find("Background01").GetComponent<SpriteRenderer>().sprite = bg1;
            //Background change gethit background
            GameObject.FindWithTag("BG").transform.Find("Background02").GetComponent<SpriteRenderer>().sprite = bg2;

            //projectile change
            ProjSpawner.instance.projectile = arrow2;

        }

        Destroy(this.gameObject);
    }
}
