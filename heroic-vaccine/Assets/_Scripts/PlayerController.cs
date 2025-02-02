﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SteerableBehaviour, IShooter, IDamageable
{
  
    Animator animator;
    private int lifes;
    private int points;
    public GameObject bullet;
    public Transform arma01;
    public float shootDelay = 0.05f;
    private float _lastShootTimestamp = 0.0f;
    public AudioClip shootSFX;
    GameManager gm;


    private void Start()
    {
        gm = GameManager.GetInstance();
        animator = GetComponent<Animator>();
    }

    public void Shoot()
    {
        if (Time.time - _lastShootTimestamp < shootDelay) return;
       
        AudioManager.PlaySFX(shootSFX);
        _lastShootTimestamp = Time.time;
        Instantiate(bullet, arma01.position, Quaternion.identity);    
    }

    public void TakeDamage()
    {
       gm.vidas--;
       if (gm.vidas <= 0){
        Die();
       }     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    if (collision.CompareTag("Inimigos"))
    {
        Destroy(collision.gameObject);
        TakeDamage();
    }
    }

    public void Die()
    {
        Destroy(gameObject);
        gm.ChangeState(GameManager.GameState.ENDGAME);
    }

    void FixedUpdate()
    {
        if (gm.gameState != GameManager.GameState.GAME) return;

        if (gm.pontos >= 10000)
        {
            gm.ChangeState(GameManager.GameState.ENDGAME);
        }

        if(Input.GetKeyDown(KeyCode.Escape) && gm.gameState == GameManager.GameState.GAME) {
            gm.ChangeState(GameManager.GameState.PAUSE);
        }

        if(Input.GetAxisRaw("Jump") != 0)
        {
           Shoot();
        }

        float yInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");
        Thrust(xInput, yInput);
        if (yInput != 0 || xInput != 0)
        {
            animator.SetFloat("Velocity", 1.0f);
        }
        else
        {
            animator.SetFloat("Velocity", 0.0f);
        }

    }    
}
