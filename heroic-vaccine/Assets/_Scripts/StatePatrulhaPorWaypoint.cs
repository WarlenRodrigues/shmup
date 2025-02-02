﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatrulhaPorWaypoint : State
{

    public Transform[] waypoints;
    SteerableBehaviour steerable;
    GameManager gm;

    public override void Awake()
    {
        base.Awake();

        Transition ToAtacando = new Transition();
        ToAtacando.condition = new ConditionDistLT(transform, GameObject.FindWithTag("Player").transform, 4.0f);
        ToAtacando.target = GetComponent<StateAtaque>();
        transitions.Add(ToAtacando);

        steerable = GetComponent<SteerableBehaviour>();
    }

    public void Start()
    {
        gm = GameManager.GetInstance();
        waypoints[0].position = transform.position;
        waypoints[1].position = GameObject.FindWithTag("Player").transform.position;
    }

    public void Update()
    {

        if (gm.gameState != GameManager.GameState.GAME) return;

        if (Vector3.Distance(transform.position, waypoints[1].position) > 0.1f)
        {
            Vector3 direction = waypoints[1].position - transform.position;
            direction.Normalize();
            steerable.Thrust(direction.x, direction.y);
        }
        else
        {
            waypoints[1].position = GameObject.FindWithTag("Player").transform.position;
        }
    }
}
