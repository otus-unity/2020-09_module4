using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBot : MonoBehaviour
{
    Animator anim;
    PlayerHealth health;
    PlayerAmmo ammo;
    BotUtility botUtility;

    void Awake()
    {
        anim = GetComponent<Animator>();
        botUtility = GetComponentInParent<BotUtility>();
        health = botUtility.GetComponent<PlayerHealth>();
        ammo = botUtility.GetComponentInChildren<PlayerAmmo>();
    }

    void Update()
    {
        anim.SetInteger("health", health.health);
        anim.SetInteger("ammo", ammo.ammo);
        anim.SetFloat("distanceToEnemy", botUtility.GetDistanceToClosestEnemy());
        anim.SetBool("navigating", botUtility.IsNavigating());
    }
}
