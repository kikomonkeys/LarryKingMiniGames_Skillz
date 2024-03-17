using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private Rigidbody rb;

    private float currentTime;

    private bool smash, invincible;

    private int currentBrokenStacks, totalStacks;

    public GameObject invicnbleObj;
    public Image invincibleFill;
    public GameObject fireEffect, winEffect, splashEffect;
    public Transform visual;
    public enum PlayerState
    {
        Prepare,
        Playing,
        Died,
        Finish,
        pause,
        none
    }

    [HideInInspector]
    public PlayerState playerState = PlayerState.Prepare;

    public AudioClip bounceOffClip, deadClip, winClip, destoryClip, iDestroyClip;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentBrokenStacks = 0;
    }

    void Start()
    {
        totalStacks = FindObjectsOfType<StackController>().Length;
    }
    void Update()
    {
        if (!GameUI.instance.starttimer)
            return;
        if (playerState == PlayerState.Playing)
        {
            if (Input.GetMouseButtonDown(0) && GameUI.instance.starttimer)
            {
                smash = true;
            }


            if (Input.GetMouseButtonUp(0))
            {
                smash = false;
            }
            //Debug.Log("smash::" + smash);
            if (invincible)
            {
                currentTime -= Time.deltaTime * .35f;
                if (!fireEffect.activeInHierarchy)
                    fireEffect.SetActive(true);
            }
            else
            {
                if (fireEffect.activeInHierarchy)
                    fireEffect.SetActive(false);

                if (smash)
                    currentTime += Time.deltaTime * .4f;
                else
                    currentTime -= Time.deltaTime * .5f;
            }

            if (currentTime >= 0.3f || invincibleFill.color == Color.red)
                invicnbleObj.SetActive(true);
            else
                invicnbleObj.SetActive(false);

            if (currentTime >= 1)
            {
                currentTime = 1;
                invincible = true;
                invincibleFill.color = Color.red;
            }
            else if (currentTime <= 0)
            {
                currentTime = 0;
                invincible = false;
                invincibleFill.color = Color.white;
            }

            if (invicnbleObj.activeInHierarchy)
                invincibleFill.fillAmount = currentTime / 1;

        }

        if (playerState == PlayerState.Finish)
        {
           Invoke("LoadNextLevel",.5f);
               
        }

        if (hit)
        {
            visual.transform.localScale =  Vector3.Lerp(visual.transform.localScale,new Vector3( visual.transform.localScale.x, .3f, visual.transform.localScale.z),.4f);
        }
        else
        {
            visual.transform.localScale =  Vector3.Lerp(visual.transform.localScale,new Vector3( visual.transform.localScale.x, 1.3f, visual.transform.localScale.z),.1f);
        }

    }

    private bool hit;

    void LoadNextLevel()
    {
        FindObjectOfType<LevelSpawner>().NextLevel();
        GameUI.isLoadNextLevel = true;
    }

    void FixedUpdate()
    {
        if(playerState == PlayerState.Playing)
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                smash = true;
                rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 3, 0);
                visual.transform.localScale =  Vector3.Lerp(visual.transform.localScale,new Vector3( visual.transform.localScale.x, .7f, visual.transform.localScale.z),.4f);
            }
        }
        

        if (rb.velocity.y > 5)
            rb.velocity = new Vector3(rb.velocity.x, 5, rb.velocity.z);
    }

    public void IncreaseBrokenStacks()
    {
        currentBrokenStacks++;
        if (!invincible)
        {
            if (PlayerPrefs.GetInt("Level") == 0)
            {
                Stackball_GameStake.ScoreManager.instance.AddScore(PlayerPrefs.GetInt("Level") + 1);
            }
            else
            {
                Stackball_GameStake.ScoreManager.instance.AddScore(PlayerPrefs.GetInt("Level"));
            }
            Stackball_GameStake.SoundManager.instance.PlaySoundFX(destoryClip, 0.5f);
        }
        else
        {
            if (PlayerPrefs.GetInt("Level") == 0)
            {
                Stackball_GameStake.ScoreManager.instance.AddScore(2 * (PlayerPrefs.GetInt("Level") + 1));
            }
            else
            {
                Stackball_GameStake.ScoreManager.instance.AddScore(2 * PlayerPrefs.GetInt("Level"));
            }
            Stackball_GameStake.SoundManager.instance.PlaySoundFX(iDestroyClip, 0.5f);
        }
    }

    void OnCollisionEnter(Collision target)
    {
        
        if (!smash)
        {
            rb.velocity = new Vector3(0, 100 * Time.deltaTime * 50, 0);
            hit = true;
            if(target.gameObject.tag != "Finish")
            {
                GameObject splash = Instantiate(splashEffect);
                splash.transform.SetParent(target.transform);
           
                splash.transform.localEulerAngles = new Vector3(90, Random.Range(0, 359), 0);
                float randomScale = Random.Range(0.18f, 0.25f);
                splash.transform.localScale = new Vector3(randomScale, randomScale, 1);
                splash.transform.position = new Vector3(transform.position.x, transform.position.y-.08f , transform.position.z);
                splash.GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
            }

            Stackball_GameStake.SoundManager.instance.PlaySoundFX(bounceOffClip, 0.5f);
        }
        else
        {
            if (invincible)
            {
                if (target.gameObject.tag == "enemy" || target.gameObject.tag == "plane")
                {
                    target.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
            }
            else
            {
                if (target.gameObject.tag == "enemy" && !hit)
                {
                    //Debug.LogError("shatter");
                    target.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }

                if (target.gameObject.tag == "plane")
                {
                    rb.isKinematic = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    //playerState = PlayerState.Died;
                    //GameUI.instance.EnableGameoverPage();
                    //tartCoroutine(ShowLCOption(0f));
                    GameUI.instance.isLostLives = true;
                    GameUI.instance.isTimeUp = false;
                    GameUI.instance.starttimer = false;
                    GameUI.instance.ShowLcOptionNow();
                    Stackball_GameStake.SoundManager.instance.PlaySoundFX(deadClip, 0.5f);
                }
            }
            
        }

        FindObjectOfType<GameUI>().LevelSliderFill(currentBrokenStacks / (float)totalStacks);

        if(target.gameObject.tag == "Finish" && playerState == PlayerState.Playing)
        {
            playerState = PlayerState.Finish;
           // SoundManager.instance.PlaySoundFX(winClip, 0.7f);
          //  GameObject win = Instantiate(winEffect);
          //  win.transform.SetParent(Camera.main.transform);
         //   win.transform.localPosition = Vector3.up * 1.5f;
          //  win.transform.eulerAngles = Vector3.zero;
        }
    }

    void OnCollisionStay(Collision target)
    {
        if (!smash || target.gameObject.tag == "Finish")
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            hit = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (!smash)
        {
            hit = false;
        }
    }
}
